using SmartSchoolsV2.Models;
using System;

namespace SmartSchoolsV2.Services
{
    public interface IChatService
    {
        event EventHandler NewMessageReceived;
        void OnMessageReceived(PushNotification message);
        void SendMessage(ChatMessage message);
    }
}
