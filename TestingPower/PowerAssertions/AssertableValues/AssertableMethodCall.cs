using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestingPower.PowerMocking;

namespace TestingPower.PowerAssertions.CapturedCalls
{
    public class AssertableMethodCall : ICapturedCallInfo
    {
        public string Id { get; }
        public object? Parameters { get; }

        public IReadOnlyCollection<ValueTuple<string, object>> GetParameterList()
        {
            if (Parameters == null)
                return Array.Empty<ValueTuple<string, object>>();
            
            return Parameters.GetType().GetProperties().Select(
                        propertyInfo => new ValueTuple<string, object>(
                            propertyInfo.Name,
                            propertyInfo.GetValue(Parameters)))
                    .ToList();
        }

        private AssertableMethodCall(string id, object? parameterObject)
        {
            Id = id;
            Parameters = parameterObject;
        }
        
        public static AssertableMethodCall ForMethodWithParameters(string id, object parameterObject)
        {
            if (parameterObject == null) throw new ArgumentNullException(nameof(parameterObject));
            
            return new AssertableMethodCall(id, parameterObject);
        }
        
        public static AssertableMethodCall ForMethodWithoutParameters(string id)
        {
            return new AssertableMethodCall(id, null);
        }

        public string ToAssertableText(AssertableTextDetailLevel detailLevel)
        {
            if (detailLevel.IsNone())
                return string.Empty;

            var sb = new StringBuilder();
            sb.Append($"Captured call to {Id}");
            
            if (Parameters != null)
            {
                sb.AppendLine(" with parameters");
                if (detailLevel.IsVerbose())
                {
                    sb.AppendLine(JsonSerializerForTests.ToJsonString(Parameters));
                }
            }
            else
            {
                sb.Append(" without parameters");
            }

            sb.AppendLine();

            return sb.ToString();
        }
    }
}