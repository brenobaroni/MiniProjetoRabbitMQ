using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interfaces
{
    public interface INotificationService
    {
        void NotifyUser(int fromId, int toId, string content);
    }
}
