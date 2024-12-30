using System.Collections.Generic;
using System.Text;
using TestingPower.PowerMocking;

namespace TestingPower.PowerAssertions.AssertableText
{
    public static class AssertableTextBuilder
    {
        public static string CreateAssertableText(
            this PowerMock powerMock,
            object? result = null,
            AssertableTextOptions? options = null)
        {
            return CreateAssertableText(
                result,
                powerMock.GetAllCapturedCalls(),
                options);
        }
        
        public static string CreateAssertableText(
            object? result,
            IReadOnlyCollection<ICapturedCallInfo> capturedCalls,
            AssertableTextOptions? options = null)
        {
            options ??= new AssertableTextOptions();
            var sb = new StringBuilder();
            if (result != null)
            {
                sb.AppendLine("Result:");
                var jsonString = JsonSerializerForTests.ToJsonString(result);
                if (options.ResultJsonStringManipulator != null)
                {
                    jsonString = options.ResultJsonStringManipulator.Invoke(jsonString);
                }
                sb.AppendLine(jsonString);
                sb.AppendLine();
            }

            foreach (var capturedCall in capturedCalls)
            {
                sb.AppendLine(GetAssertableText(capturedCall));
            }
            
            return sb.ToString();

            string GetAssertableText(ICapturedCallInfo capturedCallInfo)
            {
                var assertableText = capturedCallInfo.ToAssertableText(options.DetailLevel);
                
                var textManipulator = options.TryGetTextManipulator(capturedCallInfo.Id);
                if (textManipulator == null)
                {
                    return assertableText;
                }
                
                return textManipulator.ManipulateText(assertableText);
            }
        }
    }
}