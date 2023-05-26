using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjectShopIdentity.Areas.Admin.Models;
using ProjectShopIdentity.Data;

namespace ProjectShopIdentity.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrderStatusController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrderStatusController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/OrderStatus
        public async Task<IActionResult> Index()
        {
              return _context.order_Statuses != null ? 
                          View(await _context.order_Statuses.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.order_Statuses'  is null.");
        }

        // GET: Admin/OrderStatus/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.order_Statuses == null)
            {
                return NotFound();
            }

            var order_Status = await _context.order_Statuses
                .FirstOrDefaultAsync(m => m.OrderSID == id);
            if (order_Status == null)
            {
                return NotFound();
            }

            return View(order_Status);
        }

        // GET: Admin/OrderStatus/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/OrderStatus/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrderSID,OrderSName")] Order_Status order_Status)
        {
            if (ModelState.IsValid)
            {
                _context.Add(order_Status);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(order_Status);
        }

        // GET: Admin/OrderStatus/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.order_Statuses == null)
            {
                return NotFound();
            }

            var order_Status = await _context.order_Statuses.FindAsync(id);
            if (order_Status == null)
            {
                return NotFound();
            }
            return View(order_Status);
        }

        // POST: Admin/OrderStatus/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OrderSID,OrderSName")] Order_Status order_Status)
        {
            if (id != order_Status.OrderSID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order_Status);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!Order_StatusExists(order_Status.OrderSID))
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
            return View(order_Status);
        }

        // GET: Admin/OrderStatus/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.order_Statuses == null)
            {
                return NotFound();
            }

            var order_Status = await _context.order_Statuses
                .FirstOrDefaultAsync(m => m.OrderSID == id);
            if (order_Status == null)
            {
                return NotFound();
            }

            return View(order_Status);
        }

        // POST: Admin/OrderStatus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.order_Statuses == null)
            {
                return Problem("Entity set 'ApplicationDbContext.order_Statuses'  is null.");
            }
            var order_Status = await _context.order_Statuses.FindAsync(id);
            if (order_Status != null)
            {
                _context.order_Statuses.Remove(order_Status);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool Order_StatusExists(int id)
        {
          return (_context.order_Statuses?.Any(e => e.OrderSID == id)).GetValueOrDefault();
        }
    }
}
