using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs
{
    public class CartItemDto
    {
        public int Id { get; set; }
        public string ProductName { get; set; } = null!;
        public string PictureURL { get; set; } = null!;
        [Range(1,double.MaxValue)]
        public decimal Price { get; set; }
        [Range(1, 100)]
        public int Quantity { get; set; }
    }
}