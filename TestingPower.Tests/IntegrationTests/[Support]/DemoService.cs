using TestingPower.PowerAssertions.CapturedCalls;
using TestingPower.PowerMocking;
using TestingPower.WebApi;

namespace TestingPower.Tests.IntegrationTests;

public interface IDemoService
{
    int AddNumbers(int a, int b);

    Task<Pet> GetPet(int petId);
}

public class Pet
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public int Age { get; set; }
    
    public Pet()
    {
    }

    public Pet(
        int id,
        string name,
        int age)
    {
        Id = id;
        Name = name;
        Age = age;
    }
}

public class DemoRestApiClient(ICustomHttpClientFactory httpClientFactory)
    : RestApiClient(httpClientFactory.CreateClient(), null)
{
}

/// <summary>
/// Simulation of a production service that has to be replaced when running tests. Should normally be a third party
/// dependency and/or a service that makes calls to a database or an HTTP REST API 
/// </summary>
public class DemoService(RestApiClient restApiClient) : IDemoService
{
    public int AddNumbers(int a, int b)
    {
        return a + b;
    }

    public async Task<Pet> GetPet(int petId)
    {
        var response = await restApiClient.GetAsync($"api/v1/pets/{petId}");
        
        return response.TryGetContentAs<Pet>();
    }
}

public interface ICustomHttpClientFactory
{
    IHttpClient CreateClient();
}

/// <summary>
/// A testable (handwritten fake) that replaces the production service when running tests.
/// </summary>
/// <param name="powerMock"></param>
public class TestableDemoService(PowerMock powerMock) : IDemoService
{
    //Example of how you can use the PowerMock to handle calls
    public int AddNumbers(int a, int b) => powerMock.HandleCallWithReturnValue<int>(
        AssertableMethodCall.ForMethodWithParameters(
            GetUniqueCallId(nameof(AddNumbers)),
            new
            {
                a,
                b
            })).GetAwaiter().GetResult();

    public Task<Pet> GetPet(int petId)
    {
        //No need for faking this method. You should write tests directly against the production service!
        throw new NotImplementedException();
    }

    //The unique call id could be any string, but for clarity it is suitable to use a qualified name that is readable and easy to understand.
    //It normally makes sense to use the name of the class that owns the method and the method name itself. If there is a risk of ambiguity
    //you could also include the namespace of the class.
    internal static string GetUniqueCallId(string methodName) => $"{nameof(IDemoService)}.{methodName}";
}