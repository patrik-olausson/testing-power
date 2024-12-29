using System;

namespace TestingPower.PowerMocking
{
    public class PowerMockInvalidTypeException : Exception
    {
        public PowerMockInvalidTypeException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}