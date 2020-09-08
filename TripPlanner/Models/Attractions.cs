using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TripPlanner.Models
{
    public class Attractions
    {
        public object[] html_attributions { get; set; }
        public Result[] results { get; set; }
        public string status { get; set; }
        public string next_page_token { get; set; }
    }
}
