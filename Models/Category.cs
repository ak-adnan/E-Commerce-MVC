using System.ComponentModel.DataAnnotations;

namespace ShopNShop.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        public List<Product> Products { get; set; }
    }
}
