using System;
using System.Collections.Generic;
using System.Linq;

namespace TestingPower.PowerMocking
{
    public class AssertableTextOptions
    {
        private readonly Dictionary<string, ITextManipulator> _textManipulators;

        public AssertableTextOptions(
            AssertableTextDetailLevel detailLevel = AssertableTextDetailLevel.Verbose,
            Func<string, string>? resultJsonStringManipulator = null,
            params ITextManipulator[] textManipulators)
        {
            _textManipulators = textManipulators.ToDictionary(m => m.CallId);
            DetailLevel = detailLevel;
            ResultJsonStringManipulator = resultJsonStringManipulator;
        }

        public AssertableTextDetailLevel DetailLevel { get; }
        
        public Func<string, string>? ResultJsonStringManipulator { get; }

        public ITextManipulator? TryGetTextManipulator(string callId)
        {
            return _textManipulators.GetValueOrDefault(callId);
        }
    }
}