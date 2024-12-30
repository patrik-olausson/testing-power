using System.Security.Authentication;
using FluentAssertions;
using TestingPower.PowerAssertions.CapturedCalls;
using TestingPower.PowerMocking;
using Xunit.Abstractions;

namespace TestingPower.Tests.PowerMocking.PowerMockTests;

public class HandleCallWithReturnValue(ITestOutputHelper testOutputHelper) : PowerMockTestHarness(testOutputHelper)
{
    [Fact]
    public async Task WhenNotPreparingForCall_ThenItThrowsExceptionWhenHandlingCall()
    {
        var sut = CreateSut();
        
        await AssertThrowsAnyAsync<PowerMockCallNotPreparedException>(() => sut.HandleCallWithReturnValue<int>(AssertableMethodCall.ForMethodWithoutParameters("UniqueCallId")));
    }
    
    [Fact]
    public async Task WhenPreparingCallWithReturnValueUsingDefaultNumberOfCalls_ThenItIsPossibleToHandleOneCall()
    {
        const string uniqueCallId = "UniqueCallId";
        var sut = CreateSut();
        sut.PrepareForCallWithReturnValue(uniqueCallId, 10);

        var result = await sut.HandleCallWithReturnValue<int>(AssertableMethodCall.ForMethodWithoutParameters(uniqueCallId));

        result.Should().Be(10);
        sut.GetCapturedCalls(uniqueCallId).Should().HaveCount(1);
    }
    
    [Fact]
    public async Task WhenPreparingCallWithReturnValueUsingDefaultNumberOfCalls_ThenItThrowsExceptionWhenHandlingSecondCall()
    {
        const string uniqueCallId = "UniqueCallId";
        var sut = CreateSut();
        sut.PrepareForCallWithReturnValue(uniqueCallId, 10);
        await sut.HandleCallWithReturnValue<int>(AssertableMethodCall.ForMethodWithoutParameters(uniqueCallId));
        
        await AssertThrowsAnyAsync<PowerMockCallNotPreparedException>(() => sut.HandleCallWithReturnValue<int>(AssertableMethodCall.ForMethodWithoutParameters(uniqueCallId)));
    }
    
    [Fact]
    public async Task WhenPreparingCallToReturnTheValueTwoTimes_ThenItIsPossibleToHandleTwoCalls()
    {
        const string uniqueCallId = "UniqueCallId";
        var sut = CreateSut();
        sut.PrepareForCallWithReturnValue(uniqueCallId, 10, numberOfCalls: 2);
        
        var firstResult = await sut.HandleCallWithReturnValue<int>(AssertableMethodCall.ForMethodWithoutParameters(uniqueCallId));
        var secondResult = await sut.HandleCallWithReturnValue<int>(AssertableMethodCall.ForMethodWithoutParameters(uniqueCallId));
        
        firstResult.Should().Be(10);
        secondResult.Should().Be(10);
        sut.GetCapturedCalls(uniqueCallId).Should().HaveCount(2);
    }
    
    [Fact]
    public async Task WhenPreparingCallWithReturnValueForReferenceTypeThatIsNull_ThenItIsPossibleToHandleCallAndGetNullAsReturnValue()
    {
        const string uniqueCallId = "UniqueCallId";
        var sut = CreateSut();
        sut.PrepareForCallWithReturnValue(uniqueCallId, null);

        var result = await sut.HandleCallWithReturnValue<object>(AssertableMethodCall.ForMethodWithoutParameters(uniqueCallId));

        result.Should().BeNull();
    }
    
    [Fact]
    public async Task WhenPreparingCallWithReturnValueForReferenceTypeThatIsNull_ThenItThrowsExceptionIfTryingToCastValueToValueType()
    {
        const string uniqueCallId = "UniqueCallId";
        var sut = CreateSut();
        sut.PrepareForCallWithReturnValue(uniqueCallId, null);

        await AssertThrowsAnyAsync<PowerMockInvalidTypeException>(() => sut.HandleCallWithReturnValue<int>(AssertableMethodCall.ForMethodWithoutParameters(uniqueCallId)));
    }
    
    [Fact]
    public void WhenPreparingCallWithNumberOfCallsSetToLessThanOne_ThenItThrowsArgumentOutOfRangeException()
    {
        const string uniqueCallId = "UniqueCallId";
        var sut = CreateSut();
        
        AssertThrowsAny<ArgumentOutOfRangeException>(() => sut.PrepareForCallWithReturnValue(uniqueCallId, 10, numberOfCalls: 0));
    }
}

