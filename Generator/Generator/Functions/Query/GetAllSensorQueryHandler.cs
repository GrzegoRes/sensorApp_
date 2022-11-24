using Generator.Entity;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Generator.Functions.Query
{
    public class GetAllSensorQueryHandler : IRequestHandler<GetAllSensorQuery, IEnumerable<SensorDB>>
    {
        private IRepositorySensor _sensorRepository;
        
        public GetAllSensorQueryHandler(IRepositorySensor repositorySensor)
        {
            this._sensorRepository = repositorySensor;
        }

        public Task<IEnumerable<SensorDB>> Handle(GetAllSensorQuery request, CancellationToken cancellationToken)
        {
            var sensors = _sensorRepository.GetAll();

            return sensors;
        }
    }
}
