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
using Windows.ApplicationModel.DataTransfer.ShareTarget;
using Windows.ApplicationModel.DataTransfer;

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

        private ShareTargetActivatedEventArgs _shareTargetActivatedEventArgs;

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
                NavigationParameter navigationParameter = new NavigationParameter
                {
                    AppContact = e.AppContact
                };

                AppFrame.Navigate(typeof(ProfilePage), navigationParameter);
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

                navigationParameter.AppContact = selectedAppContact;

                switch (navigationParameter?.Scheme)
                {
                    case "ms-ipmessaging":
                        {
                            AppFrame.Navigate(typeof(ChatPage), navigationParameter);
                        }
                        break;
                    case "ms-contact-profile":
                        {
                            AppFrame.Navigate(typeof(ProfilePage), navigationParameter);
                        }
                        break;
                }
            }
        }

        public void NavigateToPageFromShare(ShareTargetActivatedEventArgs shareTargetActivatedEventArgs)
        {
            _shareTargetActivatedEventArgs = shareTargetActivatedEventArgs;

            if (_shareTargetActivatedEventArgs != null)
            {
                ContactPickerPopup.IsOpen = true;
            }
        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            Current = this;
            ContactsPage.SelectedAppContactChanged += ContactsPage_SelectedAppContactChanged;
            ContactPicker.AppContactPicked += ContactPicker_AppContactPicked;
        }

        private void ContactPicker_AppContactPicked(object sender, AppContactEventArgs e)
        {
            ContactPickerPopup.IsOpen = false;
            if(_shareTargetActivatedEventArgs != null)
            {
                NavigationParameter navigationParameter = new NavigationParameter
                {
                    AppContact = e.AppContact,
                    ShareTargetActivatedEventArgs = _shareTargetActivatedEventArgs
                };

                AppFrame.Navigate(typeof(ChatPage), navigationParameter);

                //var shareOperation = _shareTargetActivatedEventArgs.ShareOperation;

                //if (shareOperation.Data.Contains(StandardDataFormats.WebLink))
                //{

                //}

                //if (shareOperation.Data.Contains(StandardDataFormats.Bitmap))
                //{

                //}
            }
            _shareTargetActivatedEventArgs = null;
        }

        private void ContactsPage_SelectedAppContactChanged(object sender, AppContactEventArgs e)
        {
            NavigationParameter navigationParameter = new NavigationParameter
            {
                AppContact = e.AppContact
            };

            AppFrame.Navigate(typeof(ChatPage), navigationParameter);
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void ContactPickerPopup_Loaded(object sender, RoutedEventArgs e)
        {
            ContactPickerPopup.HorizontalOffset = (Window.Current.Bounds.Width - ContactPickerPopup.ActualWidth) / 2;
        }
    }
}
