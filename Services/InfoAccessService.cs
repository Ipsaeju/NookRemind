using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

namespace NookRemind.Services
{
    public class InfoAccessService
    {
        private readonly HttpClient _http;

        public InfoAccessService(HttpClient http)
        {
            _http = http;
            _http.DefaultRequestHeaders.Add("secret-key", Environment.GetEnvironmentVariable("jsonkey"));
        }

        /// <summary>
        /// Method GetBugsInfo will retrieve the JSON containing all information of bugs
        /// by making an HTTP GET request to the JSON host
        /// </summary>
        /// <returns>JSON Content if successful or null if unsuccessful</returns>
        public async Task<object> GetBugsInfo()
        {
            var response = await _http.GetAsync(Environment.GetEnvironmentVariable("acnhbugsjson"));
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<object>(responseString);
        }

        /// <summary>
        /// Method GetFishInfo will retrieve the JSON containing all information of fish
        /// by making an HTTP GET request to the JSON host
        /// </summary>
        /// <returns>JSON Content if successful or null if unsuccessful</returns>
        public async Task<object> GetFishInfo()
        {
            var response = await _http.GetAsync(Environment.GetEnvironmentVariable("acnhfishjson"));
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<object>(responseString);
        }
    }
}
