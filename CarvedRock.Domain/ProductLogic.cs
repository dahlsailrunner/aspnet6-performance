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
    public ProductLogic(ILogger<ProductLogic> logger, ICarvedRockRepository repo)
    {
        _logger = logger;
        _repo = repo;
    }
    public async Task<IEnumerable<ProductModel>> GetProductsForCategoryAsync(string category)
    {               
        _logger.LogInformation("Getting products in logic for {category}", category);

        Activity.Current?.AddEvent(new ActivityEvent("Getting products from repository"));
        var products = await _repo.GetProductsAsync(category);

        var results = new List<ProductModel>();
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

    public IEnumerable<ProductModel> GetProductsForCategory(string category)
    {
        var products =  _repo.GetProducts(category);

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

    private static ProductModel ConvertToProductModel(Product product)
    {
        var productToAdd = new ProductModel
        {
            Id = product.Id,
            Category = product.Category,
            Description = product.Description,
            ImgUrl = product.ImgUrl,
            Name = product.Name
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