using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json.Linq;

namespace SearchInsightGenerator
{
    public class SearchEngine
    {
        public static async Task<string> GetResults(string query)
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

                        await InsertSearchResults(query, responseBody);
                    }
                    else
                    {
                        Console.WriteLine("No search results found.");
                    }

                    return responseBody;
                }
                else
                {
                    var errorMessage = "Error fetching search results. Status code: " + response.StatusCode;
                    Console.WriteLine(errorMessage);
                    return errorMessage;
                }
            }

        }

        public static async Task InsertSearchResults(string searchQuery, string searchResult)
        {
            //string connectionString = "mongodb://localhost:27017";
            var connectionString = "mongodb+srv://hinaagrawal100:p6X6OypCUMIhi1dA@cluster0.0mped2d.mongodb.net/?retryWrites=true&w=majority&appName=Cluster0";

            // Connect to MongoDB
            var client = new MongoClient(connectionString);

            // Access database
            var database = client.GetDatabase("Search");

            // Access collection
            var collection = database.GetCollection<BsonDocument>("SearchHistory");

            // Create document to insert
            var document = new BsonDocument
        {
            { "SearchQuery", searchQuery },
            { "SearchResult", searchResult }
        };

            // Insert document into collection
            await collection.InsertOneAsync(document);
        }
    }
}
