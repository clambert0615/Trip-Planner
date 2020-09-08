using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TripPlanner.Models
{
    public class BasicInfo
    {
        public Covid Covid { get; set; }
        public CityState CityState { get; set; }
        public ZipCode ZipCode { get; set; }
        public Eating Restaurants { get; set; }
        public Attractions Attractions { get; set; }
        public Lodging Lodging { get; set; }
        public PlaceDetails Details { get; set; }
        public Places Places { get; set; }
    }
}
