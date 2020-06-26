using System.Collections.Generic;

namespace RouteAPI.Models
{
    public class GetAllRoute
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string CreatorId { get; set; }
        public string CreatorUsername { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public int Rating { get; set; }
        public List<GetAllPlace> Places { get; set; }
    }
}
