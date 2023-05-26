using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProjectShopIdentity.Data;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace ProjectShopIdentity.Areas.Identity.Pages.Role
{
    public class AddRoleClaimModel : RolePageModel
    {
        public AddRoleClaimModel(RoleManager<IdentityRole> roleManager, ApplicationDbContext applicationDbContext) : base(roleManager, applicationDbContext)
        {
        }

        public class InputModel
        {
            [Display(Name = "Claime Type")]
            [Required(ErrorMessage = "Type Claims khong duoc de trong")]
            [StringLength(256, MinimumLength = 3, ErrorMessage = "{0} phai dai tu {2} den {1} ki tu")]
            public string ClaimType { get; set; }


            [Display(Name = "Claime Value")]
            [Required(ErrorMessage = "Type Claims khong duoc de trong")]
            [StringLength(256, MinimumLength = 3, ErrorMessage = "{0} phai dai tu {2} den {1} ki tu")]
            public string ClaimValue { get; set; }
        }

        [BindProperty]
        public InputModel inputModel { get; set; }

        public IdentityRole roles { get; set; }

        public async Task<IActionResult> OnGet(string roleID)
        {
            roles = await _roleManager.FindByIdAsync(roleID);
            if (roles == null)
            {
                return NotFound($"Don't Fin Role With ID : {roleID}");
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string roleID)
        {
            roles = await _roleManager.FindByIdAsync(roleID);
            if (roles == null)
            {
                return NotFound($"Don't Fin Role With ID : {roleID}");
            }
            if (!ModelState.IsValid)
            {
                return Page();
            }
            if ((await _roleManager.GetClaimsAsync(roles)).Any(c => c.Type == inputModel.ClaimType && c.Value == inputModel.ClaimValue))
            {
                ModelState.AddModelError(string.Empty, "Claim nay da co trong role");
                return Page();
            }
            var newclaim = new Claim(inputModel.ClaimType, inputModel.ClaimValue);
            var resul = await _roleManager.AddClaimAsync(roles, newclaim);
            if (!resul.Succeeded)
            {
                resul.Errors.ToList().ForEach(error =>
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                });
                return Page();
            }
            StatusMessage = "Vua them dac tinh moie {claims}";
            return RedirectToPage("./Edit", new { roleID = roleID });


        }
    }
}
