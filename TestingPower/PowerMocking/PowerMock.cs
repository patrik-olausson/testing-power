using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TestingPower.PowerMocking
{
    /// <summary>
    /// Simple but powerful class that makes it easier to handle test data when writing tests.
    /// You start your test by preparing the calls you expect to be made and when running the test
    /// each call will be captured and the return value you have prepared will be returned.
    /// </summary>
    public class PowerMock 
    {
        private readonly ValueListDictionary<string, ICapturedCallInfo> _capturedCalls = new ValueListDictionary<string, ICapturedCallInfo>();
        private readonly ValueQueueDictionary<string, ReturnValueContainer> _returnValues = new ValueQueueDictionary<string, ReturnValueContainer>();
        
        public void PrepareForCallWithReturnValue(
            string uniqueCallId,
            object? valueToReturn,
            int numberOfCalls = 1)
        {
            AddReturnValue(
                uniqueCallId,
                new ReturnValueContainer(valueToReturn),
                numberOfCalls);
        }

        public void PrepareForCallWithoutReturnValue(
            string uniqueCallId,
            int numberOfCalls = 1)
        {
            AddReturnValue(
                uniqueCallId,
                new ReturnValueContainer(null),
                numberOfCalls);
        }

        public void PrepareForCallThatShouldThrowException(
            string uniqueCallId,
            Exception exception,
            int numberOfCalls = 1)
        {
            if (exception == null) throw new ArgumentNullException(nameof(exception));
            
            AddReturnValue(
                uniqueCallId,
                new ReturnValueContainer(exception),
                numberOfCalls);
        }

        public Task HandleCallWithoutReturnValue(ICapturedCallInfo capturedCallInfo)
        {
            RegisterCallAndGetReturnValue(capturedCallInfo);

            return Task.CompletedTask;
        }
        
        public Task<TReturnValue> HandleCallWithReturnValue<TReturnValue>(ICapturedCallInfo capturedCallInfo)
        {
            var valueToReturn = RegisterCallAndGetReturnValue(capturedCallInfo);

            try
            {
                return Task.FromResult((TReturnValue)valueToReturn!);
            }
            catch (Exception e)
            {
                throw new PowerMockInvalidTypeException("Unable to cast return value to the specified type, make sure you have prepared the call correctly", e);
            }
        }

        private object? RegisterCallAndGetReturnValue(ICapturedCallInfo capturedCallInfo)
        {
            _capturedCalls.AddValue(capturedCallInfo.Id, capturedCallInfo);
            var valueContainer = _returnValues.GetValue(capturedCallInfo.Id);
            if (valueContainer == null)
            {
                throw new PowerMockCallNotPreparedException($"Unexpected call to uniqueCallId: {capturedCallInfo.Id}. Make sure to prepare call during test setup!");
            }

            if (valueContainer.ContainsException)
            {
                throw valueContainer.GetException();
            }

            return valueContainer.ValueToReturn;
        }
        
        public IReadOnlyCollection<ICapturedCallInfo> GetCapturedCalls(string uniqueCallId)
        {
            return _capturedCalls.GetValues(uniqueCallId);
        }

        public IReadOnlyCollection<ICapturedCallInfo> GetAllCapturedCalls()
        {
            return _capturedCalls.GetAllValues();
        }

        private void AddReturnValue(
            string uniqueCallId,
            ReturnValueContainer returnValueContainer,
            int numberOfCalls)
        {
            if (numberOfCalls < 1)
            {
                throw new ArgumentOutOfRangeException("Number of calls must be greater than 0", nameof(numberOfCalls));
            }
            
            for (int i = 0; i <numberOfCalls; i++)
            {
                _returnValues.AddValue(uniqueCallId, returnValueContainer);    
            }
        }
    }

    public class FuncTextManipulator : ITextManipulator
    {
        public string CallId { get; }
        private readonly Func<string, string> _textManipulator;


        public FuncTextManipulator(string callId, Func<string, string> textManipulator)
        {
            if (string.IsNullOrWhiteSpace(callId))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(callId));
            
            CallId = callId;
            _textManipulator = textManipulator ?? throw new ArgumentNullException(nameof(textManipulator));
        }

        public string ManipulateText(string originalText)
        {
            return _textManipulator.Invoke(originalText);
        }
    }
}