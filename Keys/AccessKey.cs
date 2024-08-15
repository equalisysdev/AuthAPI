using System.Net.Http.Headers;
using System.Text.Json;

namespace AuthAPI.Keys
{
    public class AccessKey : Key
    {
        public string UsedAccessKey { get; set; }
        public string? Name { get; set; }
        public string? ProjectId { get; set; }
        public string? AccountId { get; set; }
        public int? rateLimit { get; set; }
        public int? rateLimitTtl { get; set; }
        internal static HttpClient httpClient = new();

        public object CreateKey()
        {
            using (var request = new HttpRequestMessage(new HttpMethod("POST"), "https://api.theauthapi.com/access-keys"))
            {
                request.Headers.TryAddWithoutValidation("x-api-key", UsedAccessKey);

                request.Content = new StringContent(JsonSerializer.Serialize(new
                {
                    name = Name,
                    ProjectId = ProjectId,
                    accountId = AccountId,
                    rateLimit = rateLimit,
                    rateLimitTtl = rateLimitTtl
                }));
                request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

                var response = httpClient.SendAsync(request);
                if (!response.Result.IsSuccessStatusCode)
                {
                    throw new Exception(response.Result.Content.ReadAsStringAsync().Result);
                }
                return response.Result.Content.ReadAsStringAsync().Result;
            }
            {
            }
        }
    }
}