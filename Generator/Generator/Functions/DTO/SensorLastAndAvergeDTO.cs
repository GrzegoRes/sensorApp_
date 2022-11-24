using System.Collections.Generic;

namespace Generator.Functions.DTO
{
    public class SensorLastAndAvergeDTO
    {
        public int sensorID { get; set; }

        public string name { get; set; }

        public string type { get; set; }

        //public List<int> lastTenValue {get; set;}
        public double lastTenValue { get; set; }
        public int lastValue { get; set;}
    }
}
