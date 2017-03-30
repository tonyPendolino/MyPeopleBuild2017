using Eyup.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eyup.Services
{
    public class MyChatService
    {
        private static MyChatService current;

        public static MyChatService Current => current ?? (current = new MyChatService());

        public event EventHandler<ChatMessage> ChatMessageReceived;

        private MyChatService() { }

        public void SendChatMessage(ChatMessage chatMessage)
        {
            if(chatMessage?.Text.ToLower().Contains("hello") == true)
            {
                Task.Delay(1000).ContinueWith((t) => { ChatMessageReceived?.Invoke(this, new ChatMessage { SenderOrReceiver = ChatMessage.SenderOrReceiverEnum.Receiver, Text = "Hi! How are you?" }); });
            }
        }
    }
}
