using CarvedRock.Core;

namespace CarvedRock.Domain;

public interface IProductLogic 
{
    Task<IEnumerable<ProductModel>> GetProductsForCategoryAsync(CancellationToken cancelToken, 
        string category);
    Task<ProductModel?> GetProductByIdAsync(int id);
    Task<IEnumerable<ProductModel>> GetProductListForCategoryAsync(string category);
    IEnumerable<ProductModel> GetProductListForCategory(string category);
    ProductModel? GetProductById(int id);
    Task<ProductModel> AddNewProductAsync(ProductModel productToAdd, bool invalidateCache);
}