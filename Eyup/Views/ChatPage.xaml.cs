using Eyup.Helpers;
using Eyup.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Eyup.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ChatPage : Page
    {
        public AppContact AppContact { get; set; }

        public static event EventHandler<AppContactEventArgs> AvatarTapped;

        public ChatPage()
        {
            this.InitializeComponent();
        }

        private void ChatTextBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if(e.Key == Windows.System.VirtualKey.Enter)
            {
                AppContact.ChatHistory.Add(ChatTextBox.Text);
                ChatTextBox.Text = string.Empty;
                e.Handled = true;
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            AppContact = e.Parameter as AppContact;
        }

        private void AvatarEllipse_Tapped(object sender, TappedRoutedEventArgs e)
        {
            AvatarTapped?.Invoke(this, new AppContactEventArgs(AppContact));
        }
    }
}
