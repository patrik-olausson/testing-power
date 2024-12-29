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
        private readonly ValueListDictionary<string, CapturedCall> _capturedCalls = new ValueListDictionary<string, CapturedCall>();
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

        public Task HandleCallWithoutReturnValue(string uniqueCallId, params AssertableParameter[] parameters)
        {
            RegisterCallAndGetReturnValue(uniqueCallId, parameters);

            return Task.CompletedTask;
        }

        public Task<TReturnValue> HandleCallWithReturnValue<TReturnValue>(string uniqueCallId, params AssertableParameter[] parameters)
        {
            var valueToReturn = RegisterCallAndGetReturnValue(uniqueCallId, parameters);

            try
            {
                return Task.FromResult((TReturnValue)valueToReturn!);
            }
            catch (Exception e)
            {
                throw new PowerMockInvalidTypeException("Unable to cast return value to the specified type, make sure you have prepared the call correctly", e);
            }
        }

        private object? RegisterCallAndGetReturnValue(string uniqueCallId, IReadOnlyCollection<AssertableParameter> parameters)
        {
            _capturedCalls.AddValue(uniqueCallId, new CapturedCall(uniqueCallId, parameters));
            var valueContainer = _returnValues.GetValue(uniqueCallId);
            if (valueContainer == null)
            {
                throw new PowerMockCallNotPreparedException($"Unexpected call to uniqueCallId: {uniqueCallId}. Make sure to prepare call during test setup!");
            }

            if (valueContainer.ContainsException)
            {
                throw valueContainer.GetException();
            }

            return valueContainer.ValueToReturn;
        }
        
        public IReadOnlyCollection<CapturedCall> GetCapturedCalls(string uniqueCallId)
        {
            return _capturedCalls.GetValues(uniqueCallId);
        }

        public IReadOnlyCollection<CapturedCall> GetAllCapturedCalls()
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
}