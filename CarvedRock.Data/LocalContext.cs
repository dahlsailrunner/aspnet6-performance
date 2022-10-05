using CarvedRock.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CarvedRock.Data
{
    public class LocalContext : DbContext
    {
        public DbSet<Product> Products { get; set; } = null!;
        public DbSet<ProductRating> ProductRatings { get; set; } = null!;

        public string DbPath { get; set; }

        public LocalContext()
        {
            var path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            DbPath = Path.Join(path, "carvedrock-logging.db");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options
                .UseSqlite($"Data Source={DbPath}")
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                .LogTo(Console.WriteLine, LogLevel.Information);

        public void MigrateAndCreateData()
        {
            Database.Migrate();

            if (Products.Any())
            {
                Products.RemoveRange(Products);
                SaveChanges();
            };

            Products.Add(new Product
            {
                Name = "Trailblazer",
                Category = "boots",
                Price = 69.99,
                Description = "Great support in this high-top to take you to great heights and trails.",
                ImgUrl = "https://www.pluralsight.com/content/dam/pluralsight2/teach/author-tools/carved-rock-fitness/img-brownboots.jpg",
                Rating = new ProductRating { AggregateRating = 4.2M, NumberOfRatings = 20 }
            });
            Products.Add(new Product
            {
                Name = "Coastliner",
                Category = "boots",
                Price = 49.99,
                Description =
                    "Easy in and out with this lightweight but rugged shoe with great ventilation to get your around shores, beaches, and boats.",
                ImgUrl = "https://www.pluralsight.com/content/dam/pluralsight2/teach/author-tools/carved-rock-fitness/img-greyboots.jpg",
                Rating = new ProductRating { AggregateRating = 4.6M, NumberOfRatings = 104 }
            });
            Products.Add(new Product
            {
                Name = "Woodsman",
                Category = "boots",
                Price = 64.99,
                Description =
                    "All the insulation and support you need when wandering the rugged trails of the woods and backcountry.",
                ImgUrl = "/images/shutterstock_222721876.jpg",
                Rating = new ProductRating { AggregateRating = 3.7M, NumberOfRatings = 68 }
            });
            Products.Add(new Product
            {
                Name = "Billy",
                Category = "boots",
                Price = 79.99,
                Description =
                    "Get up and down rocky terrain like a billy-goat with these awesome high-top boots with outstanding support.",
                ImgUrl = "/images/shutterstock_475046062.jpg"
            });
            Products.Add(new Product
            {
                Name = "Sherpa",
                Category = "equip",
                Price = 129.99,
                Description =
                    "Manage and carry your gear with ease using this backpack with great lumbar support.",
                ImgUrl = "/images/shutterstock_6170527.jpg"
            });
            Products.Add(new Product
            {
                Name = "Glide",
                Category = "kayak",
                Price = 399.99,
                Description =
                    "Navigate tricky waterways easily with this great and manageable kayak.",
                ImgUrl = "/images/shutterstock_645036007.jpg"
            });

            SaveChanges();
        }
    }
}
