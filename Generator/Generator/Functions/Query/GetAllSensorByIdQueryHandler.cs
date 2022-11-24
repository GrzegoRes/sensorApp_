using Generator.Entity;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace Generator.Functions.Query
{
    public class GetAllSensorByIdQueryHandler : IRequestHandler<GetAllSensorQueryById, IEnumerable<SensorDB>>
    {
        private IRepositorySensor _sensorRepository;
        
        public GetAllSensorByIdQueryHandler(IRepositorySensor repositorySensor)
        {
            this._sensorRepository = repositorySensor;
        }

        public Task<IEnumerable<SensorDB>> Handle(GetAllSensorQueryById request, CancellationToken cancellationToken)
        {
            var sensors = _sensorRepository.GetAllById(request.Id).Result.TakeLast(15);

            return Task.FromResult(sensors);
        }
    }
}
