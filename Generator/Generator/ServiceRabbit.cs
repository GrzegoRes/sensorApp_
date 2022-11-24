using Generator.Entity;
using Generator.Functions.Query;
using Generator.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MongoDB.Bson;
using MongoDB.Driver;
using RabbitMQ.Client;
using RabbitMQ.Client.Core.DependencyInjection;
using RabbitMQ.Client.Core.DependencyInjection.Configuration;
using RabbitMQ.Client.Core.DependencyInjection.Services.Interfaces;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Generator.Properties
{
    public class ServiceRabbit : BackgroundService
    {
        private IServiceProvider _sp;
        private ConnectionFactory _factory;
        private IConnection _connection;
        private IModel _channel;
        private readonly IRepositorySensor _productRepository;
        private readonly IHubContext<StockDataHub> _hubContext;

        public ServiceRabbit(IServiceProvider sp, IRepositorySensor productRepository)
        {
            _sp = sp;

            _factory = new ConnectionFactory() { HostName = "localhost", Port = 5672 };

            _connection = _factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.QueueDeclare(
                queue: "sensor.01",
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);
            _productRepository = productRepository;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            if (stoppingToken.IsCancellationRequested)
            {
                _channel.Dispose();
                _connection.Dispose();
                return Task.CompletedTask;
            }

            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                Console.WriteLine(" [x] Received {0}", message);


                Task.Run(() =>
                {
                    var chunks = message.Split("|");

                    var message1 = new Message2();
                    if (chunks.Length == 4)
                    {
                        message1.sensorID = Int32.Parse(chunks[0]);
                        message1.name = chunks[1];
                        message1.type = chunks[2];
                        message1.value = Int32.Parse(chunks[3]);
                    }

                    //using (var scope = _sp.CreateScope())
                    //{
                    //    var db = scope.ServiceProvider.GetRequiredService<IHeroesRepository>();
                    //    db.Create(hero);
                    //}
                    // Save To DB
                    /*
                     * 
                     * 
                     * 
                     */
                    //var client = new MongoClient("mongodb://127.0.0.1:27017");
                    //var db = client.GetDatabase("sensor");
                    //var collection = db.GetCollection<BsonDocument>("sensor_1");

                    var rng = new Random();
                    //var document = new BsonDocument {
                    //    { "sensorID", message1.sensorID },
                    //    { "name", message1.name},
                    //    { "type", message1.type},
                    //    { "value", message1.value},
                    //    { "data", DateTime.Now.ToString() }
                    //};

                    var entity = new SensorDB()
                    {   
                        sensorID = message1.sensorID,
                        name = message1.name,
                        type = message1.type,
                        value = message1.value,
                        dateGenerate = DateTime.Now
                    };

                    _productRepository.Create(entity);
                    //collection.InsertOne(document);
                    
                    //Console.WriteLine($"{message1.sensorID}-{message1.name}-{message1.type}-{message1.value}");
                });
            };
            
            _channel.BasicConsume(queue: "sensor.02", autoAck: true, consumer: consumer);
            //autoAck: true -> Czy została obsłuzona usuwanie z 
            return Task.CompletedTask;
        }
    }
}
