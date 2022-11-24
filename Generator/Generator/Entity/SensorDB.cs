using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Generator.Entity
{
    public class SensorDB
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public int sensorID { get; set; }

        public string name { get; set; }

        public string type { get; set; }

        public int value { get; set; }

        public DateTime dateGenerate { get; set; }
    }
}
