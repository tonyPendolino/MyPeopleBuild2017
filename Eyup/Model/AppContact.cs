using Eyup.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eyup.Model
{
    public class AppContact : NotifyObject
    {
        public enum GenderOptions { NotSpecified, Male, Female };

        public AppContact()
        {
            ChatHistory = new ObservableCollection<ChatMessage>();
        }

        private string contactId;

        public string ContactId
        {
            get { return contactId; }
            set
            {
                contactId = value;
                OnPropertyChanged("ContactId");
            }
        }
        
        private string firstName;

        public string FirstName
        {
            get { return firstName; }
            set
            {
                firstName = value;
                OnPropertyChanged("FirstName");
            }
        }

        private string lastName;

        public string LastName
        {
            get { return lastName; }
            set
            {
                lastName = value;
                OnPropertyChanged("LastName");
            }
        }

        private string emailAddress;

        public string EmailAddress
        {
            get { return emailAddress; }
            set
            {
                emailAddress = value;
                OnPropertyChanged("EmailAddress");
            }
        }

        private string telephoneNumber;

        public string TelephoneNumber
        {
            get { return telephoneNumber; }
            set
            {
                telephoneNumber = value;
                OnPropertyChanged("TelephoneNumber");
            }
        }

        private string photoUri;

        public string PhotoUri
        {
            get { return photoUri; }
            set
            {
                photoUri = value;
                OnPropertyChanged("PhotoUri");
            }
        }

        private GenderOptions gender;

        public GenderOptions Gender
        {
            get { return gender; }
            set
            {
                gender = value;
                OnPropertyChanged("Gender");
            }
        }

        private ObservableCollection<ChatMessage> chatHistory;

        public ObservableCollection<ChatMessage> ChatHistory
        {
            get { return chatHistory; }
            set
            {
                chatHistory = value;
                OnPropertyChanged("ChatHistory");
            }
        }

    }
}
