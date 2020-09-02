using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace TripPlanner.Models
{
    public class CovidDAL
    {
        public HttpClient GetHttpClient()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://localcoviddata.com/covid19/v1/");
            return client;
        }

        public async Task<Covid> GetCovid(string zip)
        {
            HttpClient client = GetHttpClient();
            HttpResponseMessage response = await client.GetAsync($"cases/newYorkTimes?zipCode={zip}&daysInPast=4");
            //install-package Microsoft.AspNet.WebAPI.Client
            Covid covid = await response.Content.ReadAsAsync<Covid>();

            return covid;
        }





    }
}
