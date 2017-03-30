using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace Eyup.Model
{
    public class ChatMessage
    {
        public enum SenderOrReceiverEnum { NotSet, Sender, Receiver };

        public string Text { get; set; }

        public BitmapImage Image { get; set; }

        public SenderOrReceiverEnum SenderOrReceiver { get; set; }

    }
}
