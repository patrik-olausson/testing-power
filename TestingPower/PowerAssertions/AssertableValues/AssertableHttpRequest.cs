using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TestingPower.PowerMocking;

namespace TestingPower.PowerAssertions.CapturedCalls
{
    public class AssertableHttpRequest : ICapturedCallInfo
    {
        private readonly string _rawContentString;
        private readonly JsonDocument? _contentAsJsonDocument;
        
        public string Id { get; }
        public HttpMethod Method { get; }
        public string RequestUrl { get; }
        public IReadOnlyCollection<AssertableHeader> Headers { get; }

        private AssertableHttpRequest(
            string id,
            HttpMethod method,
            string requestUrl,
            string rawContentString,
            IReadOnlyCollection<AssertableHeader> headers)
        {
            _rawContentString = rawContentString;
            _contentAsJsonDocument = rawContentString.TryCreateJsonDocument();
            Id = id;
            Method = method;
            RequestUrl = requestUrl;
            Headers = headers;
        }

        public static async Task<AssertableHttpRequest> CreateFor(string id, HttpRequestMessage requestMessage)
        {
            if (requestMessage == null) throw new ArgumentNullException(nameof(requestMessage));

            return new AssertableHttpRequest(
                id,
                requestMessage.Method,
                requestMessage.RequestUri.ToString(),
                await requestMessage.Content.TryGetContentAsString(),
                requestMessage.Headers.ToAssertableHeaders()
            );
        }

        public string ToAssertableText(AssertableTextDetailLevel detailLevel)
        {
            if (detailLevel.IsNone())
                return string.Empty;

            var sb = new StringBuilder();
            sb.AppendLine($"Captured HttpRequest {Id}");
            sb.AppendLine($"{Method.Method} {RequestUrl}");

            if (detailLevel.IsVerbose())
            {
                if (Headers.Any())
                {
                    var headerString = string.Join(", ", Headers.Select(header => $"{header.Name}:{header.Value}"));
                    sb.AppendLine($"Headers: [{headerString}]");
                }

                if (_contentAsJsonDocument != null)
                {
                    sb.AppendLine(JsonSerializerForTests.ToJsonString(_contentAsJsonDocument));
                }
                else
                {
                    sb.AppendLine(_rawContentString);
                }
            }

            sb.AppendLine();
            
            return sb.ToString();
        }
    }
}