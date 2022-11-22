using CarvedRock.Core;
using System.Text.Json;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using Newtonsoft.Json;

namespace CarvedRock.Benchmarking;

internal class Program
{
    static void Main(string[] args)
    {
        BenchmarkRunner.Run<JsonComparisons>();
    }
}

public class JsonComparisons
{
    private readonly List<ProductModel> _products;
    private readonly JsonSerializerOptions _jsonOptions;
    public JsonComparisons()
    {
        _jsonOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web);

        _products = new List<ProductModel>
        {
            new ProductModel
            {
                Id = 1, Name = "First Product", Category = "boots",
                Description = "Most awesome set of boots", ImgUrl = "https://someurl/firstimage",
                NumberOfRatings = 5, Price = 20.99, Rating = 4.2M
            },
            new ProductModel
            {
                Id = 2, Name = "Second Product", Category = "boots",
                Description = "Second-most awesome set of boots", ImgUrl = "https://someurl/secondimage",
                NumberOfRatings = 22, Price = 30.99, Rating = 4.7M
            },
            new ProductModel
            {
                Id = 3, Name = "Third Product", Category = "boots",
                Description = "Third-most awesome set of boots", ImgUrl = "https://someurl/thirdimage",
                NumberOfRatings = 2, Price = 19.99, Rating = 4.1M
            }
        };
    }

    [Benchmark]
    public string Newtonsoft() => JsonConvert.SerializeObject(_products);

    [Benchmark]
    public string SystemTextJson() => System.Text.Json.JsonSerializer.Serialize(_products, _jsonOptions);

    [Benchmark]
    public string SourceGenerator() => System.Text.Json.JsonSerializer.Serialize(_products,
        ProductModelGenerationContext.Default.ListProductModel);
}