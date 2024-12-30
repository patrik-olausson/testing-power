using System.Net.Http;
using System.Threading.Tasks;
using TestingPower.PowerAssertions.CapturedCalls;
using TestingPower.WebApi;

namespace TestingPower.PowerMocking.TestableImplementations
{
    public class TestableHttpClient : IHttpClient
    {
        private readonly PowerMock _powerMock;

        public TestableHttpClient(PowerMock powerMock)
        {
            _powerMock = powerMock;
        }
        
        public async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, string? methodCallId = null)
        {
            methodCallId ??= nameof(SendAsync);
            var capturedRequest = await AssertableHttpRequest.CreateFor(methodCallId, request);
            
            return await _powerMock.HandleCallWithReturnValue<HttpResponseMessage>(capturedRequest);
        }
    }
}