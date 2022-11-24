using Generator.Entity;
using MediatR;
using System.Collections.Generic;

namespace Generator.Functions.Query
{
    public class GetAllSensorQuery
        : IRequest<IEnumerable<SensorDB>>
    {

    }
}