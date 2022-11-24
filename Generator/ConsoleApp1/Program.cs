using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace ConsoleApp1
{
    internal class Program
    {
        private static readonly List<sensor> sensorType =
            new List<sensor> 
            { 
                new sensor() {sensorID = 1,name = "Cz_temp_1", type = "temperature" }, 
                new sensor() {sensorID = 2,name = "Cz_lig_1", type = "light" }, 
                new sensor() {sensorID = 3,name = "Cz_humi_1", type = "humidity" }, 
                new sensor() {sensorID = 4,name = "Cz_oth_1", type = "other" } 
            };
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "mymq", Port = 5672};
            factory.UserName = "guest";
            factory.Password = "guest";
            // 1. create connection
            using (var connection = factory.CreateConnection())

            // 2. create channel
            using (var channel = connection.CreateModel())
            {
                // 3. connect to the queue
                channel.QueueDeclare(queue: "sensor.02",
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                int index = 1;
                var rng = new Random();
                while (index <= 99999)
                {
                    var senor = sensorType[rng.Next(sensorType.Count)];
                    string message = $"{senor.sensorID}|{senor.name}|{senor.type}|{rng.Next(-15,15)}";
                    var body = Encoding.UTF8.GetBytes(message);

                    // push content into the queue 
                    channel.BasicPublish(exchange: "", routingKey: "sensor.02", basicProperties: null, body: body);
                    Console.WriteLine(" [x] Sent {0}", message); index++; Thread.Sleep(1000);
                }
            }
        }
    }
}
