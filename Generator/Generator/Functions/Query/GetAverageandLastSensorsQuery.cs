using Generator.Entity;
using Generator.Functions.DTO;
using MediatR;
using System.Collections.Generic;

namespace Generator.Functions.Query
{
    public class GetAverageandLastSensorsQuery
        : IRequest<IEnumerable<SensorLastAndAvergeDTO>>
    {

    }
}