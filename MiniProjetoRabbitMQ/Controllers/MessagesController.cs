using Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniProjetoRabbitMQ.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {

        private readonly ConnectionFactory _connectionFactory;
        private const string QUEUE_NAME = "messages";
        //private readonly RabbitMqConfiguration _rabbitMqConfiguration;

        public MessagesController()
        {
            _connectionFactory = new ConnectionFactory() { HostName = "localhost" };
        }

        [HttpPost]
        public IActionResult PostMessage([FromBody] MessageInputModel message)
        {
            //Main AMQP Model Connection 'Advanced Message Queuing Protocol'
            using (IConnection connection = _connectionFactory.CreateConnection())
            {
                //AMQP Model
                using (IModel channel = connection.CreateModel())
                {
                    channel.QueueDeclare(
                        queue: QUEUE_NAME,
                        durable: false,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null);

                    var stringfiedMessage = JsonConvert.SerializeObject(message);
                    //RabbitMQ needs the message to be in byte array
                    var bytesMessage = Encoding.UTF8.GetBytes(stringfiedMessage);

                    channel.BasicPublish(
                        exchange: "",
                        routingKey: QUEUE_NAME,
                        basicProperties: null,
                        body: bytesMessage);


                }
            }

            return Accepted();
        }


    }
}
