using System.ComponentModel.DataAnnotations;

namespace ShopNShop.Models
{
    public class OrderStatus
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        public List<Order> Orders { get; set; }
    }
}
