namespace CarvedRock.Core
{
    public class LocationInventory
    {
        public int ProductId { get; set; }
        public int LocationId { get; set; }
        public int OnHand { get; set; }
        public int OnOrder { get; set; }
    }

    public class Promotion
    {
        public int ProductId { get; set; }
        public string Description { get; set; }
        public double Discount { get; set; }
    }

    public class LocalClaim
    {
        public string Type { get; set; }
        public string Value { get; set; }

        public override string ToString() => $"{Type} : {Value}";
    }

}
