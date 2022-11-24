using Generator.Entity;
using MediatR;
using System.Collections.Generic;

namespace Generator.Functions.Query
{
    public class GetAllSensorQueryById
        : IRequest<IEnumerable<SensorDB>>
    {
        public int Id { get; set; }
    }
}