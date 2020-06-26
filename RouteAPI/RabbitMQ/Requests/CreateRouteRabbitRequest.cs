using RouteAPI.Models;
using System.Collections.Generic;

namespace RouteAPI.RabbitMQ.Requests
{
    public class CreateRouteRabbitRequest
    {
        public string UserId { get; set; }
        public string RouteName { get; set; }
        public List<Place> Places { get; set; }
    }
}
