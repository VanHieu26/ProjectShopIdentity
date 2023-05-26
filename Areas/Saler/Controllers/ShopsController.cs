using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjectShopIdentity.Areas.Product.Models;
using ProjectShopIdentity.Areas.Saler.Data;
using ProjectShopIdentity.Data;
using ProjectShopIdentity.Service;

namespace ProjectShopIdentity.Areas.Saler.Controllers
{
    [Area("Saler")]
    //[Authorize(Roles = "Saler")]
    public class ShopsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ShopsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Saler/Shops
        public async Task<IActionResult> Index()
        {
            var claims = HttpContext.User;
            var userId = claims.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var applicationDbContext = _context.Shops.Where(s => s.UserID == userId).Include(a => a.AppUser);
            return View(await applicationDbContext.ToListAsync());
        }


        public async Task<IActionResult> ShopAll()
        {
            return _context.Shops != null ? View(await _context.Shops.Include(u => u.AppUser).Include(sp => sp.Shop_Products)
                
                .ThenInclude(p => p.Products).ToListAsync()) : NotFound("Khong co");
        }







        // GET: Saler/Shops/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Shops == null)
            {
                return NotFound();
            }

            var shop = await _context.Shops
                .Include(s => s.AppUser).Include(p => p.Shop_Products).ThenInclude(p => p.Products)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (shop == null)
            {
                return NotFound();
            }

            return View(shop);
        }
        

        // GET: Saler/Shops/Create
        public IActionResult Create()
        { 
            return View();
        }

        // POST: Saler/Shops/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Address,UserID")] Shop shop)
        {
            
            if (ModelState.IsValid)
            {
                var claims = HttpContext.User;
                var userId = claims.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                shop.UserID = userId;
                _context.Add(shop);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            
            return View(shop);
        }

        // GET: Saler/Shops/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Shops == null)
            {
                return NotFound();
            }

            var shop = await _context.Shops.FindAsync(id);
            if (shop == null)
            {
                return NotFound();
            }
            return View(shop);
        }

        // POST: Saler/Shops/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Address,UserID")] Shop shop)
        {
            if (id != shop.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var claims = HttpContext.User;
                    var userId = claims.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                    shop.UserID = userId;
                    _context.Update(shop);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ShopExists(shop.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }     
            return View(shop);
        }


        [HttpGet]
        public IActionResult ShopAddProduct()
        {
            ViewBag.Category = _context.Categories?.ToList();
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> ShopAddProduct(int? id, [Bind("Name,Price,Product_Description,Product_Image,Product_Quantity")] Products products, IFormFile fileimg, int categoryId)
        {
            if (id == null || _context.Shops == null)
            {
                return NotFound();
            }
            var shopitem = _context.Shops.FirstOrDefault(shop => shop.Id == id);
            if (shopitem == null)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                if (fileimg != null && fileimg.Length > 0)
                {
                    products.Product_Image = FileUpload.UploadFile(products.Id.ToString() + products.Name, fileimg);
                }
                else
                {
                    products.Product_Image = "error.png";
                }
                _context.Add(products);
                await _context.SaveChangesAsync();

                var shopproduct = new Shop_Product()
                {
                    ShopID = shopitem.Id,
                    ProductID = products.Id
                };
                var category = new Product_Catogory()
                {
                    ProductID = products.Id,
                    CategoryID = categoryId
                };
                _context.Add(category);
                _context.Add(shopproduct);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Details), new { id = id });
            }
            ViewBag.Category = _context.Categories?.ToList();
            return View(products);
        }


        // GET: Saler/Shops/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Shops == null)
            {
                return NotFound();
            }

            var shop = await _context.Shops
                .Include(s => s.AppUser)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (shop == null)
            {
                return NotFound();
            }

            return View(shop);
        }

        // POST: Saler/Shops/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Shops == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Shops'  is null.");
            }
            var shop = await _context.Shops.FindAsync(id);
            if (shop != null)
            {
                _context.Shops.Remove(shop);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ShopExists(int id)
        {
          return (_context.Shops?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
