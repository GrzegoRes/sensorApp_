using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Generator
{
    public class MongoSensorDBContext : IMongoSensorDBContext
    {
        private IMongoDatabase _db { get; set; }
        private MongoClient _mongoClient {get; set;}
        public IClientSession Session { get; set; }

        public MongoSensorDBContext()
        {
            _mongoClient = new MongoClient("mongodb://127.0.0.1:27017");
            _db = _mongoClient.GetDatabase("sensorTest");
        }

        public IMongoCollection<T> GetCollection<T>(string name)
        {
            return _db.GetCollection<T>(name);
        }
    }
}
