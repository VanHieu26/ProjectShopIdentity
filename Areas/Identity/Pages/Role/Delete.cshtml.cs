using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProjectShopIdentity.Data;

namespace ProjectShopIdentity.Areas.Identity.Pages.Role
{
    public class DeleteModel : RolePageModel
    {
        public DeleteModel(RoleManager<IdentityRole> roleManager, ApplicationDbContext applicationDbContext) : base(roleManager, applicationDbContext)
        {
        }
        public IdentityRole role { get; set; }


        public async Task<IActionResult> OnGet(string roleID)
        {
            if (roleID == null)
            {
                return NotFound("Khong tim thay role");
            }
            role = await _roleManager.FindByIdAsync(roleID);
            if (role == null) { return NotFound($"Khong co Role voi id : {roleID}"); }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string roleId)
        {
            if (roleId == null) return NotFound("Khong tim thay role");
            role = await _roleManager.FindByIdAsync(roleId);
            if (role == null) return NotFound("Khong co role nhu tren");

            if (!ModelState.IsValid)
            {
                return Page();
            }


            var result = await _roleManager.DeleteAsync(role);
            if (result.Succeeded)
            {
                StatusMessage = $"Ban vua xoa {role.Name}";
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
