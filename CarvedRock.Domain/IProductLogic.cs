using CarvedRock.Core;

namespace CarvedRock.Domain;

public interface IProductLogic 
{
    Task<IEnumerable<ProductModel>> GetProductsForCategoryAsync(string category);
    Task<ProductModel?> GetProductByIdAsync(int id);
    IEnumerable<ProductModel> GetProductsForCategory(string category);
    ProductModel? GetProductById(int id);
}