using System.Collections.Generic;
using System.Linq;

namespace TestingPower.PowerAssertions.CapturedCalls
{
    public class AssertableHeader
    {
        public string Name { get; }
        public string Value { get; }

        public AssertableHeader(string name, string value)
        {
            Name = name;
            Value = value;
        }

        public AssertableHeader(KeyValuePair<string,IEnumerable<string>> header)
        {
            Name = header.Key;
            Value = header.Value.FirstOrDefault() ?? string.Empty;
        }
    }
}