using Microsoft.Extensions.Configuration;
using Snackis.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;

namespace Snackis.Gateway
{
    public class BadWordGateway : IBadWordGateway
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public BadWordGateway(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<BadWord> PostBadWordAsync(BadWord badWord)
        {
            var response = await _httpClient.PostAsJsonAsync(_configuration["BadWordAPIConnection"], badWord);
            return badWord;
        }

        public async Task<List<BadWord>> GetAllBadWordsAsync()
        {
            var response = await _httpClient.GetAsync(_configuration["BadWordAPIConnection"]);
            var apiResponse = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<BadWord>>(apiResponse);
        }

        public async Task DeleteBadWordAsync(int id)
        {
            var response = await _httpClient.DeleteAsync(_configuration["BadWordAPIConnection"] + "/" + id);
        }

        //Test
        public async Task<string> CheckForBadWords(string message)
        {
            message = HttpUtility.UrlEncode(message);

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(_configuration["CensorMessageAPI"]),
                Headers =
                {
                    {"text", message }
                },
            };

            var response = await _httpClient.SendAsync(request);
            string censoredMessage = await response.Content.ReadAsStringAsync();

            return censoredMessage;
        }

        //TEST
    }
}
