namespace TestingPower.PowerMocking
{
    public class AssertableParameter
    {
        public string ValueAsString { get; }
        public object? OriginalValue { get; }

        public AssertableParameter(object? originalValue) : this(
            originalValue,
            originalValue?.ToString() ?? string.Empty)
        {
        }

        public AssertableParameter(object? originalValue, string valueAsString)
        {
            ValueAsString = valueAsString;
            OriginalValue = originalValue;
        }
    }
}