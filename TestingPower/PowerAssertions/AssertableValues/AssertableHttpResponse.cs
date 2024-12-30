using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TestingPower.PowerMocking;

namespace TestingPower.PowerAssertions.CapturedCalls
{
    public class AssertableHttpResponse : ICapturedCallInfo
    {
        private readonly string _rawContentString;
        private readonly JsonDocument? _contentAsJsonDocument;
        
        public string Id { get; }
        public HttpStatusCode StatusCode { get; }
        public IReadOnlyCollection<AssertableHeader> Headers { get; }
        internal bool IsSuccessStatusCode { get; }

        private AssertableHttpResponse(
            string id,
            HttpStatusCode statusCode,
            bool isSuccessStatusCode,
            string rawContentString,
            IReadOnlyCollection<AssertableHeader> headers)
        {
            _rawContentString = rawContentString;
            _contentAsJsonDocument = rawContentString.TryCreateJsonDocument();
            Id = id;
            IsSuccessStatusCode = isSuccessStatusCode;
            StatusCode = statusCode;
            Headers = headers;
        }

        public static async Task<AssertableHttpResponse> CreateFor(string id, HttpResponseMessage responseMessage)
        {
            if (responseMessage == null) throw new ArgumentNullException(nameof(responseMessage));

            return new AssertableHttpResponse(
                id,
                responseMessage.StatusCode,
                responseMessage.IsSuccessStatusCode,
                await responseMessage.Content.TryGetContentAsString(),
                responseMessage.Headers.ToAssertableHeaders());
        }

        public string ToAssertableText(AssertableTextDetailLevel detailLevel)
        {
            if (detailLevel.IsNone())
                return string.Empty;

            var sb = new StringBuilder();
            sb.AppendLine($"Captured HttpResponse {Id}");
            sb.AppendLine($"Status code: {StatusCode}");
            
            if (detailLevel.IsVerbose())
            {
                if (Headers.Any())
                {
                    sb.AppendLine(
                        $"Headers: [{string.Join(", ", Headers.Select(header => $"{header.Name}:{header.Value}"))}]");
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
            
            return sb.ToString();
        }

        public T TryGetContentAs<T>()
        {
            return JsonSerializerForTests.FromJsonString<T>(_rawContentString);
        }
    }
}