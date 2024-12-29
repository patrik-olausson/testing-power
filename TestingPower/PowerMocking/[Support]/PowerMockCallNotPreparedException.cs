using System;

namespace TestingPower.PowerMocking
{
    public class PowerMockCallNotPreparedException : Exception
    {
        public PowerMockCallNotPreparedException(string message) : base(message)
        {
        }
    }
}