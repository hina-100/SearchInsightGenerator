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

            // Constructing the URL for Google Custom Search API request
            string url = $"https://www.googleapis.com/customsearch/v1?key={apiKey}&cx={cseId}&q={query}&num=10";

            // Sending HTTP request and retrieving response
            using (HttpClient client = new HttpClient())
            {
                string searchResultString = "";
                HttpResponseMessage response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    JObject jsonResponse = JObject.Parse(responseBody);

                    // Extracting search results
                    JArray searchResults = (JArray)jsonResponse["items"];
                    var searchResultsList = new List<string>(); 
                    if (searchResults != null)
                    {
                        Console.WriteLine("Top 10 search results:");
                        foreach (JToken result in searchResults)
                        {
                            string title = result["title"].ToString();
                            string link = result["link"].ToString();
                            Console.WriteLine($"{title}: {link}");
                            searchResultsList.Add($"{title}: {link}");
                        }

                        searchResultString = string.Join(';', searchResultsList);
                        await InsertSearchResults(query, searchResultString);
                       // await RunAIModel(query, searchResultString);
                    }
                    else
                    {
                        Console.WriteLine("No search results found.");
                    }

                    return searchResultString;
                }
                else
                {
                    var errorMessage = "Error fetching search results. Status code: " + response.StatusCode;
                    Console.WriteLine(errorMessage);
                    return errorMessage;
                }
            }

        }

        static async Task<string> AnalyzeSentiment(string text, string apiKey)
        {
            using (HttpClient client = new HttpClient())
            {
                // Construct request payload
                var requestData = new
                {
                    text = text
                };

                // Convert request payload to JSON
                var requestBody = Newtonsoft.Json.JsonConvert.SerializeObject(requestData);

                // Set request headers
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
                //client.DefaultRequestHeaders.Add("Content-Type", "application/json");

                // Send POST request to Claude AI sentiment analysis endpoint
                HttpResponseMessage response = await client.PostAsync("https://api.claude.ai/v1/sentiment", new StringContent(requestBody));

                // Check if request was successful
                if (response.IsSuccessStatusCode)
                {
                    // Read response content
                    string responseBody = await response.Content.ReadAsStringAsync();
                    JObject jsonResponse = JObject.Parse(responseBody);
                    return jsonResponse["sentiment"].ToString();
                }
                else
                {
                    Console.WriteLine($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                    return null;
                }
            }
        }

        public static async Task RunAIModel(string query, string queryResponse)
        {
            string apiKey = "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";
            string text = $"Extract and refine relevant information from following search results: {queryResponse}";

            // Send request to Claude AI sentiment analysis endpoint
            string sentiment = await AnalyzeSentiment(text, apiKey);

            // Display sentiment analysis result
            Console.WriteLine($"Sentiment: {sentiment}");
            /* string apiKey = "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";
             string prompt = $"Extract and refine relevant information from following search results: {queryResponse}";
             string model = "text-davinci-003";

             // Construct the request payload
             var requestData = new
             {
                 model = model,
                 prompt = prompt,
                 max_tokens = 50
             };

             // Convert request payload to JSON
             var requestBody = System.Text.Json.JsonSerializer.Serialize(requestData);

             Console.WriteLine("Request Length:" + requestBody.Length);
             // Create HttpClient instance
             using (HttpClient client = new HttpClient())
             {
                 // Set OpenAI API authentication headers
                 client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
                 //client.DefaultRequestHeaders.Add("Content-Type", "application/json");

                 // Make POST request to OpenAI API
                 var response = await client.PostAsync("https://api.openai.com/v1/completions", new StringContent(
                     new string(requestBody.Take(Math.Min(3000, requestBody.Length)).ToArray())));

                 // Check if request was successful
                 if (response.IsSuccessStatusCode)
                 {
                     // Read response content
                     var responseContent = await response.Content.ReadAsStringAsync();
                     Console.WriteLine(responseContent);
                 }
                 else
                 {
                     Console.WriteLine($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                 }
             }*/
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
