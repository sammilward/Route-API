﻿using System.Collections.Generic;

namespace RouteAPI.Models
{
    public class GetAllPlace
    {
        public string PlaceId { get; set; }
        public string Name { get; set; }
        public string PhotoReference { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Rating { get; set; }
        public int NumberOfRatings { get; set; }
        public List<string> Types { get; set; }
    }
}
