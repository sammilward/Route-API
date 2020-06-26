using RouteAPI.Models;
using System.Collections.Generic;

namespace RouteAPI.Contracts.V1.Requests
{
    public class CreateRouteRequest
    {
        public string RouteName { get; set; }
        public List<Place> Places { get; set; }
    }
}
