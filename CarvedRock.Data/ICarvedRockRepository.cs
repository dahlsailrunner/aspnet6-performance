using CarvedRock.Data.Entities;

namespace CarvedRock.Data
{
    public interface ICarvedRockRepository
    {
        Task<List<Product>> GetProductsAsync(CancellationToken cancelToken, string category);
        Task<Product?> GetProductByIdAsync(int id);

        Task<List<Product>> GetProductListAsync(string category);
        List<Product> GetProductList(string category);
        Product? GetProductById(int id);
        Task<Product> AddNewProductAsync(Product product, bool invalidateCache);
    }
}
