using Eyup.Helpers;
using Eyup.Model;
using Eyup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Eyup.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ChatPage : Page, INotifyPropertyChanged
    {
        private AppContact appContact;

        public AppContact AppContact
        {
            get { return appContact; }
            set
            {
                appContact = value;
                OnPropertyChanged("AppContact");
            }
        }
        
        public static event EventHandler<AppContactEventArgs> AvatarTapped;

        public event PropertyChangedEventHandler PropertyChanged;

        public ChatPage()
        {
            this.InitializeComponent();
            Loaded += ChatPage_Loaded;
        }

        private void ChatPage_Loaded(object sender, RoutedEventArgs e)
        {
            MyChatService.Current.ChatMessageReceived += Current_ChatMessageReceived;
        }

        private async void Current_ChatMessageReceived(object sender, ChatMessage e)
        {
            await CoreWindow.GetForCurrentThread().Dispatcher.RunAsync(CoreDispatcherPriority.Low, () => { AppContact.ChatHistory.Add(e); }); 
        }

        private void ChatTextBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if(e.Key == Windows.System.VirtualKey.Enter)
            {
                ChatMessage sendMessage = new ChatMessage { Text = ChatTextBox.Text, SenderOrReceiver = ChatMessage.SenderOrReceiverEnum.Sender };
                AppContact.ChatHistory.Add(sendMessage);
                ChatTextBox.Text = string.Empty;
                e.Handled = true;
                MyChatService.Current.SendChatMessage(sendMessage);
            }
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            var navigationParameter = e.Parameter as NavigationParameter;
            AppContact = navigationParameter.AppContact;

            if(navigationParameter.ShareTargetActivatedEventArgs != null)
            {
                var shareOperation = navigationParameter.ShareTargetActivatedEventArgs.ShareOperation;

                if(shareOperation.Data.Contains(StandardDataFormats.StorageItems))
                {
                    var storageItems = await shareOperation.Data.GetStorageItemsAsync();
                    var storageFile = storageItems[0] as StorageFile;
                                        
                    var randomAccessStream = await storageFile.OpenAsync(FileAccessMode.Read);
                    BitmapImage bitmapImage = new BitmapImage();
                    bitmapImage.SetSource(randomAccessStream);
                    
                    ChatMessage sendMessage = new ChatMessage { Image = bitmapImage, SenderOrReceiver = ChatMessage.SenderOrReceiverEnum.Sender };
                    AppContact.ChatHistory.Add(sendMessage);

                    MyChatService.Current.SendChatMessage(sendMessage);
                }

                if (shareOperation.Data.Contains(StandardDataFormats.WebLink))
                {
                    var uri = await shareOperation.Data.GetWebLinkAsync();

                    ChatMessage sendMessage = new ChatMessage { Text = uri.OriginalString, SenderOrReceiver = ChatMessage.SenderOrReceiverEnum.Sender };
                    AppContact.ChatHistory.Add(sendMessage);

                    MyChatService.Current.SendChatMessage(sendMessage);
                }

                await new MessageDialog("Content shared.").ShowAsync();

                shareOperation.ReportCompleted();
            }           
        }

        private void AvatarEllipse_Tapped(object sender, TappedRoutedEventArgs e)
        {
            AvatarTapped?.Invoke(this, new AppContactEventArgs(AppContact));
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
