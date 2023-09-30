using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopNShop.Data;
using ShopNShop.Models;
using System.Security.Claims;

namespace ShopNShop.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ShoppingCartController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult AddToCart(int productId, int quantity)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            ShoppingCart shoppingCart = _context.ShoppingCarts
                .Include(cart => cart.CartDetails)
                .FirstOrDefault(cart => cart.UserId == userId);

            if (shoppingCart == null)
            {
                // If the cart doesn't exist, create one
                shoppingCart = new ShoppingCart { UserId = userId };
                _context.ShoppingCarts.Add(shoppingCart);
            }

            // Check if the product is already in the cart
            if (shoppingCart.CartDetails != null)
            {
                CartDetails cartItem = shoppingCart.CartDetails
                    .FirstOrDefault(item => item.ProductId == productId);

                if (cartItem != null)
                {
                    // If the product is already in the cart, update the quantity
                    cartItem.Quantity += quantity;
                }
                else
                {
                    // If the product is not in the cart, add it
                    shoppingCart.CartDetails.Add(new CartDetails
                    {
                        ProductId = productId,
                        Quantity = quantity
                    });
                }
            }
            else
            {
                // If cart details are null, create a new list and add the item
                shoppingCart.CartDetails = new List<CartDetails>
        {
            new CartDetails
            {
                ProductId = productId,
                Quantity = quantity
            }
        };
            }

            _context.SaveChanges();

            // Redirect to the cart view or wherever you want to go after adding to the cart
            return RedirectToAction("Cart");
        }


    }
}
