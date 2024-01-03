using System.Diagnostics;
using System.Text;
using System.Text.Json;
using CarvedRock.Data.Entities;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace CarvedRock.Data;

public class CarvedRockRepository : ICarvedRockRepository
{
    private readonly LocalContext _ctx;
    private readonly ILogger<CarvedRockRepository> _logger;
    private readonly IMemoryCache _memoryCache;
    private readonly IDistributedCache _distCache;
    private readonly ILogger _factoryLogger;

    public CarvedRockRepository(LocalContext ctx, ILogger<CarvedRockRepository> logger,
        ILoggerFactory loggerFactory, IMemoryCache memoryCache, IDistributedCache distCache)
    {
        _ctx = ctx;
        _logger = logger;
        _memoryCache = memoryCache;
        _distCache = distCache;
        _factoryLogger = loggerFactory.CreateLogger("DataAccessLayer");
    }

    public async Task<List<Product>> GetProductsAsync(CancellationToken cancelToken, string category)
    {
        _logger.LogInformation("Getting products in repository for {category}", category);

        try
        {
            var cacheKey = $"products_{category}";
            //if (!_memoryCache.TryGetValue(cacheKey, out List<Product> results))
            //{
            //    Thread.Sleep(5000);  // simulates heavy query
            //    results = await _ctx.Products
            //        .Where(p => p.Category == category || category == "all")
            //        .Include(p => p.Rating).ToListAsync();

            //    _memoryCache.Set(cacheKey, results, TimeSpan.FromMinutes(2));
            //}

            var distResults = await _distCache.GetAsync(cacheKey, cancelToken);

            if (distResults == null)
            {
                var timer = new Stopwatch();
                timer.Start();

                await Task.Delay(500, cancelToken); // simulates heavy query
                var productsToSerialize = await _ctx.Products
                    .Where(p => p.Category == category || category == "all")
                    .Include(p => p.Rating).ToListAsync(cancelToken);

                var serialized = JsonSerializer.Serialize(productsToSerialize,
                    CacheSourceGenerationContext.Default.ListProduct);

                await _distCache.SetAsync(cacheKey, Encoding.UTF8.GetBytes(serialized),
                    new DistributedCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
                    }, cancelToken);

                timer.Stop();
                _logger.LogInformation("Database and caching took {ElapsedMs} milliseconds",
                    timer.ElapsedMilliseconds);
                return productsToSerialize;
            }

            var results = JsonSerializer.Deserialize(Encoding.UTF8.GetString(distResults),
                CacheSourceGenerationContext.Default.ListProduct);

            return results ?? new List<Product>();
        }
        catch (Exception ex)
        {
            var newEx = new ApplicationException("Something bad happened in database", ex);
            newEx.Data.Add("Category", category);
            throw newEx;
        }
    }

    public async Task<Product?> GetProductByIdAsync(int id)
    {
        return await _ctx.Products.FindAsync(id);
    }

    [DapperAot]
    public async Task<List<Product>> GetProductListAsync(string category)
    {
        //await Task.Delay(5000); // simulates heavy query
        if (string.Equals(category, "error", StringComparison.InvariantCultureIgnoreCase))
            throw new Exception("Something bad happened");

        var db = _ctx.Database.GetDbConnection();
        return (await db.QueryAsync<Product>(
            "SELECT * FROM Products WHERE Category = @category OR @category = 'all'",
            new { category })).ToList();

        //return await _ctx.Products.Where(p => p.Category == category || category == "all")
        //    .ToListAsync();
    }

    public List<Product> GetProductList(string category)
    {
        Thread.Sleep(5000); // simulates heavy query
        return _ctx.Products.Where(p => p.Category == category || category == "all")
            .ToList();
    }

    public Product? GetProductById(int id)
    {
        var timer = new Stopwatch();
        timer.Start();

        var product = _ctx.Products.Find(id);
        timer.Stop();

        _logger.LogDebug("Querying products for {id} finished in {milliseconds} milliseconds",
            id, timer.ElapsedMilliseconds);

        _factoryLogger.LogInformation("(F) Querying products for {id} finished in {ticks} ticks",
            id, timer.ElapsedTicks);

        return product;
    }

    public async Task<Product> AddNewProductAsync(Product product, bool invalidateCache)
    {
        _ctx.Products.Add(product);
        await _ctx.SaveChangesAsync();

        if (invalidateCache)
        {
            var cacheKey = $"products_{product.Category}";
            await _distCache.RemoveAsync(cacheKey);
        }

        return product;
    }
}
