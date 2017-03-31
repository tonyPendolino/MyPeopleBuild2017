using Eyup.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Contacts;
using Windows.Storage;

namespace Eyup.Services
{
    public class MyContactStoreService
    {
        private static MyContactStoreService current;

        public static MyContactStoreService Current => current ?? (current = new MyContactStoreService());

        public ContactList ContactList { get; set; }

        private ContactAnnotationList _contactAnnotationList;

        private List<AppContact> _appContacts = null;

        public ContactStore ContactStore { get; set; }

        public ContactAnnotationStore ContactAnnotationStore { get; set; }

        private bool _isInitialized = false;

        private MyContactStoreService()
        {
            
        }

        public async Task InitializeAsync(List<AppContact> appContacts, bool forceReinitialize = false)
        {
            if (!_isInitialized || forceReinitialize)
            {
                _appContacts = appContacts;
                await InitializeContactStoresAsync();

                if (forceReinitialize == true)
                {
                    await CleanUpContactListAsync();
                }

                await InitializeContactListAsync();
                await InitializeAnnotationListAsync();
                await AddAppContactsAndAnnotationsToStoreAsync();
            }
            _isInitialized = true;
        }

        public void Cleanup()
        {
            ContactList.ContactChanged -= _contactList_ContactChanged;
            ContactList = null;
            ContactStore = null;
            _isInitialized = false;
        }

        private async Task InitializeContactStoresAsync()
        {
            ContactStore = await ContactManager.RequestStoreAsync(ContactStoreAccessType.AppContactsReadWrite);
            ContactAnnotationStore = await ContactManager.RequestAnnotationStoreAsync(ContactAnnotationStoreAccessType.AppAnnotationsReadWrite);
        }

        private async Task CleanUpContactListAsync()
        {
            if (ContactStore != null)
            {
                var contactLists = await ContactStore.FindContactListsAsync();

                ContactList = (from c in contactLists where c.DisplayName == Constants.CONTACT_LIST_NAME select c).SingleOrDefault();

                if (ContactList != null)
                {
                    await ContactList.DeleteAsync();
                }
            }
        }

        private async Task InitializeContactListAsync()
        {
            if (ContactStore == null)
            {
                throw new Exception("Unable to get contacts store");
            }

            var contactLists = await ContactStore.FindContactListsAsync();

            ContactList = (from c in contactLists where c.DisplayName == Constants.CONTACT_LIST_NAME select c).SingleOrDefault();

            if (ContactList == null)
            {
                ContactList = await ContactStore.CreateContactListAsync(Constants.CONTACT_LIST_NAME);
                ContactList.OtherAppReadAccess = ContactListOtherAppReadAccess.Limited;
                await ContactList.SaveAsync();
            }

            ContactList.ContactChanged += _contactList_ContactChanged;
            ContactList.ChangeTracker.Enable();

        }

        private void _contactList_ContactChanged(ContactList sender, ContactChangedEventArgs args)
        {
            
        }

        private async Task InitializeAnnotationListAsync()
        {
            if (ContactAnnotationStore == null)
            {
                throw new Exception("Unable to get contact annotation store");
            }

            var contactAnnotationLists = await ContactAnnotationStore.FindAnnotationListsAsync();

            _contactAnnotationList = (from c in contactAnnotationLists select c).SingleOrDefault();

            if (_contactAnnotationList == null)
            {
                _contactAnnotationList = await ContactAnnotationStore.CreateAnnotationListAsync();
            }
        }

        private async Task AddAppContactsAndAnnotationsToStoreAsync()
        {
            foreach (var appContact in _appContacts)
            {
                Uri pictureUri = new Uri(appContact.PhotoUri);
                var pictureFile = await StorageFile.GetFileFromApplicationUriAsync(new Uri(appContact.PhotoUri));

                Contact contact = new Contact
                {
                    FirstName = appContact.FirstName,
                    LastName = appContact.LastName,
                    SourceDisplayPicture = pictureFile,
                    RemoteId = appContact.ContactId
                };

                ContactEmail email1 = new ContactEmail
                {
                    Address = appContact.EmailAddress
                };

                ContactPhone phone1 = new ContactPhone
                {
                    Number = appContact.TelephoneNumber
                };

                contact.Emails.Add(email1);
                contact.Phones.Add(phone1);
                
                if (ContactList != null)
                {
                    try
                    {
                        await ContactList.SaveContactAsync(contact);
                    }
                    catch(Exception ex)
                    {
                        Debug.WriteLine($"Problem saving contact. Name: {contact.Name}, Message: {ex.Message}.  Try flushing ContactList with await MyContactStoreService.Current.InitializeAsync(AppContacts.ToList(), true);");
                    }
                }

                ContactAnnotation contactAnnotation = new ContactAnnotation
                {
                    ContactId = contact.Id,
                    RemoteId = appContact.ContactId,
                    SupportedOperations = ContactAnnotationOperations.ContactProfile | 
                                            ContactAnnotationOperations.Message | 
                                            ContactAnnotationOperations.Share
                };

                contactAnnotation.ProviderProperties.Add("ContactPanelAppID", "fa86aedc-86f1-4dfc-9ecf-04a41289f016_rkjxvw9zmwpp0!App");
                contactAnnotation.ProviderProperties.Add("ContactShareAppID", "fa86aedc-86f1-4dfc-9ecf-04a41289f016_rkjxvw9zmwpp0!App");

                try
                {
                    if (!await _contactAnnotationList.TrySaveAnnotationAsync(contactAnnotation))
                    {
                        throw new Exception($"Unable to save contact annotation.");
                    }
                }
                catch(Exception ex)
                {
                    Debug.WriteLine($"Problem saving contact annotation. Name: {contact.Name}, Message: {ex.Message}. Try flushing ContactList with await MyContactStoreService.Current.InitializeAsync(AppContacts.ToList(), true);");
                }
            }
        }
        
        public async Task<string> GetRemoteIdForContactIdAsync(string contactId)
        {
            var fullContact = await ContactStore.GetContactAsync(contactId);

            var contactAnnotations = await ContactAnnotationStore.FindAnnotationsForContactAsync(fullContact);

            if (contactAnnotations.Count >= 0)
            {
                return contactAnnotations[0].RemoteId;
            }

            return string.Empty;
        }

    }
}
