using System;
using System.Collections.Generic;

namespace TestingPower.PowerMocking
{
    public class CapturedCall
    {
        private IReadOnlyCollection<AssertableParameter> _parameters;

        public string Id { get; }

        public CapturedCall(string id, IReadOnlyCollection<AssertableParameter>? parameters = null)
        {
            Id = id;
            _parameters = parameters ?? Array.Empty<AssertableParameter>();
        }
    }
}