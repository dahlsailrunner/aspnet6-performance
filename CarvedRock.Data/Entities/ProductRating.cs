namespace CarvedRock.Data.Entities
{
    public class ProductRating
    {
        public int Id { get; set; }
        public decimal AggregateRating { get; set; }
        public int NumberOfRatings { get; set; }
    }
}
