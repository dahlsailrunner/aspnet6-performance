using NBomber.Contracts;
using NBomber.CSharp;

namespace CarvedRock.LoadTest;

internal class Program
{
    private const string BaseUrl = "https://localhost:7213";
    private static readonly IFeed<string> ApiParams = Feed.CreateRandom("categories",
        new List<string> { "all", "boots", "equip", "kayak" });

    static void Main(string[] args)
    {
        // ----------------------------------------------------
        // Get Products - Main API method
        // ----------------------------------------------------
        var psClientFactory = ClientCredentialsFactory.GetClientFactory("psFact", BaseUrl);
        var productStep = Step.Create(
            name: "get_products_from_api",
            clientFactory: psClientFactory,
            feed: ApiParams,
            execute: async context =>
            {
                var response = await context.Client.GetAsync(
                    $"Product?Category={context.FeedItem}");

                return response.IsSuccessStatusCode
                    ? Response.Ok(statusCode: (int)response.StatusCode)
                    : Response.Fail(statusCode: (int)response.StatusCode);
            },
            timeout: TimeSpan.FromSeconds(10)
        );

        var getProductsScenario = ScenarioBuilder
            .CreateScenario("get_products", productStep)
            .WithWarmUpDuration(TimeSpan.FromSeconds(5))
            .WithLoadSimulations(
                Simulation.InjectPerSec(rate: 200, during: TimeSpan.FromSeconds(30)));

        // ----------------------------------------------------
        // Async version of API
        // ----------------------------------------------------
        var asyncFactory = ClientCredentialsFactory.GetClientFactory("asyncFact", BaseUrl);
        var asyncStep = Step.Create(
            name: "simple_async",
            clientFactory: asyncFactory,
            feed: ApiParams,
            execute: async context =>
            {
                var response = await context.Client.GetAsync(
                    $"AsyncProduct?category={context.FeedItem}");

                return response.IsSuccessStatusCode
                    ? Response.Ok(statusCode: (int)response.StatusCode)
                    : Response.Fail(statusCode: (int)response.StatusCode);
            },
            timeout: TimeSpan.FromSeconds(10));

        var asyncScenario = ScenarioBuilder
            .CreateScenario("async_products", asyncStep)
            .WithWarmUpDuration(TimeSpan.FromSeconds(5))
            .WithLoadSimulations(
                Simulation.InjectPerSec(rate: 200, during: TimeSpan.FromSeconds(30)));

        // ----------------------------------------------------
        // Sync version of API
        // ----------------------------------------------------
        var syncFactory = ClientCredentialsFactory.GetClientFactory("syncFact", BaseUrl);
        var syncStep = Step.Create(
            name: "simple_sync",
            clientFactory: syncFactory,
            execute: async context =>
            {
                var response = await context.Client.GetAsync("SyncProduct?category=all");

                return response.IsSuccessStatusCode
                    ? Response.Ok(statusCode: (int)response.StatusCode)
                    : Response.Fail(statusCode: (int)response.StatusCode);
            },
            timeout: TimeSpan.FromSeconds(15));

        var syncScenario = ScenarioBuilder
            .CreateScenario("sync_products", syncStep)
            .WithWarmUpDuration(TimeSpan.FromSeconds(5))
            .WithLoadSimulations(
                Simulation.RampPerSec(rate: 100, during: TimeSpan.FromSeconds(30)));

        NBomberRunner
            .RegisterScenarios(  // comment out scenarios you don't want to run below
                                 //getProductsScenario,
                syncScenario
                //asyncScenario
                )
            .Run();
    }
}
