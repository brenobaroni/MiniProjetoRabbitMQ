using Service.Interfaces;
using System;

namespace Service
{
    public class NotificationService : INotificationService
    {
        public void NotifyUser(int fromId, int toId, string content)
        {
            Console.WriteLine($"Message Received fromId: {fromId}, toId: {toId}.");
        }
    }
}
