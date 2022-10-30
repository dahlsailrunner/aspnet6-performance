using CarvedRock.Core;

namespace CarvedRock.Domain
{
    public interface IExtraLogic
    {
        public Task<List<LocationInventory>> GetInventoryForProductsAsync(List<int> productIds);
        public Task<Promotion?> GetPromotionForProductsAsync(List<int> productIds);
    }
}
