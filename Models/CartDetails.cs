using System.ComponentModel.DataAnnotations;

namespace ShopNShop.Models
{
    public class CartDetails
    {
        [Key]
    public int CartDetailsId { get; set; }

        [Required]
        public int ShoppingCartId { get; set; }
        public ShoppingCart ShoppingCart { get; set; }

        [Required]
        public int ProductId { get; set; }
        public Product Product { get; set; }

        [Required]
        public int Quantity { get; set; }
    }
}
