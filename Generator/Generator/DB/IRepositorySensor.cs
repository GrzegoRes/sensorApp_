using Generator.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Generator.Functions.Query
{
    public interface IRepositorySensor
    {
        Task<IEnumerable<SensorDB>> GetAll();
        Task Create(SensorDB sensor);
        Task<IEnumerable<SensorDB>> GetAllById(int sensorI);
    }
}