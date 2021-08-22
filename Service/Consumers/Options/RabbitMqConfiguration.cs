using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Consumers.Options
{
    public class RabbitMqConfiguration
    {
        public string Host { get; set; }
        public string Queue { get; set; }
    }
}
