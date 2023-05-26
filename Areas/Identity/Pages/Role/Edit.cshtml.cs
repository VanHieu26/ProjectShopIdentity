using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ProjectShopIdentity.Data;
using System.ComponentModel.DataAnnotations;

namespace ProjectShopIdentity.Areas.Identity.Pages.Role
{
    public class EditModel : RolePageModel
    {
        public EditModel(RoleManager<IdentityRole> roleManager, ApplicationDbContext applicationDbContext) : base(roleManager, applicationDbContext)
        {
        }
        public IdentityRole role { get; set; }

        public class InputModel
        {
            [Display(Name = "Role Name")]
            [Required(ErrorMessage = "Ten role khong duoc de trong")]
            [StringLength(256, MinimumLength = 3, ErrorMessage = "{0} phai dai tu {2} den {1} ki tu")]
            public string Name { get; set; }
        }
        [BindProperty]
        public InputModel inputModel { get; set; }

        public List<IdentityRoleClaim<string>> Claims { get; set; }

        public async Task<IActionResult> OnGet(string roleID)
        {
            if (roleID == null)
            {
                return NotFound("Khong tim thay role");
            }
            role = await _roleManager.FindByIdAsync(roleID);
            if (role != null)
            {
                inputModel = new InputModel()
                {
                    Name = role.Name
                };
                Claims = await _context.RoleClaims.Where(rc => rc.RoleId == role.Id).ToListAsync();
                return Page();
            }
            return NotFound("Khong tim thay role");
        }

        public async Task<IActionResult> OnPostAsync(string roleID)
        {
            if (roleID == null) return NotFound("Khong tim thay role");
            role = await _roleManager.FindByIdAsync(roleID);
            if (role == null) return NotFound("Khong co role nhu tren");
            Claims = await _context.RoleClaims.Where(rc => rc.RoleId == role.Id).ToListAsync();
            if (!ModelState.IsValid)
            {
                return Page();
            }

            role.Name = inputModel.Name;
            var result = await _roleManager.UpdateAsync(role);
            if (result.Succeeded)
            {
                StatusMessage = $"Ban vua chinh sua {inputModel.Name}";
                return RedirectToPage("./Index");
            }
            else
            {
                result.Errors.ToList().ForEach(error =>
                {
                    ModelState.AddModelError(string.Empty, error.Description);

                });
            }
            return Page();


        }
    }
}
