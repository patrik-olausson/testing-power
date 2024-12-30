using Xunit.Abstractions;

namespace TestingPower.Tests;

public class TestHarness(ITestOutputHelper testOutputHelper)
{
    protected void LogTestOutput(string message) => testOutputHelper.WriteLine(message);
    
    protected async Task<TException> AssertThrowsAnyAsync<TException>(Func<Task> testCode)
        where TException : Exception
    {
        var exception = await Assert.ThrowsAnyAsync<TException>(testCode);
        LogTestOutput($"Exception message: {exception.Message }");

        return exception;
    } 
    
    protected TException AssertThrowsAny<TException>(Action testCode)
        where TException : Exception
    {
        var exception = Assert.ThrowsAny<TException>(testCode);
        LogTestOutput($"Exception message: {exception.Message }");

        return exception;
    } 
}