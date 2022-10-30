using CarvedRock.Core;

namespace CarvedRock.Domain;

public class ExtraLogic : IExtraLogic
{
    private readonly List<int> _locationIds = new() { 1, 5, 7, 9 };

    public Task<List<LocationInventory>> GetInventoryForProductsAsync(List<int> productIds)
    {
        Thread.Sleep(5000); // heavy remote operation
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
        
        return Task.FromResult(inventory);
    }

    public Task<Promotion?> GetPromotionForProductsAsync(List<int> productIds)
    {
        Thread.Sleep(5000); // heavy remote operation
        var rand = new Random();
        var productIndexForPromotion = rand.Next(-1, productIds.Count);

        if (productIndexForPromotion >= 0)
        {
            return Task.FromResult<Promotion?>(new Promotion
            {
                ProductId = productIds[productIndexForPromotion],
                Description = "Get 'em while they're hot!!",
                Discount = 0.15
            });
        }

        return Task.FromResult<Promotion?>(null);
    }
}
