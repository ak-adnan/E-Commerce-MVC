using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopNShop.Data;
using ShopNShop.Models;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ShopNShop.Controllers
{
    public class MyOrderController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MyOrderController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: MyOrder/Index
        public async Task<IActionResult> Index()
        {
            return _context.Orders != null ?
                        View(await _context.Orders.ToListAsync()) :
                        Problem("Entity set 'ApplicationDbContext.Orders'  is null.");
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Orders == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }
        [HttpPost]
        [Route("MyOrder/PlaceOrder")] // Corrected route attribute
        public async Task<IActionResult> PlaceOrder([FromBody] Order orderData)
        {
            // Check if the user is authenticated
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account"); // Redirect to the login page if not authenticated
            }

            try
            {
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                // Retrieve the user's shopping cart
                var shoppingCart = await _context.ShoppingCarts
                    .Include(cart => cart.CartDetails)
                    .FirstOrDefaultAsync(cart => cart.UserId == userId);

                if (shoppingCart == null || shoppingCart.CartDetails == null || shoppingCart.CartDetails.Count == 0)
                {
                    return RedirectToAction("CartDetails", "ShoppingCart"); // Redirect to the cart if it's empty
                }

                // Create a new order
                var newOrder = new Order
                {
                    UserId = Convert.ToInt32(userId),
                    OrderDate = DateTime.Now, // Set the order date to the current date and time
                    OrderStatusId = 1, // Replace with the appropriate order status ID
                    TotalAmount = shoppingCart.CartDetails.Sum(item => item.Product.Price * item.Quantity),
                    OrderDetails = shoppingCart.CartDetails.Select(item => new OrderDetails
                    {
                        ProductId = item.ProductId,
                        Quantity = item.Quantity
                    }).ToList()
                };

                // Add the new order to the database
                _context.Orders.Add(newOrder);

                // Clear the user's shopping cart
                _context.CartDetails.RemoveRange(shoppingCart.CartDetails);

                // Save changes to the database
                await _context.SaveChangesAsync(); // Ensure that changes are saved to the database

                // Redirect to the user's orders page or display a success message
                return RedirectToAction("Index", "MyOrder");
            }
            catch (Exception ex)
            {
                // Handle exceptions, e.g., log the error and return an error view
                return View("Error");
            }
        }



    }
}
