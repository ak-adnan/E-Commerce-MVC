﻿using System.ComponentModel.DataAnnotations;

namespace ShopNShop.Models
{
    public class ShoppingCart
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        public List<CartDetails> CartDetails { get; set; }
    }
}
