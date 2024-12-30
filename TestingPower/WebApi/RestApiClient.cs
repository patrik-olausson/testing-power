using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using TestingPower.PowerAssertions.CapturedCalls;
using TestingPower.PowerMocking;

namespace TestingPower.WebApi
{
    public class RestApiClient
    {
        private readonly IHttpClient _httpClient;
        private readonly Action<string> _testOutputLogger;

        public RestApiClient(IHttpClient httpClient, Action<string>? testOutputLogger)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _testOutputLogger = testOutputLogger ?? Console.WriteLine;
        }
        
        public Task<AssertableHttpResponse> GetAsync(
            string url,
            bool ensureSuccessStatusCode = true,
            Action<HttpRequestMessage>? manipulateRequest = null)
        {
            return SendAsync(
                nameof(GetAsync),
                CreateRequestMessage(HttpMethod.Get, url), 
                ensureSuccessStatusCode,
                manipulateRequest);
        }
        
        public Task<AssertableHttpResponse> PostJsonAsync<T>(
            string url, 
            T content,
            bool ensureSuccessStatusCode = true,
            Action<HttpRequestMessage>? manipulateRequest = null)
        {
            return SendAsync(
                nameof(PostJsonAsync),
                CreateRequestMessage(HttpMethod.Post, url,CreateJsonContent(JsonSerializer.Serialize(content))),
                ensureSuccessStatusCode,
                manipulateRequest);
        }
        
        public Task<AssertableHttpResponse> PutJsonAsync<T>(
            string url, 
            T content,
            bool ensureSuccessStatusCode = true,
            Action<HttpRequestMessage>? manipulateRequest = null)
        {
            return SendAsync(
                nameof(PutJsonAsync),
                CreateRequestMessage(HttpMethod.Put, url,CreateJsonContent(JsonSerializer.Serialize(content))),
                ensureSuccessStatusCode,
                manipulateRequest);
        }
        
        public Task<AssertableHttpResponse> DeleteAsync(
            string url, 
            bool ensureSuccessStatusCode = true,
            Action<HttpRequestMessage>? manipulateRequest = null)
        {
            return SendAsync(
                nameof(DeleteAsync),
                CreateRequestMessage(HttpMethod.Delete, url),
                ensureSuccessStatusCode,
                manipulateRequest);
        }
        
        public Task<AssertableHttpResponse> SendAsync(
            HttpRequestMessage request,
            bool ensureSuccessStatusCode = true)
        {
            return SendAsync(
                nameof(SendAsync),
                request,
                ensureSuccessStatusCode);
        }

        private static HttpRequestMessage CreateRequestMessage(
            HttpMethod httpMethod,
            string url,
            HttpContent? content = null)
        {
            var requestMessage = new HttpRequestMessage(httpMethod, url);

            if (content != null)
            {
                requestMessage.Content = content;
            }

            return requestMessage;
        }

        private static HttpContent CreateJsonContent(string content)
        {
            return new StringContent(content, System.Text.Encoding.UTF8, "application/json");
        }
        
        private async Task<AssertableHttpResponse> SendAsync(
            string callId,
            HttpRequestMessage request,
            bool ensureSuccessStatusCode,
            Action<HttpRequestMessage>? manipulateRequest = null)
        {
            manipulateRequest?.Invoke(request);
            await LogRequest();
            
            var response = await _httpClient.SendAsync(request);
            var assertableResponse = await AssertableHttpResponse.CreateFor(callId, response);
            
            LogResponse();
            
            if (ensureSuccessStatusCode && !assertableResponse.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Response status code does not indicate success: {response.StatusCode}");
            }

            return assertableResponse;

            async Task LogRequest()
            {
                var assertableRequest = await AssertableHttpRequest.CreateFor(callId, request);
                _testOutputLogger.Invoke("Request");
                _testOutputLogger.Invoke(assertableRequest.ToAssertableText(AssertableTextDetailLevel.Verbose));
                _testOutputLogger.Invoke(string.Empty);
            }

            void LogResponse()
            {
                _testOutputLogger.Invoke("Response");
                _testOutputLogger.Invoke(assertableResponse.ToAssertableText(AssertableTextDetailLevel.Verbose));
            }
        }
    }
}