public class HandleCallWithoutReturnValue(ITestOutputHelper testOutputHelper) : PowerMockTestHarness(testOutputHelper)
{
    [Fact]
    public async Task WhenNotPreparingForCall_ThenItThrowsExceptionWhenHandlingCall()
    {
        var sut = CreateSut();
        
        await AssertThrowsAnyAsync<PowerMockCallNotPreparedException>(() => sut.HandleCallWithoutReturnValue(AssertableMethodCall.ForMethodWithoutParameters("UniqueCallId")));
    }
    
    [Fact]
    public async Task WhenPreparingCallWithReturnValueUsingDefaultNumberOfCalls_ThenItIsPossibleToHandleOneCall()
    {
        const string uniqueCallId = "UniqueCallId";
        var sut = CreateSut();
        sut.PrepareForCallWithoutReturnValue(uniqueCallId);

        await sut.HandleCallWithoutReturnValue(AssertableMethodCall.ForMethodWithoutParameters(uniqueCallId));
        
        sut.GetCapturedCalls(uniqueCallId).Should().HaveCount(1);
    }
    
    [Fact]
    public async Task WhenPreparingCallWithReturnValueUsingDefaultNumberOfCalls_ThenItThrowsExceptionWhenHandlingSecondCall()
    {
        const string uniqueCallId = "UniqueCallId";
        var sut = CreateSut();
        sut.PrepareForCallWithoutReturnValue(uniqueCallId);
        await sut.HandleCallWithoutReturnValue(AssertableMethodCall.ForMethodWithoutParameters(uniqueCallId));
        
        await AssertThrowsAnyAsync<PowerMockCallNotPreparedException>(() => sut.HandleCallWithoutReturnValue(AssertableMethodCall.ForMethodWithoutParameters(uniqueCallId)));
    }
    
    [Fact]
    public async Task WhenPreparingCallToReturnTheValueTwoTimes_ThenItIsPossibleToHandleTwoCalls()
    {
        const string uniqueCallId = "UniqueCallId";
        var sut = CreateSut();
        sut.PrepareForCallWithoutReturnValue(uniqueCallId, numberOfCalls: 2);
        
        await sut.HandleCallWithoutReturnValue(AssertableMethodCall.ForMethodWithoutParameters(uniqueCallId));
        await sut.HandleCallWithoutReturnValue(AssertableMethodCall.ForMethodWithoutParameters(uniqueCallId));
        
        sut.GetCapturedCalls(uniqueCallId).Should().HaveCount(2);
    }
    
    [Fact]
    public void WhenPreparingCallWithNumberOfCallsSetToLessThanOne_ThenItThrowsArgumentOutOfRangeException()
    {
        const string uniqueCallId = "UniqueCallId";
        var sut = CreateSut();
        
        AssertThrowsAny<ArgumentOutOfRangeException>(() => sut.PrepareForCallWithoutReturnValue(uniqueCallId, numberOfCalls: 0));
    }
}

public class PrepareForCallThatShouldThrowException(ITestOutputHelper testOutputHelper) : PowerMockTestHarness(testOutputHelper)
{
    [Fact]
    public async Task WhenPreparingCallToThrowException_ThenItThrowsTheExpectedExceptionWhenHandlingCallWithReturnValue()
    {
        const string uniqueCallId = "UniqueCallId";
        var sut = CreateSut();
        sut.PrepareForCallThatShouldThrowException(uniqueCallId, new AuthenticationException("Fake exception"));

        await AssertThrowsAnyAsync<AuthenticationException>(() => sut.HandleCallWithReturnValue<string>(AssertableMethodCall.ForMethodWithoutParameters(uniqueCallId)));
        sut.GetCapturedCalls(uniqueCallId).Should().HaveCount(1);
    }
    
    [Fact]
    public async Task WhenPreparingCallToThrowException_ThenItThrowsTheExpectedExceptionWhenHandlingCallWithoutReturnValue()
    {
        const string uniqueCallId = "UniqueCallId";
        var sut = CreateSut();
        sut.PrepareForCallThatShouldThrowException(uniqueCallId, new AuthenticationException("Fake exception"));

        await AssertThrowsAnyAsync<AuthenticationException>(() => sut.HandleCallWithoutReturnValue(AssertableMethodCall.ForMethodWithoutParameters(uniqueCallId)));
        sut.GetCapturedCalls(uniqueCallId).Should().HaveCount(1);
    }
    
