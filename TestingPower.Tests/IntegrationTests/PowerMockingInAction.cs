using System.Net;
using System.Text;
using FluentAssertions;
using TestingPower.PowerMocking;
using TestingPower.PowerMocking.TestableImplementations;
using TestingPower.Tests.IntegrationTests;
using TestingPower.WebApi;
using Xunit.Abstractions;

namespace TestingPower.Tests.PowerAssertions.AssertableText;

public class PowerMockingInAction(ITestOutputHelper testOutputHelper) : TestHarness(testOutputHelper)
{
    [Fact]
    public async Task GivenCapturedCallInfoWithVerboseDetails_ThenItBuildsTheExpectedText()
    {
        var powerMock = new PowerMock();
        powerMock.PrepareForCallWithReturnValue(
            nameof(IHttpClient.SendAsync),
            CreateJsonResponseMessage(new Pet(123, "Fido", 2)));
        var sut = new DemoService(new RestApiClient(new TestableHttpClient(powerMock), LogTestOutput));

        var result = await sut.GetPet(123);

        LogTestOutput(powerMock.CreateAssertableText(result));
        result.Should().NotBeNull();
    }

    private HttpResponseMessage CreateJsonResponseMessage(
        object content,
        HttpStatusCode statusCode = HttpStatusCode.OK)
    {
        return new HttpResponseMessage(statusCode)
        {
            Content = new StringContent(
                JsonSerializerForTests.ToJsonString(content),
                Encoding.UTF8,
                "application/json")
        };
    }
}