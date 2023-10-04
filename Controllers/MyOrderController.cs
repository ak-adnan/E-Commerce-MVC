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
        public async Task<IActionResult> PlaceOrders([FromBody] Order order)
        {
            
            
                try
                {
                    // Save the order to the database
                    order.OrderDate = DateTime.Now;
                    _context.Orders.Add(order);
                    await _context.SaveChangesAsync();

                    // Clear the user's cart (you might have a similar method in your ShoppingCartController)
                    // ClearUserCart(order.UserId);

                    return Ok(order.Id); // Return the order ID or a success response
                }
                catch (Exception ex)
                {
                    return StatusCode(500, $"Internal server error: {ex.Message}");
                }
         
            return BadRequest();
        }


    }
}