    [Fact]
    public void WhenPreparingCallToThrowExceptionButNullIsProvidendInstadOfExceptionInstance_ThenItThrowsArgumentNullException()
    {
        const string uniqueCallId = "UniqueCallId";
        var sut = CreateSut();
        AssertThrowsAny<ArgumentNullException>(() => sut.PrepareForCallThatShouldThrowException(uniqueCallId, null!));
    }
}

public class GetAllCapturedCalls(ITestOutputHelper testOutputHelper) : PowerMockTestHarness(testOutputHelper)
{
    [Fact]
    public void GivenNoCallsHasBeenMade_ThenItReturnsAnEmptyCollection()
    {
        var sut = CreateSut();

        var result = sut.GetAllCapturedCalls();

        result.Should().BeEmpty();
    }
    
    [Fact]
    public async Task GivenOneCallHasBeenMade_ThenItReturnsCollectionWithOneCapturedCall()
    {
        const string uniqueCallId = "UniqueCallId";
        var sut = CreateSut();
        sut.PrepareForCallWithoutReturnValue(uniqueCallId);
        await sut.HandleCallWithoutReturnValue(AssertableMethodCall.ForMethodWithoutParameters(uniqueCallId));

        var result = sut.GetAllCapturedCalls();

        result.Should().HaveCount(1);
    }
    
    [Fact]
    public async Task GivenTwoDifferentCallsHasBeenMade_ThenItReturnsCollectionWithTwoCapturedCalls()
    {
        const string uniqueCallId1 = "UniqueCallId_1";
        const string uniqueCallId2 = "UniqueCallId_2";
        var sut = CreateSut();
        sut.PrepareForCallWithoutReturnValue(uniqueCallId1);
        await sut.HandleCallWithoutReturnValue(AssertableMethodCall.ForMethodWithoutParameters(uniqueCallId1));
        sut.PrepareForCallWithoutReturnValue(uniqueCallId2);
        await sut.HandleCallWithoutReturnValue(AssertableMethodCall.ForMethodWithoutParameters(uniqueCallId2));

        var result = sut.GetAllCapturedCalls();

        result.Should().HaveCount(2);
    }
}

public class GetCapturedCalls(ITestOutputHelper testOutputHelper) : PowerMockTestHarness(testOutputHelper)
{
    [Fact]
    public void GivenNoCallsHasBeenMade_ThenItReturnsAnEmptyCollection()
    {
        var sut = CreateSut();

        var result = sut.GetCapturedCalls("Call1");

        result.Should().BeEmpty();
    }
    
    [Fact]
    public async Task GivenOneCallHasBeenMade_ThenItReturnsCollectionWithOneCapturedCall()
    {
        const string uniqueCallId = "UniqueCallId";
        var sut = CreateSut();
        sut.PrepareForCallWithoutReturnValue(uniqueCallId);
        await sut.HandleCallWithoutReturnValue(AssertableMethodCall.ForMethodWithoutParameters(uniqueCallId));

        var result = sut.GetCapturedCalls(uniqueCallId);

        result.Should().HaveCount(1);
    }
    
    [Fact]
    public async Task GivenTwoDifferentCallsHasBeenMade_ThenItOnlyReturnsCollectionWithOneCapturedCallsForTheSpecifiedCallId()
    {
        const string uniqueCallId1 = "UniqueCallId_1";
        const string uniqueCallId2 = "UniqueCallId_2";
        var sut = CreateSut();
        sut.PrepareForCallWithoutReturnValue(uniqueCallId1);
        await sut.HandleCallWithoutReturnValue(AssertableMethodCall.ForMethodWithoutParameters(uniqueCallId1));
        sut.PrepareForCallWithoutReturnValue(uniqueCallId2);
        await sut.HandleCallWithoutReturnValue(AssertableMethodCall.ForMethodWithoutParameters(uniqueCallId2));

        var result = sut.GetCapturedCalls(uniqueCallId1);

        result.Should().HaveCount(1).And.Contain(capturedCall => capturedCall.Id == uniqueCallId1);
    }
}

public class PowerMockTestHarness(ITestOutputHelper testOutputHelper) : TestHarness(testOutputHelper)
{
    protected PowerMock CreateSut() => new PowerMock();
}
