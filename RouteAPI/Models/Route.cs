using System.Collections.Generic;

namespace RouteAPI.Models
{
    public class Route
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string CreatorId { get; set; }
        public string CreatorUsername { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public int Rating { get; set; }
        public List<Place> Places { get; set; }
        public double[] RouteCoords { get; set; }
        public bool UserLikes { get; set; }
    }
}
