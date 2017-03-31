using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using static Eyup.Model.ChatMessage;

namespace Eyup.Helpers
{
    public class SenderOrReceiverToHorizontalAlignmentConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            SenderOrReceiverEnum senderOrReceiverEnum =  (SenderOrReceiverEnum)value;

            switch(senderOrReceiverEnum)
            {
                case SenderOrReceiverEnum.Sender:
                    return HorizontalAlignment.Right;
                case SenderOrReceiverEnum.Receiver:
                    return HorizontalAlignment.Left;
            }

            return HorizontalAlignment.Center;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
