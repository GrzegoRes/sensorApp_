using MongoDB.Driver;

namespace Generator
{
    public interface IMongoSensorDBContext
    {
        IMongoCollection<T> GetCollection<T>(string name);
    }
}