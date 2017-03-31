using Eyup.Model;
using Eyup.Services;
using Eyup.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Contacts;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Eyup
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AppContactPanelShell : Page
    {
        public Frame AppFrame { get { return this.AppContactPanelShellFrame; } }

        public static AppContactPanelShell Current = null;

        private string _appContactId = string.Empty;

        public AppContactPanelShell()
        {
            this.InitializeComponent();

            SystemNavigationManager.GetForCurrentView().BackRequested += AppContactPanelShell_BackRequested; ;
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;

            Loaded += AppContactPanelShell_Loaded;
        }

        private void AppContactPanelShell_BackRequested(object sender, BackRequestedEventArgs e)
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

        private void AppContactPanelShell_Loaded(object sender, RoutedEventArgs e)
        {
            Current = this;
        }

        public async void NavigateToPage(ContactPanelActivatedEventArgs args)
        {
            args.ContactPanel.LaunchFullAppRequested += ContactPanel_LaunchFullAppRequested;

            var lightContact = args.Contact;

            _appContactId = await MyContactStoreService.Current.GetRemoteIdForContactIdAsync(lightContact.Id);

            AppContact appContact = null;

            if (_appContactId != null)
            {
                appContact = (from a in App.AppContacts where a.ContactId == _appContactId select a).FirstOrDefault();
            }
            
            AppFrame.Navigate(typeof(ChatPage), appContact);
        }

        private async void ContactPanel_LaunchFullAppRequested(ContactPanel sender, ContactPanelLaunchFullAppRequestedEventArgs args)
        {
            LauncherOptions options = new LauncherOptions();
            options.TargetApplicationPackageFamilyName = "fa86aedc-86f1-4dfc-9ecf-04a41289f016_rkjxvw9zmwpp0";
            options.UI.PreferredPlacement = Windows.UI.Popups.Placement.Default;
            bool success = await Launcher.LaunchUriAsync(new Uri($"ms-contact-profile:?ContactRemoteIds={_appContactId}"), options);

            sender.ClosePanel();
            args.Handled = true;
        }
    }
}
