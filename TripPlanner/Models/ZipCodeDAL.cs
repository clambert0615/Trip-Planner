using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace TripPlanner.Models
{
    public class ZipCodeDAL
    {
        private readonly string APIKey;
        public ZipCodeDAL(IConfiguration configuration)
        {
            APIKey = configuration.GetSection("ApiKeys")["ZipAPI"];
        }

        public HttpClient GetHttpClient()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://www.zipcodeapi.com/rest/");
            return client;
        }
        
        public async Task<ZipCode> GetZip(string city, string state)
        {
            HttpClient client = GetHttpClient();
            HttpResponseMessage response = await client.GetAsync($"{APIKey}/city-zips.json/{city}/{state}");
            //install-package Microsoft.AspNet.WebAPI.Client
            var zipjson = await response.Content.ReadAsStringAsync();
            ZipCode zip = JsonConvert.DeserializeObject<ZipCode>(zipjson);
            return zip;
        }
        public async Task<CityState> GetCity(string zip)
        {
            HttpClient client = GetHttpClient();
            HttpResponseMessage response = await client.GetAsync($"{APIKey}/info.json/{zip}/degrees");
            var cityjson = await response.Content.ReadAsStringAsync();
            CityState city = JsonConvert.DeserializeObject<CityState>(cityjson);
            return city;
        }

        
    }
}
