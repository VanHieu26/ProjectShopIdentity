using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using ProjectShopIdentity.Constants;
using ProjectShopIdentity.Data;
using ProjectShopIdentity.Models;
using System.Security.Claims;

namespace ProjectShopIdentity.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        public ShoppingCartController(ApplicationDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }


        [HttpGet]
        [Authorize]
        public async Task<IActionResult> RegisterSaler()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var isRole = await _userManager.GetRolesAsync(user);
            
            if (isRole.Contains(Roles.RegisterSaler.ToString()))
            {
                return RedirectToAction("Wait");
            }else if (isRole.Contains("Saler"))
            {
                return RedirectToAction("Index", "Shops", new { area = "Saler" });
            }
            else
            {
                return View();
            }

            
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> RegisterSaler(RegisterAsSalerViewModel model)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            if (user == null)
            {
                return NotFound();
            }

            model.RoleName = Roles.RegisterSaler.ToString();
            var isInRole = await _userManager.IsInRoleAsync(user, Roles.RegisterSaler.ToString());
            
            if (isInRole)
            {
                return BadRequest();
            }

            await _userManager.AddToRoleAsync(user, Roles.RegisterSaler.ToString());
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Wait()
        {
            return View();
        }


        public IActionResult AddToCart(int? id)
        {

            return View();
        }
        



    }
}
