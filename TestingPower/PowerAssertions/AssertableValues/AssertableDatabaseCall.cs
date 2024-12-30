using System.Text;
using TestingPower.PowerMocking;

namespace TestingPower.PowerAssertions.CapturedCalls
{
    public class AssertableDatabaseCall : ICapturedCallInfo
    {
        public string Id { get; }
        public string Sql { get; }
        public object? ParameterObject { get; }

        public AssertableDatabaseCall(string id, string sql, object? parameterObject = null)
        {
            Id = id;
            Sql = sql;
            ParameterObject = parameterObject;
        }
        
        public string ToAssertableText(AssertableTextDetailLevel detailLevel)
        {
            if (detailLevel.IsNone()) return string.Empty;

            var sb = new StringBuilder();
            sb.AppendLine($"Captured call to {Id}");
            if (detailLevel.IsVerbose())
            {
                sb.AppendLine(Sql);
            }

            sb.AppendLine();
            
            return sb.ToString();
        }
    }
}