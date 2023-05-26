using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectShopIdentity.Data;
using ProjectShopIdentity.Areas.Product.Models;
namespace ProjectShopIdentity.Areas.Product.Controllers
{
    [Area("Product")]
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        public CartController(ApplicationDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
    
        public IActionResult Index()
        {
            return View();
        }


        [Authorize]
        [HttpGet]
        public async Task<IActionResult> CartDetails()
        {
            if( _context.Carts == null)
            {
                return NotFound();
            }
            var userID = await _userManager.GetUserAsync(HttpContext.User);
           
            var cart = await _context.Carts.Include(c => c.Cart_Items).ThenInclude(p => p.Product).FirstOrDefaultAsync(c=> c.UserId == userID.Id);
            if(cart == null) 
            {
                return NotFound();
            
            }
            ViewBag.CartId = cart.CartId;
            return View(cart);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteProductInCart(int? id)
        {
            if(id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product =await _context.Products.FirstOrDefaultAsync(p => p.Id == id);

            var user =await _userManager.GetUserAsync(HttpContext.User);
            if(user == null)
            {
                return NotFound();
            }
            var carts = await _context.Carts.FirstOrDefaultAsync(c => c.UserId == user.Id);
            var cartItems = await _context.cart_Items.FirstOrDefaultAsync(ct => ct.CartID == carts.CartId && ct.ProductID == product.Id);
            if(cartItems == null)
            {
                return NotFound("Khoafg");
            }
            _context.cart_Items.Remove(cartItems);
            await _context.SaveChangesAsync();
            
            return RedirectToAction(nameof(CartDetails));
            
        }


        [HttpGet]
        [Authorize]
        public async Task<IActionResult> AddToCart(int? productID)
        {
            if(productID == null || _context.Products == null){
                return NotFound();
            }
            var prod = await _context.Products.FirstOrDefaultAsync(p => p.Id == productID);
            if(prod == null)
            {
                return NotFound();
            }
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var cart = await _context.Carts.FirstOrDefaultAsync(c => c.UserId == user.Id);

            var productInCart = await _context.cart_Items.FirstOrDefaultAsync(c => c.ProductID == productID);
            if(productInCart == null)
            {
                productInCart = new Cart_Item()
                {
                    ProductID = prod.Id,
                    CartID = cart.CartId,
                    Quantity = 1
                };
                _context.Add(productInCart);
            }
            else
            {
                productInCart.Quantity += 1;
            }
            await _context.SaveChangesAsync();
            return RedirectToAction("CartDetails");
        }

        [HttpGet]
        public async Task<IActionResult> OrderIF(int? id)
        {
            if(id == null && _context.Products == null)
            {
                return NotFound();
            }
            //var user = await _userManager.GetUserAsync(HttpContext.User);
            //if(user.PhoneNumber == null && user.Address == "Noen")
            //{
            //    RedirectToPage("/Identity/Account/Manage/AddAddress");
            //}
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
            if(product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        public async Task<IActionResult> AddOrderDetails(int? id)
        {
            if(id == null || _context.Products == null) 
            {
                return NotFound();
            }
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
            if(product == null)
            {
                return NotFound();
            }
            return View(product);
        }
        


    }
}
