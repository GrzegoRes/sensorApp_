using Generator.Entity;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Generator.Functions.Query
{
    public class RepostiorySensor : IRepositorySensor
    {
        private readonly IMongoSensorDBContext _mondoContext;
        private IMongoCollection<SensorDB> _dbCollection;
        public RepostiorySensor(IMongoSensorDBContext context)
        {
            _mondoContext = context;
            _dbCollection = _mondoContext.GetCollection<SensorDB>(typeof(SensorDB).Name);
        }
        public async Task Create(SensorDB sensor)
        {
            await _dbCollection.InsertOneAsync(sensor);
        }

        public async Task<IEnumerable<SensorDB>> GetAll()
        {
            var all = await _dbCollection.FindAsync(Builders<SensorDB>.Filter.Empty);
            return await all.ToListAsync();
        }

        public async Task<IEnumerable<SensorDB>> GetAllById(int sensorId)
        {
            //var objectId = new ObjectId(sensorId);

            FilterDefinition<SensorDB> filter = Builders<SensorDB>.Filter.Eq("sensorID", sensorId);

            return await _dbCollection.FindAsync(filter).Result.ToListAsync();

            //var all = await _dbCollection.FindAsync(Builders<SensorDB>.Filter.Eq);
            // await all.ToListAsync();
        }
    }
}
