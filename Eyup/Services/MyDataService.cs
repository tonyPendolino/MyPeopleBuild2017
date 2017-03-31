using Eyup.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows;
using Windows.Foundation;

namespace Eyup.Services
{
    public class MyDataService
    {
        private ObservableCollection<AppContact> SAMPLE_DATA;

        private static MyDataService current;

        public static MyDataService Current => current ?? ( current = new MyDataService() );

        private MyDataService()
        {
            BuildSampleData();
        }

        private void BuildSampleData()
        {
            // data populated from https://microsoft.sharepoint.com/sites/lcaweb/Pages/Applications/FictitiousNameFinder.aspx
            SAMPLE_DATA = new ObservableCollection<AppContact>
            {
                new AppContact { ContactId="1", FirstName = "Mia", LastName="Reynolds", Gender = AppContact.GenderOptions.Female, EmailAddress = "mia@contoso.com", TelephoneNumber = "+1-425-555-0101", PhotoUri = "ms-appx:///Images/f1.png" },
                new AppContact { ContactId="2", FirstName = "Sarah", LastName="Elliott", Gender = AppContact.GenderOptions.Female, EmailAddress = "sarah@contoso.com", TelephoneNumber = "+1-425-555-0102", PhotoUri = "ms-appx:///Images/f2.png" },
                new AppContact { ContactId="3", FirstName = "Abigail", LastName="Osborne", Gender = AppContact.GenderOptions.Female, EmailAddress = "abigail@contoso.com", TelephoneNumber = "+1-425-555-0103", PhotoUri = "ms-appx:///Images/f3.png" },
                new AppContact { ContactId="4", FirstName = "Demi", LastName="Faulkner", Gender = AppContact.GenderOptions.Female, EmailAddress = "demi@contoso.com", TelephoneNumber = "+1-425-555-0104", PhotoUri = "ms-appx:///Images/f4.png" },
                new AppContact { ContactId="5", FirstName = "Taylor", LastName="Thorpe", Gender = AppContact.GenderOptions.Male, EmailAddress = "taylor@contoso.com", TelephoneNumber = "+1-425-555-0105", PhotoUri = "ms-appx:///Images/m1.png" },
                new AppContact { ContactId="6", FirstName = "Lily", LastName="Parker", Gender = AppContact.GenderOptions.Female, EmailAddress = "lily@contoso.com", TelephoneNumber = "+1-425-555-0106", PhotoUri = "ms-appx:///Images/f5.png" },
                new AppContact { ContactId="7", FirstName = "Oscar", LastName="Doyle", Gender = AppContact.GenderOptions.Male, EmailAddress = "oscar@contoso.com", TelephoneNumber = "+1-425-555-0107", PhotoUri = "ms-appx:///Images/m2.png" },
                new AppContact { ContactId="8", FirstName = "William", LastName="Miller", Gender = AppContact.GenderOptions.Male, EmailAddress = "william@contoso.com", TelephoneNumber = "+1-425-555-0108", PhotoUri = "ms-appx:///Images/m3.png" },
                new AppContact { ContactId="9", FirstName = "Isobel", LastName="Stevens", Gender = AppContact.GenderOptions.Female, EmailAddress = "isobel@contoso.com", TelephoneNumber = "+1-425-555-0109", PhotoUri = "ms-appx:///Images/f6.png" },
                new AppContact { ContactId="10", FirstName = "Anthony", LastName="Marsden", Gender = AppContact.GenderOptions.Male, EmailAddress = "anthony@contoso.com", TelephoneNumber = "+1-425-555-0110", PhotoUri = "ms-appx:///Images/m4.png" }
            };
        }

        public ObservableCollection<AppContact> GetAllContacts()
        {
            return SAMPLE_DATA;
        }
    }
}
