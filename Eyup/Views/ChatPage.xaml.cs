using Eyup.Helpers;
using Eyup.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
    using Windows.Data.Xml.Dom;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Notifications;
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
                if (ChatTextBox.Text == Constants.SHOULDER_TAP_MAGIC_PHRASE)
                {
                    SendBackShoulderTap();
                }

                AppContact.ChatHistory.Add(ChatTextBox.Text);
                ChatTextBox.Text = string.Empty;
                e.Handled = true;
            }
        }

        async private void SendBackShoulderTap()
        {
            await Task.Delay(3000);
            Debug.WriteLine("Shoulder Tap " + AppContact.FirstName);


            string shoulderTapString = @"<toast hint-people='mailto:" + AppContact.EmailAddress + @"'>
                <visual>
                    <binding template='ToastGeneric'>
                        <text> You got a shoulder tap!</text>
                        <text> Click to check it out</text>
                        <image src='https://static-asm.secure.skypeassets.com/pes/v1/emoticons/pizza/views/default_80_anim' />
                    </binding>
                    <binding template='ToastGeneric' experienceType='shoulderTap'>
                        <image src='https://static-asm.secure.skypeassets.com/pes/v1/emoticons/pizza/views/default_80_anim' 
                            spritesheet-src='https://static-asm.secure.skypeassets.com/pes/v1/emoticons/pizza/views/default_80_anim' 
                            spritesheet-height='80' spritesheet-fps='25' spritesheet-startingFrame='15' />
                    </binding>
                </visual>
            </toast>";

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(shoulderTapString);

            var notif = new ToastNotification(doc);
            ToastNotificationManager.CreateToastNotifier().Show(notif);


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
