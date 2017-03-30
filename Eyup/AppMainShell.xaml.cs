using Eyup.Model;
using Eyup.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Contacts;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Threading.Tasks;
using Windows.UI.Popups;
using System.ComponentModel;
using System.Collections.ObjectModel;
using Windows.ApplicationModel.Activation;
using System.Diagnostics;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Core;
using Eyup.Views;
using Eyup.Helpers;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Eyup
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AppMainShell : Page, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public Frame AppFrame { get { return this.AppMainShellFrame; } }

        public static AppMainShell Current = null;

        public AppMainShell()
        {
            this.InitializeComponent();

            SystemNavigationManager.GetForCurrentView().BackRequested += AppShell_BackRequested;
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;

            ChatPage.AvatarTapped += ChatPage_AvatarTapped;

            Loaded += MainPage_Loaded;
        }

        private void ChatPage_AvatarTapped(object sender, AppContactEventArgs e)
        {
            var parentFrame = (sender as Page).Parent as Frame;
            if (parentFrame.Name == "AppMainShellFrame")
            {
                AppFrame.Navigate(typeof(ProfilePage), e.AppContact);
            }
        }

        private void AppShell_BackRequested(object sender, BackRequestedEventArgs e)
        {
            bool handled = e.Handled;

            if (this.AppFrame == null)
                return;
            
            if (this.AppFrame.CanGoBack && !handled)
            {
                handled = true;
                this.AppFrame.GoBack();
            }
        }

        public void NavigateToPage(NavigationParameter navigationParameter)
        {
            if (!string.IsNullOrEmpty(navigationParameter?.ContactRemoteIds))
            {
                var selectedAppContact = (from c in App.AppContacts where c.ContactId == navigationParameter.ContactRemoteIds select c).FirstOrDefault();
                ContactsPage.SelectedAppContact = selectedAppContact;

                switch (navigationParameter?.Scheme)
                {
                    case "ms-ipmessaging":
                        {
                            AppFrame.Navigate(typeof(ChatPage), selectedAppContact);
                        }
                        break;
                    case "ms-contact-profile":
                        {
                            AppFrame.Navigate(typeof(ProfilePage), selectedAppContact);
                        }
                        break;
                }
            }
        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            Current = this;
            ContactsPage.SelectedAppContactChanged += ContactsPage_SelectedAppContactChanged;
        }

        private void ContactsPage_SelectedAppContactChanged(object sender, AppContactEventArgs e)
        {
            AppFrame.Navigate(typeof(ChatPage), e.AppContact);
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
