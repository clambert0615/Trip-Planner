using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace TripPlanner.Models
{
    public class PlacesDAL
    {

        private readonly string APIKey;
        public PlacesDAL(IConfiguration configuration)
        {
            APIKey = configuration.GetSection("ApiKeys")["Google"];
        }

        public HttpClient GetHttpClient()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://maps.googleapis.com/maps/api/place/");
            return client;
        }

        public async Task<Eating> GetRestaurants(float latitude, float longitude)
        {
            HttpClient client = GetHttpClient();
            HttpResponseMessage response = await client.GetAsync($"nearbysearch/json?type=restaurant&key={APIKey}&location={latitude},{longitude}&radius=15000");
            //install-package Microsoft.AspNet.WebAPI.Client
            var restaurantjson = await response.Content.ReadAsStringAsync();
            Eating restaurant = JsonConvert.DeserializeObject<Eating>(restaurantjson);
            return restaurant;
        }
    }
}
