using Microsoft.AspNetCore.SignalR;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace SearchInsightGenerator.Obsolete
{
    public class SearchEngineCode
    {

        /*       public static  void Get()
               {
                   var client = new RestClient("https://google.serper.dev/search");
                   client.Timeout = -1;
                   var request = new RestRequest(Method.Post);
                   request.AddHeader("X-API-KEY", "a3499a412d3b70fe54a2667f6d65a4675ca2bb56");
                   request.AddHeader("Content-Type", "application/json");
                   var body = @"{""q"":""What is bitcoin"",""gl"":""in""}";
                   request.AddParameter("application/json", body, ParameterType.RequestBody);
                   IRestResponse response = client.Execute(request);
                   Console.WriteLine(response.Content);
               }*/
        public static async Task GetSearchResults()
        {
            string apiKey = "a3499a412d3b70fe54a2667f6d65a4675ca2bb56";
            string query = "Your search query";

            // Constructing the URL for the SerpApi request
            string url = $"https://serpapi.com/search.json?engine=google&q={query}&num=10&api_key={apiKey}";

            // Sending HTTP request and retrieving response
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    JObject jsonResponse = JObject.Parse(responseBody);

                    // Extracting search results
                    JArray searchResults = (JArray)jsonResponse["organic_results"];
                    if (searchResults != null)
                    {
                        Console.WriteLine("Top 10 search results:");
                        foreach (JToken result in searchResults)
                        {
                            string title = result["title"].ToString();
                            string link = result["link"].ToString();
                            Console.WriteLine($"{title}: {link}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("No search results found.");
                    }
                }
                else
                {
                    Console.WriteLine("Error fetching search results. Status code: " + response.StatusCode);
                }
            }
        }
    }
}
