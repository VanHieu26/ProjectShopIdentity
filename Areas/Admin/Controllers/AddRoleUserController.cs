using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectShopIdentity.Constants;
using ProjectShopIdentity.Data;

namespace ProjectShopIdentity.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AddRoleUserController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ApplicationDbContext _context;
        public AddRoleUserController(ApplicationDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            var saler = await _userManager.GetUsersInRoleAsync(Roles.RegisterSaler.ToString());
            return View(saler);
        }
    }
}
