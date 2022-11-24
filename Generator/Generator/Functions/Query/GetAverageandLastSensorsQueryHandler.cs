using Generator.Entity;
using Generator.Functions.DTO;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace Generator.Functions.Query
{
    public class GetAverageandLastSensorsQueryHandler : IRequestHandler<GetAverageandLastSensorsQuery, IEnumerable<SensorLastAndAvergeDTO>>
    {
        private IRepositorySensor _sensorRepository;
        
        public GetAverageandLastSensorsQueryHandler(IRepositorySensor repositorySensor)
        {
            this._sensorRepository = repositorySensor;
        }

        public Task<IEnumerable<SensorLastAndAvergeDTO>> Handle(GetAverageandLastSensorsQuery request, CancellationToken cancellationToken)
        {
            var sensors = _sensorRepository.GetAll();

            //var res = sensors.Result.GroupBy(x => (x.name, x.sensorID, x.type))
            //                    .OrderBy(x => x.Key.sensorID)
            //                    .Select(d => new SensorLastAndAvergeDTO
            //                    {
            //                        sensorID = d.Key.sensorID,
            //                        name = d.Key.name,
            //                        type = d.Key.type,
            //                        lastTenValue = d.TakeLast(3).Select(x => x.value).ToList(),
            //                        lastValue = d.Last().value
            //                    });


            var res = sensors.Result.GroupBy(x => (x.name, x.sensorID, x.type))
                               .OrderBy(x => x.Key.sensorID)
                               .Select(d => new SensorLastAndAvergeDTO
                               {
                                   sensorID = d.Key.sensorID,
                                   name = d.Key.name,
                                   type = d.Key.type,
                                   lastTenValue = d.TakeLast(3).Average(v => v.value),
                                   lastValue = d.Last().value
                               });

            return Task.FromResult(res);
        }
    }
}
