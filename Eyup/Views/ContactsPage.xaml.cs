using Eyup.Helpers;
using Eyup.Model;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Windows.UI.Xaml.Controls;
using System.Linq;
using Windows.ApplicationModel.Contacts;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;
using Windows.Foundation;
using System.Diagnostics;
using Eyup.Services;
using System.Threading.Tasks;

namespace Eyup.Views
{
    public sealed partial class ContactsPage : Page, INotifyPropertyChanged
    {
        public ContactsPage()
        {
            this.InitializeComponent();
            Loaded += ContactsPage_Loaded;
        }

        private void ContactsPage_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            AppContacts = App.AppContacts;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public event EventHandler<AppContactEventArgs> SelectedAppContactChanged;

        private PinnedContactManager _pinnedContactManager;

        private ObservableCollection<AppContact> appContacts;

        public ObservableCollection<AppContact> AppContacts
        {
            get { return appContacts; }
            set
            {
                appContacts = value;
                OnPropertyChanged("AppContacts");
            }
        }

        private AppContact selectedAppContact;

        public AppContact SelectedAppContact
        {
            get { return selectedAppContact; }
            set
            {
                if (selectedAppContact != value)
                {
                    selectedAppContact = value;
                    OnSelectedAppChanged();
                    OnPropertyChanged("SelectedAppContact");
                }
            }
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void OnSelectedAppChanged()
        {
            if (selectedAppContact != null)
            {
                SelectedAppContactChanged?.Invoke(this, new AppContactEventArgs(selectedAppContact));
            }
        }
        
        private async void ContactGrid_ContextRequested(UIElement sender, ContextRequestedEventArgs args)
        {
            _pinnedContactManager = PinnedContactManager.GetDefault();

            if (PinnedContactManager.IsSupported() && _pinnedContactManager.IsPinSurfaceSupported(PinnedContactSurface.Taskbar))
            {
                Grid grid = sender as Grid;
                AppContact appContact = grid.DataContext as AppContact;

                if (appContact != null)
                {
                    MenuFlyout menuFlyout = new MenuFlyout();
                    menuFlyout.Closed += MenuFlyout_Closed;

                    string menuText = $"{Constants.CONTACT_CONTEXT_MENU_TEXT_PIN} {appContact.FirstName}";
                    string menuTag = Constants.CONTACT_CONTEXT_MENU_TAG_PIN;

                    Contact contactToPinUnpin = await GetAggregateContactFromAppContactAsync(appContact);

                    if (_pinnedContactManager.IsContactPinned(contactToPinUnpin, PinnedContactSurface.Taskbar))
                    {
                        menuText = $"{Constants.CONTACT_CONTEXT_MENU_TEXT_UNPIN} {appContact.FirstName}";
                        menuTag = Constants.CONTACT_CONTEXT_MENU_TAG_UNPIN;
                    }

                    var menuFlyoutItem = new MenuFlyoutItem
                    {
                        Text = menuText,
                        Tag = menuTag
                    };

                    menuFlyoutItem.Click += MenuFlyoutItem_Click;
                    menuFlyout.Items.Add(menuFlyoutItem);

                    grid.ContextFlyout = menuFlyout;

                    Point point;
                    bool succeeded = args.TryGetPosition(grid, out point);

                    if (succeeded)
                    {
                        menuFlyout.ShowAt(grid, point);
                    }
                }
            }
        }

        private void MenuFlyout_Closed(object sender, object e)
        {
            var grid = (sender as MenuFlyout).Target as Grid;

            if(grid != null)
            {
                grid.ContextFlyout = null;
            }
        }

        private async Task<Contact> GetAggregateContactFromAppContactAsync(AppContact appContact)
        {
            var contactToPinUnpin = await MyContactStoreService.Current.ContactList.GetContactFromRemoteIdAsync(appContact.ContactId);

            // if we have got hold of the raw contact, then grab the parent aggregate contact instead
            if (contactToPinUnpin?.IsAggregate == false && !string.IsNullOrEmpty(contactToPinUnpin?.AggregateId))
            {
                contactToPinUnpin = await MyContactStoreService.Current.ContactStore.GetContactAsync(contactToPinUnpin.AggregateId);
            }

            return contactToPinUnpin;
        }

        private async void MenuFlyoutItem_Click(object sender, RoutedEventArgs e)
        {
            var menuFlyoutItem = sender as MenuFlyoutItem;
            var appContact = menuFlyoutItem?.DataContext as AppContact;

            if(appContact != null)
            {
                var contactToPinUnpin = await GetAggregateContactFromAppContactAsync(appContact);

                switch (menuFlyoutItem.Tag)
                {
                    case Constants.CONTACT_CONTEXT_MENU_TAG_PIN:
                        await _pinnedContactManager.RequestPinContactAsync(contactToPinUnpin, PinnedContactSurface.Taskbar);
                        break;
                    case Constants.CONTACT_CONTEXT_MENU_TAG_UNPIN:
                        await _pinnedContactManager.RequestUnpinContactAsync(contactToPinUnpin, PinnedContactSurface.Taskbar);
                        break;
                }
            }
        }
    }
}
