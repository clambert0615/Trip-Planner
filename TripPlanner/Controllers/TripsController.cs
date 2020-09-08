using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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
        Restaurants = new Eating(), Attractions = new Attractions(), Lodging = new Lodging(), Details = new PlaceDetails(),
        Places = new Places()};
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
            var response = await pd.GetRestaurants(lat, lng);
            var token = response.next_page_token;
            var restaurants = response.results;
            bi.Restaurants.results = restaurants;
            bi.Places.next_page_token = token;
            bi.CityState.city = city;
            return View(bi);
        }

        public async Task<IActionResult> Attractions(float lat, float lng, string city)
        {
            var response = await pd.GetAttractions(lat, lng);
            var token = response.next_page_token;
            var attractions = response.results;
            bi.Attractions.results = attractions;
            bi.Places.next_page_token = token;
            bi.CityState.city = city;
            return View(bi);
        }

        public async Task<IActionResult> Lodging(float lat, float lng, string city)
        {
            var response = await pd.GetLodging(lat, lng);
            var token = response.next_page_token;
            var lodging = response.results;
            bi.Lodging.results = lodging;
            bi.Places.next_page_token = token;
            bi.CityState.city = city;
            return View(bi);
        }

        public async Task<IActionResult> Details(string id)
        {
            var details = (await pd.GetDetails(id)).result;
            bi.Details.result = details;
            
            //foreach(var photo in details.photos)
            //{
            //   await pd.GetPhotos(photo.photo_reference);

            //}
            return View(bi);
        }
        public async Task<IActionResult> GetMoreResults(string token)
        {
            var morePlaces = (await pd.MorePlaces(token));
            return View(morePlaces);
        }
        [Authorize]
        public IActionResult AddFavorite(string id)
        {
            string userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Favorites favorite = new Favorites();
            favorite.Destination = id;
            favorite.UserId = userid;
            if (_context.Favorites.Where(x => (x.Destination == id) && (x.UserId == userid)).ToList().Count > 0)
            {
                return RedirectToAction("GetFavorites");
            }
            if (ModelState.IsValid)
            {
                _context.Favorites.Add(favorite);
                _context.SaveChanges();
            }
            return RedirectToAction("GetFavorites");
        }
        public async Task<IActionResult> GetFavorites()
        {
            string userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            List<Favorites> favList = _context.Favorites.Where(x => x.UserId == userid).ToList();
            List<PlaceDetails> pdlist= await pd.GetFavoritesList(favList);
            return View(pdlist);
        }
        public IActionResult DeleteFavorite(string id)
        {
            string userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Favorites found = _context.Favorites.First(x =>(x.Destination == id) && x.UserId == userid);
            _context.Favorites.Remove(found);
            _context.SaveChanges();
            return RedirectToAction("GetFavorites");
        }
    }
}