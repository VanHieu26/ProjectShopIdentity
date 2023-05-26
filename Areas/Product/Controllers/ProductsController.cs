using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ProjectShopIdentity.Areas.Product.Models;
using ProjectShopIdentity.Areas.Saler.Data;
using ProjectShopIdentity.BussinessLogic.Interface;
using ProjectShopIdentity.Controllers;
using ProjectShopIdentity.Data;
using ProjectShopIdentity.Models;
using ProjectShopIdentity.Reponsitory;
using ProjectShopIdentity.Service;

namespace ProjectShopIdentity.Areas.Product.Controllers
{
    [Area("Product")]
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Product/Products
        public async Task<IActionResult> Index()
        {
            //var product = await _context.Products.Include(pc => pc.Product_Catogories).ThenInclude(p => p.Product).ToListAsync();
              return _context.Products != null ? 
                          View(await _context.Products.OrderBy( p=> p.Name)
                                .Include(pc => pc.Product_Catogories)
                                .ThenInclude(p => p.Category)
                                .ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Products'  is null.");
        }

        // GET: Product/Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var products = await _context.Products
                .FirstOrDefaultAsync(m => m.Id == id);
            


            if (products == null)
            {
                return NotFound();
            }
            var shop = await _context.Shops.FirstOrDefaultAsync(s => s.Shop_Products.Any(sp => sp.ProductID == id));
            var productLq = await _context.Products.Where(p => p.Name.ToLower().Equals(products.Name.ToLower())).Take(4).ToListAsync();
            if(productLq.Count > 0)
            {
                ViewBag.productLq = productLq;
            }
            else
            {
                ViewBag.productLq = null;
            }
            if (shop != null)
            {
                ViewBag.Shop = shop;
            }
            else
            {
                ViewBag.Shop = null; // or handle it however you want
            }
           
            return View(products);
        }
        
        // GET: Product/Products/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Product/Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Price,Product_Description,Product_Quantity")] Products products, IFormFile fileimg)
        {
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
                return RedirectToAction(nameof(Index));
            }
            return View(products);
        }

        // GET: Product/Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var products = await _context.Products.FindAsync(id);
            if (products == null)
            {
                return NotFound();
            }
            return View(products);
        }

        // POST: Product/Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Price,Product_Description,Product_Image,Product_Quantity")] Products products)
        {
            if (id != products.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(products);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductsExists(products.Id))
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
            return View(products);
        }

        // GET: Product/Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var products = await _context.Products
                .FirstOrDefaultAsync(m => m.Id == id);
            if (products == null)
            {
                return NotFound();
            }

            return View(products);
        }

        // POST: Product/Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Products == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Products'  is null.");
            }
            var products = await _context.Products.FindAsync(id);
            if (products != null)
            {
                _context.Products.Remove(products);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductsExists(int id)
        {
          return (_context.Products?.Any(e => e.Id == id)).GetValueOrDefault();
        }





        public IActionResult AddProduct()
        {
            
            ViewBag.Cate = _context.Categories.ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddProduct([Bind("Name,Price,Product_Description,Product_Image,Product_Quantity")] Products product, int categoryId, IFormFile fileimg)
        {
            if (!ModelState.IsValid)
            {
                if (fileimg != null && fileimg.Length > 0)
                {
                    product.Product_Image = FileUpload.UploadFile(product.Id.ToString() + product.Name, fileimg);
                }
                else
                {
                    product.Product_Image = "error.png";
                }
                _context.Add(product);
                await _context.SaveChangesAsync();
    
                var productCategory = new Product_Catogory
                {
                    ProductID = product.Id,
                    CategoryID = categoryId
                };

                _context.Add(productCategory);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            ViewBag.CategoryId = new SelectList(_context.Categories, "Id", "Name");
            return View(product);
        }

        public async Task<IActionResult> ProductInCategory(int? id)
        {
            var category = await _context.Products.Where(p => p.Product_Catogories.Any(c => c.CategoryID == id)).Include(c => c.Product_Catogories).ThenInclude(a => a.Category).ToListAsync();
            ViewBag.Cat = category.First().Product_Catogories.First().Category.Name;
            return View(category);
        }

        public async Task<IActionResult> SearchProduct(string? productname)
        {
            if(productname == null)
            {
                return View(await _context.Products.ToListAsync());   
            }
            var item = await _context.Products.Where(p => p.Name.ToLower().Contains(productname.ToLower())).ToListAsync();
            return  View(item);
        }




        





        public async Task<IActionResult> AllProduct()
        {
            return _context.Products != null ? View(await _context.Products.ToListAsync()) : NotFound("Khong co san pham nao");
        }

    }
}
