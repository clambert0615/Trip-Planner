using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TripPlanner.Models
{

    public class Covid
    {
        public string zipCd { get; set; }
        public County[] counties { get; set; }
    }

    public class County
    {
        public string countyName { get; set; }
        public Geo geo { get; set; }
        public Historicdata[] historicData { get; set; }
    }

    public class Geo
    {
        public float rightTopLatLong { get; set; }
        public float leftBottomLatLong { get; set; }
        public float leftTopLatLong { get; set; }
        public float rightBottomLatLong { get; set; }
    }

    public class Historicdata
    {
        public string date { get; set; }
        public int deathCt { get; set; }
        public int positiveCt { get; set; }
        public object recoveredCt { get; set; }
    }

}
