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
        public async Task<Attractions> GetAttractions(float latitude, float longitude)
        {
            HttpClient client = GetHttpClient();
            HttpResponseMessage response = await client.GetAsync($"nearbysearch/json?type=tourist_attraction&key={APIKey}&location={latitude},{longitude}&radius=15000");
            //install-package Microsoft.AspNet.WebAPI.Client
            var attractionjson = await response.Content.ReadAsStringAsync();
            Attractions attraction = JsonConvert.DeserializeObject<Attractions>(attractionjson);
            return attraction;
        }

        public async Task<Lodging> GetLodging(float latitude, float longitude)
        {
            HttpClient client = GetHttpClient();
            HttpResponseMessage response = await client.GetAsync($"nearbysearch/json?type=lodging&key={APIKey}&location={latitude},{longitude}&radius=15000");
            //install-package Microsoft.AspNet.WebAPI.Client
            var lodgingjson = await response.Content.ReadAsStringAsync();
            Lodging lodging = JsonConvert.DeserializeObject<Lodging>(lodgingjson);
            return lodging;
        }

        public async Task<PlaceDetails> GetDetails(string id)
        {
            HttpClient client = GetHttpClient();
            HttpResponseMessage response = await client.GetAsync($"details/json?place_id={id}&fields=name,formatted_address,photo,place_id,vicinity,website,rating,review&key={APIKey}");
            var detailsjson = await response.Content.ReadAsStringAsync();
            PlaceDetails details = JsonConvert.DeserializeObject<PlaceDetails>(detailsjson);
            return details;
        }
      
    }
}
