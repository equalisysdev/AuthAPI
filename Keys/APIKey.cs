using System.Net.Http.Headers;
using System.Text.Json;

namespace AuthAPI.Keys
{
    public class APIKey : Key
    {
        public string? CustomUserId { get; set; }
        public string? CustomAccountId { get; set; }
        public string? Key { get; set; }
        public Dictionary<string, string>? CustomMetaData { get; set; }
        public DateTime? Expiry { get; set; }
        public long? RateLimit { get; set; }
        public long? RateLimitTtl { get; set; }
        internal static HttpClient httpClient = new();

        public dynamic CreateKey()
        {
            // Creates a key in an AuthAPI project
            using (var request = new HttpRequestMessage(new HttpMethod("POST"), "https://api.theauthapi.com/api-keys/"))
            {
                // Adds the access key to the request headers
                request.Headers.TryAddWithoutValidation("x-api-key", AccessKey);

                // Adds the request body and serializes it to JSON
                request.Content = new StringContent(JsonSerializer.Serialize(new
                {
                    name = Name,
                    projectId = ProjectId,
                    customUserId = CustomUserId,
                    customAccountId = CustomAccountId,
                    key = Key,
                    customMetaData = CustomMetaData,
                    expiry = Expiry,
                    rateLimit = RateLimit,
                    rateLimitTtl = RateLimitTtl
                }));
                request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application / json");

                // Sends the request and returns the response
                var response = httpClient.SendAsync(request);
                if (!response.Result.IsSuccessStatusCode)
                {
                    throw new Exception(response.Result.Content.ReadAsStringAsync().Result);
                }
                return response.Result.Content.ReadAsStringAsync().Result;
            }
        }

        public static async Task<object> GetApiKeysAuth(string accessKey, string apiKey)
        {
            // Crafts the request
            var request = new HttpRequestMessage(new HttpMethod("GET"), "https://api.theauthapi.com/api-keys/auth/" + apiKey);
            request.Headers.TryAddWithoutValidation("x-api-key", accessKey);

            // Sends the request, ensures it was successful then returns it
            HttpResponseMessage response = await httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            return response.Content.ReadAsStringAsync();
        }

        // API Key object constructor
        public APIKey(string accessKey, string name, string projectId, string? customUserId = null, string? customAccountId = null, string? key = null,
            Dictionary<string, string>? customMetaData = null, DateTime? expiry = null, long? rateLimit = null, long? rateLimitTtl = null)
        {
            AccessKey = accessKey;
            Name = name;
            ProjectId = projectId;
            CustomUserId = customUserId;
            CustomAccountId = customAccountId;
            Key = key;
            CustomMetaData = customMetaData;
            Expiry = expiry;
            RateLimit = rateLimit;
            RateLimitTtl = rateLimitTtl;
        }
    }
}