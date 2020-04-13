using PortaleMusei.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Net.Http.Formatting;
using Microsoft.Extensions.Configuration;

namespace PortaleMusei
{
    public static class MuseiNewsApi
    {
        private static HttpClient client = new HttpClient();

        /* Edit the following lines with your MuseiNews API configuration.
         * https://github.com/cristianlivella/MuseiNews */
        private static string host = "https://musei.cristianlivella.com:2053";
        private static int clientId = 2;
        private static string clientToken = "pu24trkh61y7efr3zinj2q9okop5k24au0lirddv6rwnrqrh2ptke4q8uoufbp6d";

        static MuseiNewsApi()
        {
            client.BaseAddress = new Uri(host);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", clientToken);
        }

        public static async Task<IEnumerable<News>> GetLastNews(int number = 3)
        {
            IEnumerable<News> news = null;
            HttpResponseMessage response = await client.GetAsync("/news");
            if (response.IsSuccessStatusCode)
            {
                news = await response.Content.ReadAsAsync<IEnumerable<News>>();
            }
            return news.OrderByDescending(n => n.Timestamp).Take(number);
        }

        public static async Task<IEnumerable<News>> GetUnreadNews(string userId, int number = 3)
        {
            IEnumerable<News> news = null;
            HttpResponseMessage response = await client.GetAsync("/clients/" + clientId + "/users/" + userId + "/news/unread");
            if (response.IsSuccessStatusCode)
            {
                news = await response.Content.ReadAsAsync<IEnumerable<News>>();
            }
            news = news.OrderByDescending(n => n.Timestamp).Take(number);
            foreach (News thisNews in news) {
                thisNews.Read = true;
                await client.PutAsJsonAsync("/clients/" + clientId + "/users/" + userId + "/news/" + thisNews.Id, thisNews);
            }
            return news;
        }
    }
}
