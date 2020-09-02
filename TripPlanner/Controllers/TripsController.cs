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
        public TripsController(TripPlannerDbContext Context, IConfiguration configuration)
        {
            _context = Context;
            zd = new ZipCodeDAL(configuration);
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
                return RedirectToAction("TripInfo", new { zip });
            }
            else
            {
                var zips = (await zd.GetZip(city, state)).zip_codes.ToList();
                zip = zips[0];
                return RedirectToAction("TripInfo", new { zip });
            }
        }

        [HttpGet]
        public async Task<IActionResult> TripInfo(string zip)
        {
            Covid result = await cd.GetCovid(zip);
            return View(result);
        }


    }
}