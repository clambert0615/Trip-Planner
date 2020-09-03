using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using TripPlanner.Models;

namespace TripPlanner.Controllers
{
    public class TripsController : Controller
    {
        private readonly TripPlannerDbContext _context;
        private ZipCodeDAL zd;
        private CovidDAL cd = new CovidDAL();
        private PlacesDAL pd;
        public BasicInfo bi = new BasicInfo { Covid = new Covid(), CityState = new CityState(), ZipCode = new ZipCode(),
        Restaurants = new Eating()};
        public CityState cs = new CityState();
        public TripsController(TripPlannerDbContext Context, IConfiguration configuration)
        {
            _context = Context;
            zd = new ZipCodeDAL(configuration);
            pd = new PlacesDAL(configuration);
        }
        public IActionResult TripIndex()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetZip(string zip, string city, string state)
        {
            if(zip != null)
            {
                return RedirectToAction("TripInfo", new { zip, city, state });
            }
            else
            {
                var zips = (await zd.GetZip(city, state)).zip_codes.ToList();
                zip = zips[0];
                return RedirectToAction("TripInfo", new { zip, city, state });
            }
        }

        [HttpGet]
        public async Task<IActionResult> TripInfo(string zip)
        {
            Covid covid = await cd.GetCovid(zip);
             cs = await zd.GetCity(zip);

            bi.Covid = covid;
            bi.CityState.city = cs.city;
            bi.CityState.state = cs.state;
            bi.CityState.lat = cs.lat;
            bi.CityState.lng = cs.lng;
          
            return View(bi);

        }
        public async Task<IActionResult> Restaurants(float lat, float lng, string city)
        {
            var restaurants = (await pd.GetRestaurants(lat, lng)).results;
            bi.Restaurants.results = restaurants;
            bi.CityState.city = city;
            return View(bi);
        }


    }
}