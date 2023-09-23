using System.ComponentModel.DataAnnotations;

namespace ShopNShop.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public DateTime OrderDate { get; set; }

        [Required]
        public int OrderStatusId { get; set; }
        public OrderStatus OrderStatus { get; set; }

        [Required]
        public decimal TotalAmount { get; set; }

        public List<OrderDetails> OrderDetails { get; set; }
    }
}
