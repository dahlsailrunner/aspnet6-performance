using CarvedRock.Domain;
using Microsoft.AspNetCore.Authorization;

namespace CarvedRock.Aot.Api;

public static class ProductEndpoints
{
    public static void Map(WebApplication app)
    {
        var productsApi = app.MapGroup("/products");

        productsApi.MapGet("/", [Authorize] async
            (IProductLogic productLogic, ILogger<Program> logger, string category = "all") =>
        {
            using (logger.BeginScope("ScopeCat: {ScopeCat}", category))
            {
                logger.LogInformation("Getting products in API.");
                return await productLogic.GetProductListForCategoryAsync(category);
            }
        });
    }
}
