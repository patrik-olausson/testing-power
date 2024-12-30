using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using TestingPower.PowerAssertions.CapturedCalls;

namespace TestingPower.PowerAssertions
{
    public static class ExtensionMethods
    {
        public static JsonDocument? TryCreateJsonDocument(this string value)
        {
            if (!value.LooksLikeItContainsJson()) return null;
            
            try
            {
                return JsonDocument.Parse(value);
            }
            catch
            {
                return null;
            }
        }
        
        public static async Task<string> TryGetContentAsString(this HttpContent? content)
        {
            if (content == null) return "No content (null)";
            
            var contentAsString = await content.ReadAsStringAsync();
            if (string.IsNullOrWhiteSpace(contentAsString))
            {
                return "No content (empty)";
            }

            return contentAsString;
        }
        
        public static bool LooksLikeItContainsJson(this string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return false;
            if (value.StartsWith('{')) return true;
            if (value.StartsWith('[')) return true;

            return false;
        }

        public static IReadOnlyCollection<AssertableHeader> ToAssertableHeaders(this HttpHeaders? headers)
        {
            if (headers == null) return Array.Empty<AssertableHeader>();

            return headers.Select(header => new AssertableHeader(header)).ToList();
        }
    }
}