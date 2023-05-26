using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjectShopIdentity.Data;
using System.ComponentModel;

namespace ProjectShopIdentity.Areas.Identity.Pages.User
{
    public class AddRoleModel : PageModel
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;

        public SelectList allRole { get; set; }
        public List<IdentityRoleClaim<string>> claimInRole { get; set; }
        public List<IdentityUserClaim<string>> claimInUserCl { get; set; }
        public AddRoleModel(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _context = context;
        }

        [BindProperty]
        [DisplayName("Cac role gan cho user")]
        public string[] RoleName { get; set; }


        [TempData]
        public string StatusMessage { get; set; }


        public AppUser user { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound($"Khong tim thay user co id: {id}");
            }
            user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound($"khong thay user voi id '{id}'.");
            }

            RoleName = (await _userManager.GetRolesAsync(user)).ToArray<string>();
            List<string> roleNames = await _roleManager.Roles.Select(R => R.Name).ToListAsync
                ();

            var listRole = from role in _context.Roles
                           join ur in _context.UserRoles on role.Id equals ur.RoleId
                           where ur.UserId == id
                           select role;

            var _claimInRole = from c in _context.RoleClaims
                               join r in listRole on c.RoleId equals r.Id
                               select c;

            claimInRole = await _claimInRole.ToListAsync();
            allRole = new SelectList(roleNames);
            await GetClaims(id);

            return Page();
        }
        async Task GetClaims(string id)
        {

            var listRole = from role in _context.Roles
                           join ur in _context.UserRoles on role.Id equals ur.RoleId
                           where ur.UserId == id
                           select role;

            var _claimInRole = from c in _context.RoleClaims
                               join r in listRole on c.RoleId equals r.Id
                               select c;

            claimInRole = await _claimInRole.ToListAsync();

            claimInUserCl = await (from c in _context.UserClaims
                                   where c.UserId == id
                                   select c).ToListAsync();
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound($"Khong tim thay user co id: {id}");
            }

            user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound($"khong thay user voi id '{id}'.");
            }

            await GetClaims(id);

            // Role Name
            var oldRoleName = (await _userManager.GetRolesAsync(user)).ToArray<string>();
            var deleteRole = oldRoleName.Where(r => !RoleName.Contains(r));

            var addRole = RoleName.Where(r => !oldRoleName.Contains(r));

            List<string> roleNames = await _roleManager.Roles.Select(R => R.Name).ToListAsync
               ();

            allRole = new SelectList(roleNames);

            var resultDelete = await _userManager.RemoveFromRolesAsync(user, deleteRole);
            if (!resultDelete.Succeeded)
            {
                resultDelete.Errors.ToList().ForEach(error =>
                {
                    ModelState.AddModelError(string.Empty, error.Description);

                });
                return Page();
            }

            var resultAdd = await _userManager.AddToRolesAsync(user, addRole);
            if (!resultAdd.Succeeded)
            {
                resultDelete.Errors.ToList().ForEach(error =>
                {
                    ModelState.AddModelError(string.Empty, error.Description);

                });
                return Page();
            }




            StatusMessage = $"Ban vua cap nhat role cho user {user.UserName}";

            return RedirectToPage("./Index");
        }
    }
}
