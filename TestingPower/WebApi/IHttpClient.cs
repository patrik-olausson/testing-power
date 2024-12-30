using System.Net.Http;
using System.Threading.Tasks;

namespace TestingPower.WebApi
{
    public interface IHttpClient
    {
        Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, string? methodCallId = null);   
    }
}