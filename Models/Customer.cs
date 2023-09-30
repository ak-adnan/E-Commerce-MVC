using Microsoft.AspNetCore.Identity;

namespace ShopNShop.Models
{
    public class Customer: IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; } 
        public string Contact { get; set; }

    }
}
