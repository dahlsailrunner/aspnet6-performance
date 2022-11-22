using System.Diagnostics;
using CarvedRock.Core;
using CarvedRock.Data;
using CarvedRock.Data.Entities;
using Microsoft.Extensions.Logging;

namespace CarvedRock.Domain;

public class ProductLogic : IProductLogic
{
    private readonly ILogger<ProductLogic> _logger;
    private readonly ICarvedRockRepository _repo;
    private readonly IExtraLogic _extraLogic;
    private readonly IApiCaller _apiCaller;

    public ProductLogic(ILogger<ProductLogic> logger, ICarvedRockRepository repo, IExtraLogic extraLogic,
        IApiCaller apiCaller)
    {
        _logger = logger;
        _repo = repo;
        _extraLogic = extraLogic;
        _apiCaller = apiCaller;
    }
    public async Task<IEnumerable<ProductModel>> GetProductsForCategoryAsync(CancellationToken cancelToken, 
        string category)
    {               
        _logger.LogInformation("Getting products in logic for {category}", category);

        Activity.Current?.AddEvent(new ActivityEvent("Getting products from repository"));

        var products = await _repo.GetProductsAsync(cancelToken, category);

        // REMOVED FOR LOAD TESTS
        //var apiResults = await _apiCaller.CallExternalApiAsync();
        
        _logger.LogInformation("ABOUT TO MAKE EXTRA ASYNC CALLS");
        // TWO NEW SEQUENTIAL ASYNC CALLS HERE
        //var inventory = await _extraLogic.GetInventoryForProductsAsync(
        //    products.Select(p => p.Id).ToList());
        //_logger.LogInformation("finished getting {count} inventory records", inventory.Count);

        //var promotion = await _extraLogic.GetPromotionForProductsAsync(
        //    products.Select(p => p.Id).ToList());
        //_logger.LogInformation("got promotion for product id {id}", promotion?.ProductId);

        // instead of using HttpContext in parallel tasks - grab what you need 
        // here and pass as arg(s) -- like the path or an access token or whatever
        var invTask = _extraLogic.GetInventoryForProductsAsync(
                products.Select(p => p.Id).ToList(), cancelToken);
        var promotionTask = _extraLogic.GetPromotionForProductsAsync(
                products.Select(p => p.Id).ToList(), cancelToken);

        await Task.WhenAll(invTask, promotionTask);

        var inventory = await invTask;
        _logger.LogInformation("finished getting {count} inventory records", inventory.Count);
        var promotion = await promotionTask;
        _logger.LogInformation("got promotion for product id {id}", promotion?.ProductId);

        var results = new List<ProductModel>();
        // TODO: merge inventory and promotion results into product models
        foreach (var product in products)
        {
            var productToAdd = ConvertToProductModel(product);
            results.Add(productToAdd);
        }
        
        Activity.Current?.AddEvent(new ActivityEvent("Retrieved products from repository"));

        return results;
    }

    public async Task<ProductModel?> GetProductByIdAsync(int id)
    {
        var product = await _repo.GetProductByIdAsync(id);
        return product != null ? ConvertToProductModel(product) : null;
    }

    public async Task<IEnumerable<ProductModel>> GetProductListForCategoryAsync(string category)
    {
        var products =  await _repo.GetProductListAsync(category);

        var results = new List<ProductModel>();
        foreach (var product in products)
        {
            var productToAdd = ConvertToProductModel(product);
            results.Add(productToAdd);
        }

        return results;
    }

    public IEnumerable<ProductModel> GetProductListForCategory(string category)
    {
        var products = _repo.GetProductList(category);

        var results = new List<ProductModel>();
        foreach (var product in products)
        {
            var productToAdd = ConvertToProductModel(product);
            results.Add(productToAdd);
        }

        return results;
    }

    public ProductModel? GetProductById(int id)
    {
        var product = _repo.GetProductById(id);
        return product != null ? ConvertToProductModel(product) : null;
    }

    public async Task<ProductModel> AddNewProductAsync(ProductModel productToAdd, bool invalidateCache)
    {
        var product = new Product
        {
            Category = productToAdd.Category,
            Description = productToAdd.Description,
            ImgUrl = productToAdd.ImgUrl,
            Name = productToAdd.Name,
            Price = productToAdd.Price
        };
        var addedProduct = await _repo.AddNewProductAsync(product, invalidateCache);
        return ConvertToProductModel(addedProduct);
    }

    private static ProductModel ConvertToProductModel(Product product)
    {
        var productToAdd = new ProductModel
        {
            Id = product.Id,
            Category = product.Category,
            Description = product.Description,
            ImgUrl = product.ImgUrl,
            Name = product.Name,
            Price = product.Price
        };
        var rating = product.Rating;
        if (rating != null)
        {
            productToAdd.Rating = rating.AggregateRating;
            productToAdd.NumberOfRatings = rating.NumberOfRatings;
        }

        return productToAdd;
    }

    
}