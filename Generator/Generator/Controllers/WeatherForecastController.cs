using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Bson;
using Generator.Properties;
using MediatR;
using Generator.Entity;
using Generator.Functions.Query;
using System.Web.Http.Cors;
using Generator.Functions.DTO;

namespace Generator.Controllers
{ 
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private static readonly string[] sensorType = new[]
        {

            "temperature","light","humidity","other"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IMediator _mediator;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet]
        [Route("/zapisz")]
        public String Get()
        {
            //https://www.mongodb.com/blog/post/quick-start-c-sharp-and-mongodb-creating-documents
            var client = new MongoClient("mongodb://127.0.0.1:27017");
            var db = client.GetDatabase("sensor");
            var collection = db.GetCollection<BsonDocument>("sensor_1");

            var rng = new Random();
            var document = new BsonDocument { 
                { "sensorID", 10000 }, 
                { "name", "Cz_temp_1"}, 
                { "type", sensorType[rng.Next(sensorType.Length)]},
                { "value", rng.Next(-20, 55)},
                { "data", DateTime.Now.ToString() }
            };

            collection.InsertOne(document);

            return document.ToJson();
        }


        [HttpGet]
        [Route("/pobierz")]
        public String GetCos()
        {

            var client = new MongoClient("mongodb://127.0.0.1:27017");
            var db = client.GetDatabase("sensor");
            var collection = db.GetCollection<BsonDocument>("sensor_1");

            var filter = Builders<BsonDocument>.Filter.Eq("type", "temperature");
            var value = collection.Find(filter).ToList();

            return value.ToJson();
        }


        [HttpGet]
        [Route("/test")]
        public async Task<IActionResult> Create()
        {
            _logger.LogInformation("work");
            return Accepted();
        }

        [HttpGet]
        [Route("/GetAllSensor")]
        public async Task<ActionResult<List<SensorDB>>> GetAll()
        {
            //var req = new GetAllSensorQuery{
            // nazwa = klasaOptiona.typ
            // }

            var result = await _mediator.Send(new GetAllSensorQuery());

            return await Task.FromResult(result.ToList());
        }

        [HttpGet]
        [Route("/GetAllSensor/{id}")]
        public async Task<ActionResult<List<SensorDB>>> GetInt(int id)
        {
            var request = new GetAllSensorQueryById()
            {
                Id = id
            };

            var result = await _mediator.Send(request);

            return await Task.FromResult(result.ToList());
        }

        [HttpGet]
        [Route("/GetAllSensor/test")]
        public async Task<ActionResult<List<SensorLastAndAvergeDTO>>> GetAll2()
        {
            var request = new GetAverageandLastSensorsQuery();

            var result = await _mediator.Send(request);

            return await Task.FromResult(result.ToList());
        }
    }
}
