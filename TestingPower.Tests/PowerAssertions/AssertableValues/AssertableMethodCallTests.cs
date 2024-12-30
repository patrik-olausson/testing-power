using FluentAssertions;
using TestingPower.PowerAssertions.CapturedCalls;
using TestingPower.PowerMocking;
using Xunit.Abstractions;

namespace TestingPower.Tests.PowerAssertions.CapturedCalls.AssertableMethodCallTests;

public class ToAssertableText
{
    public class MethodCallWithParameters(ITestOutputHelper testOutputHelper)
        : AssertableMethodCallTestHarness(testOutputHelper)
    {
        [Fact]
        public async Task
            GivenCapturedCallInfoWithVerboseDetails_ThenItReturnsTextThatContainsTheMethodIdAndParameterInformation()
        {
            var sut = CreateSut();

            var result = sut.ToAssertableText(AssertableTextDetailLevel.Verbose);

            LogTestOutput(result);
            await Verify(result);
        }

        [Fact]
        public void GivenCapturedCallInfoWithMinimalDetails_ThenItReturnsStringThatContainsTheMethodId()
        {
            var sut = CreateSut();

            var result = sut.ToAssertableText(AssertableTextDetailLevel.Minimal);

            LogTestOutput(result);
            result.Should().StartWith("Captured call to Class.TheCalledMethod with parameters");
        }

        [Fact]
        public void GivenCapturedCallInfoWithNoDetails_ThenItReturnsEmptyString()
        {
            var sut = CreateSut();

            var result = sut.ToAssertableText(AssertableTextDetailLevel.None);

            result.Should().BeEmpty();
        }

        private AssertableMethodCall CreateSut() => CreateSut(
            parameterObject: new
            {
                param1 = "value 1",
                param2 = 42,
                param3 = true,
                param4 = new
                {
                    prop1 = 3.14m,
                    prop2 = new[] { 1, 2, 3 }
                }
            });
    }

    public class MethodCallWithoutParameters(ITestOutputHelper testOutputHelper)
        : AssertableMethodCallTestHarness(testOutputHelper)
    {
        [Fact]
        public void GivenCapturedCallInfoWithVerboseDetails_ThenItReturnsStringThatContainsTheMethodId()
        {
            var sut = CreateSut();

            var result = sut.ToAssertableText(AssertableTextDetailLevel.Verbose);

            result.Should().StartWith("Captured call to Class.TheCalledMethod without parameters");
        }

        [Fact]
        public void GivenCapturedCallInfoWithMinimalDetails_ThenItReturnsStringThatContainsTheMethodId()
        {
            var sut = CreateSut();

            var result = sut.ToAssertableText(AssertableTextDetailLevel.Minimal);

            result.Should().StartWith("Captured call to Class.TheCalledMethod without parameters");
        }

        [Fact]
        public void GivenCapturedCallInfoWithNoDetails_ThenItReturnsEmptyString()
        {
            var sut = CreateSut();

            var result = sut.ToAssertableText(AssertableTextDetailLevel.None);

            result.Should().BeEmpty();
        }
    }
}

public class GetParameterList(ITestOutputHelper testOutputHelper) : AssertableMethodCallTestHarness(testOutputHelper)
{
    [Fact]
    public void GivenMethodCallWithNoParameters_ThenItReturnsEmptyCollection()
    {
        var sut = CreateSut();

        var result = sut.GetParameterList();
        
        result.Should().BeEmpty();
    }
    
    [Fact]
    public void GivenMethodCallWithParameters_ThenItReturnsCollectionWithEntryForEachParameterObjectProperty()
    {
        var param2ComplexTypeValue = new { prop1 = 42 };
        var parameterObject = new
        {
            param1 = "value 1",
            param2 = param2ComplexTypeValue,
            param3 = true
        };
        var sut = CreateSut(parameterObject: parameterObject);

        var result = sut.GetParameterList();
        
        result.Should().HaveCount(3).And.ContainInOrder(("param1", "value 1"), ("param2", param2ComplexTypeValue), ("param3", true));
    }
}

public class Parameters(ITestOutputHelper testOutputHelper) : AssertableMethodCallTestHarness(testOutputHelper)
{
    [Fact]
    public void GivenMethodCallWithNoParameters_ThenItReturnsNull()
    {
        var sut = CreateSut();

        var result = sut.Parameters;
        
        result.Should().BeNull();
    }
    
    [Fact]
    public void GivenMethodCallWithParameters_ThenItReturnsTheParameterObject()
    {
        var parameterObject = new { param1 = "value 1" };
        var sut = CreateSut(parameterObject: parameterObject);

        var result = sut.Parameters;
        
        result.Should().Be(parameterObject);
    }
}

public class AssertableMethodCallTestHarness(ITestOutputHelper testOutputHelper) : TestHarness(testOutputHelper)
{
    protected AssertableMethodCall CreateSut(string id = "Class.TheCalledMethod", object? parameterObject = null)
    {
        if (parameterObject is null) return AssertableMethodCall.ForMethodWithoutParameters(id);

        return AssertableMethodCall.ForMethodWithParameters(id, parameterObject);
    }
}