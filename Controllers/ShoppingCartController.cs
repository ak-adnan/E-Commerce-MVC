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

            return RedirectToAction("CartDetails");
        }
        //cart view-------------------------------------
        public IActionResult CartDetails()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account"); 
            }

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Retrieve the cart items for the user
            var cartItems = _context.ShoppingCarts
                .Include(cart => cart.CartDetails)
                .ThenInclude(cd => cd.Product) 
                .FirstOrDefault(cart => cart.UserId == userId)?
                .CartDetails;

            return View(cartItems); 
        }
        // remove from cart function 
        public IActionResult RemoveFromCart(int cartDetailsId)
        {
            
            var cartItem = _context.CartDetails.FirstOrDefault(item => item.CartDetailsId == cartDetailsId);

            if (cartItem != null)
            {
                _context.CartDetails.Remove(cartItem);
                _context.SaveChanges();
            }

            return Ok(); // Return a success status code
        }


        [HttpGet]
        public IActionResult GetCartItemCount()
        {
            if (!User.Identity.IsAuthenticated)
            {
                // If the user is not authenticated, return 0
                return Ok(0);
            }

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            ShoppingCart shoppingCart = _context.ShoppingCarts
                .Include(cart => cart.CartDetails)
                .FirstOrDefault(cart => cart.UserId == userId);

            if (shoppingCart == null || shoppingCart.CartDetails == null)
            {
                // If the cart doesn't exist or is empty, return 0
                return Ok(0);
            }

            int itemCount = shoppingCart.CartDetails.Sum(item => item.Quantity);

            return Ok(itemCount);
        }
    }
}
