using TestingPower.PowerAssertions.AssertableText;
using TestingPower.PowerAssertions.CapturedCalls;
using TestingPower.PowerMocking;
using Xunit.Abstractions;

namespace TestingPower.Tests.PowerAssertions.AssertableText.AssertableTextBuilderTests;

public class CreateAssertableText(ITestOutputHelper testOutputHelper)
    : AssertableTextBuilderTestHarness(testOutputHelper)
{
    [Fact]
    public async Task GivenThatResultIsNullAndThereAreTwoCapturedCallsAndUsingDefaultOptions_ThenItCreatesVerboseTextForTheTwoCapturedCalls()
    {
        IReadOnlyCollection<ICapturedCallInfo> capturedCalls =
        [
            AssertableMethodCall.ForMethodWithoutParameters("Method1"),
            AssertableMethodCall.ForMethodWithoutParameters("Method2")
        ];
        
        var result = AssertableTextBuilder.CreateAssertableText(
            result: null,
            capturedCalls: capturedCalls,
            options: null);

        await Verify(result);
    }
    
    [Fact]
    public async Task GivenResultAndThereAreOneCapturedCallAndUsingDefaultOptions_ThenItIncludesJsonForTheResultAndCreatesVerboseTextForTheCapturedCall()
    {
        IReadOnlyCollection<ICapturedCallInfo> capturedCalls =
        [
            AssertableMethodCall.ForMethodWithoutParameters("Method1")
        ];
        
        var result = AssertableTextBuilder.CreateAssertableText(
            result: new { SomeProperty = "Some value" },
            capturedCalls: capturedCalls,
            options: null);

        await Verify(result);
    }
    
    [Fact]
    public async Task GivenResultAndOptionsWithManipulator_ThenItCallsTheManipulatorBeforeAddingJsonToAssertableText()
    {
        var result = AssertableTextBuilder.CreateAssertableText(
            result: new { SomeProperty = "Some value" },
            capturedCalls: Array.Empty<ICapturedCallInfo>(),
            options: new AssertableTextOptions(resultJsonStringManipulator: original => original.Replace("Some value", "Replaced value")));

        await Verify(result);
    }
    
    [Fact]
    public async Task GivenTwoDifferentCapturedCallsAndOptionsWithManipulatorForOneCallId_ThenItCallsTheManipulatorBeforeAddingTextToAssertableText()
    {
        IReadOnlyCollection<ICapturedCallInfo> capturedCalls =
        [
            AssertableMethodCall.ForMethodWithoutParameters("Method1"),
            AssertableMethodCall.ForMethodWithoutParameters("Method2")
        ];
        
        var result = AssertableTextBuilder.CreateAssertableText(
            result: null,
            capturedCalls: capturedCalls,
            options: new AssertableTextOptions(textManipulators: [new FuncTextManipulator("Method2", original => original.Replace("Method2", "Manipulated Method2"))]));

        await Verify(result);
    }
}

public class AssertableTextBuilderTestHarness(ITestOutputHelper testOutputHelper) : TestHarness(testOutputHelper)
{
    
}