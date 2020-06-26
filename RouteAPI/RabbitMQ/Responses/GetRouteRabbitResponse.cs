using RouteAPI.Models;

namespace RouteAPI.RabbitMQ.Responses
{
    public class GetRouteRabbitResponse
    {
        public bool FoundRoute { get; set; }
        public Route Route { get; set; }
    }
}
