using Reqnroll;

namespace IT;

[Binding]
public class TestHooks
{
    public static HttpClient Client = null!;
    private static CustomWebApplicationFactory _factory = null!;

    [BeforeTestRun]
    public static void Setup()
    {
        _factory = new CustomWebApplicationFactory();
        Client = _factory.CreateClient();
    }

    [AfterTestRun]
    public static void Teardown()
    {
        Client.Dispose();
        _factory.Dispose();
    }
}
