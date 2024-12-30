using System.Text.Json;

namespace TestingPower
{
    public class JsonSerializerForTests
    {
        private static readonly JsonSerializerOptions DefaultSerializerOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            WriteIndented = true
        };

        public static T FromJsonString<T>(string stringWithJsonContent) =>
            JsonSerializer.Deserialize<T>(stringWithJsonContent, DefaultSerializerOptions)!;

        public static string ToJsonString<T>(T valueToSerialize) =>
            JsonSerializer.Serialize(valueToSerialize, DefaultSerializerOptions);
    }
}
