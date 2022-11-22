using CarvedRock.Core;

namespace CarvedRock.Domain;

public class ExtraLogic : IExtraLogic
{
    private readonly List<int> _locationIds = new() { 1, 5, 7, 9 };

    public async Task<List<LocationInventory>> GetInventoryForProductsAsync(List<int> productIds,
        CancellationToken cancelToken)
    {
        //await Task.Delay(200, cancelToken); // heavy remote operation
        var inventory = new List<LocationInventory>();
        foreach (var productId in productIds)
        {
            foreach (var location in _locationIds)
            {
                inventory.Add(new LocationInventory
                {
                    ProductId = productId,
                    LocationId = location,
                    OnHand = 1,
                    OnOrder = 2
                });
            }
        }
        
        return inventory;
    }

    public async Task<Promotion?> GetPromotionForProductsAsync(List<int> productIds,
        CancellationToken cancelToken)
    {
        //await Task.Delay(100, cancelToken); // heavy remote operation
        var rand = new Random();
        var productIndexForPromotion = rand.Next(-1, productIds.Count);

        if (productIndexForPromotion >= 0)
        {
            return new Promotion
            {
                ProductId = productIds[productIndexForPromotion],
                Description = "Get 'em while they're hot!!",
                Discount = 0.15
            };
        }

        return null;
    }
}
