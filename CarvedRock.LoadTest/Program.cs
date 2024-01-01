using NBomber.CSharp;
using NBomber.Data;
using NBomber.Data.CSharp;
using NBomber.Http.CSharp;

namespace CarvedRock.LoadTest;

internal class Program
{
    private const string BaseUrl = "https://localhost:7213";
    private static readonly IDataFeed<string> ApiParams = DataFeed.Random(new List<string> { "all", "boots", "equip", "kayak" });

    static async Task Main(string[] args)
    {
        var httpClient = await ClientCredentialsFactory.CreateClient("testClient", BaseUrl);

        // ----------------------------------------------------
        // Get Products - Main API method
        // ----------------------------------------------------
        var getProductScenario = Scenario.Create("get_products", async context =>
        {
            var productCategory = ApiParams.GetNextItem(context.ScenarioInfo);

            var productStep = await Step.Run("get_products_from_api", context, async () =>
            {
                var request = Http.CreateRequest("GET", $"Product?Category={productCategory}");
                return await Http.Send(httpClient, request);
            });
            return Response.Ok();
        })
            .WithWarmUpDuration(TimeSpan.FromSeconds(5))
            .WithLoadSimulations(
                Simulation.Inject(rate: 200, interval: TimeSpan.FromSeconds(1), during: TimeSpan.FromSeconds(30)));

        // ----------------------------------------------------
        // Async version of API
        // ----------------------------------------------------        
        var asyncScenario = Scenario.Create("async_products", async context =>
        {
            var productCategory = ApiParams.GetNextItem(context.ScenarioInfo);

            var productStep = await Step.Run("get_products_from_api", context, async () =>
            {
                var request = Http.CreateRequest("GET", $"AsyncProduct?category={productCategory}");
                return await Http.Send(httpClient, request);
            });
            return Response.Ok();
        })
            .WithWarmUpDuration(TimeSpan.FromSeconds(5))
            .WithLoadSimulations(
                Simulation.Inject(rate: 200, interval: TimeSpan.FromSeconds(1), during: TimeSpan.FromSeconds(30)));
        
        // ----------------------------------------------------
        // Sync version of API
        // ----------------------------------------------------
        var syncScenario = Scenario.Create("async_products", async context =>
        {
            var productCategory = ApiParams.GetNextItem(context.ScenarioInfo);

            var productStep = await Step.Run("get_products_from_api", context, async () =>
            {
                var request = Http.CreateRequest("GET", $"SyncProduct?category={productCategory}");
                return await Http.Send(httpClient, request);
            });
            return Response.Ok();            
        }).WithWarmUpDuration(TimeSpan.FromSeconds(5))
            .WithLoadSimulations(
                Simulation.Inject(rate: 100, interval: TimeSpan.FromSeconds(1), during: TimeSpan.FromSeconds(30)));
        
        NBomberRunner
            .RegisterScenarios(  // comment out scenarios you don't want to run below
                //getProductScenario,
                //syncScenario
                asyncScenario
                )
            .Run();
    }
}
