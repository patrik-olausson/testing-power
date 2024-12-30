// namespace TestingPower.PowerMocking
// {
//     public class AssertableParameter
//     {
//         public string Name { get; }
//         public string ValueAsString { get; }
//         public object? OriginalValue { get; }
//
//         public AssertableParameter(string parameterName, object? originalValueWithSuitableToStringOverride) : this(
//             parameterName,
//             originalValueWithSuitableToStringOverride,
//             originalValueWithSuitableToStringOverride?.ToString() ?? string.Empty)
//         {
//         }
//
//         public AssertableParameter(string parameterName, object? originalValue, string valueAsString)
//         {
//             Name = parameterName;
//             ValueAsString = valueAsString;
//             OriginalValue = originalValue;
//         }
//     }
// }