namespace Domain.Models.Carts
{
    public class CartItem
    {
        public int Id { get; set; }
        public string ProductName { get; set; } = null!;
        public string PictureURL { get; set; } = null!;
        public decimal Price { get; set; }
        public int Quantity { get; set; }

    }
}