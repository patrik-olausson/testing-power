using System;

namespace TestingPower.PowerMocking
{
    public class ReturnValueContainer
    {
        public object? ValueToReturn { get; }
        public bool ContainsException => ValueToReturn is Exception;

        public ReturnValueContainer(object? valueToReturn)
        {
            ValueToReturn = valueToReturn;
        }
        
        public Exception GetException() => (Exception) ValueToReturn!;
    }
}