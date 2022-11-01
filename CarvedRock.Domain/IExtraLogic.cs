using CarvedRock.Core;

namespace CarvedRock.Domain
{
    public interface IExtraLogic
    {
        public Task<List<LocationInventory>> GetInventoryForProductsAsync(List<int> productIds, 
            CancellationToken cancelToken);
        public Task<Promotion?> GetPromotionForProductsAsync(List<int> productIds,
            CancellationToken cancelToken);
    }
}
