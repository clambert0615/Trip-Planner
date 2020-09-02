using System;
using System.Collections.Generic;

namespace TripPlanner.Models
{
    public partial class Favorites
    {
        public int FavId { get; set; }
        public string Destination { get; set; }
        public string UserId { get; set; }

        public virtual AspNetUsers User { get; set; }
    }
}
