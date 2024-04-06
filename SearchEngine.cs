using Newtonsoft.Json.Linq;

namespace SearchInsightGenerator
{
    public class SearchEngine
    {
        public static async Task GetResults(string query)
        {
            string apiKey = "AIzaSyC6ATf9iKqBTYxtbR3CmgXnm8dBUN-AwVw"; // Replace with your Google API Key
            string cseId = "654327066e1214d7a";   // Replace with your Custom Search Engine ID
            //string query = "What is bitcoin";

            // Constructing the URL for Google Custom Search API request
            string url = $"https://www.googleapis.com/customsearch/v1?key={apiKey}&cx={cseId}&q={query}&num=10";

            // Sending HTTP request and retrieving response
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    JObject jsonResponse = JObject.Parse(responseBody);

                    // Extracting search results
                    JArray searchResults = (JArray)jsonResponse["items"];
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
