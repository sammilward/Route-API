using RouteAPI.Models;
using System.Collections.Generic;

namespace RouteAPI.RabbitMQ.Responses
{
    public class GetAllRoutesRabbitResponse
    {
        public bool FoundRoutes { get; set; }
        public List<GetAllRoute> Routes { get; set; }
    }
}
