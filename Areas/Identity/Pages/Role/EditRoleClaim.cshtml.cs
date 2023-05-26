using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ProjectShopIdentity.Data;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace ProjectShopIdentity.Areas.Identity.Pages.Role
{
    public class EditRoleClaimModel : RolePageModel
    {
        public EditRoleClaimModel(RoleManager<IdentityRole> roleManager, ApplicationDbContext applicationDbContext) : base(roleManager, applicationDbContext)
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

        public IdentityRoleClaim<string> Claim { get; set; }

        public async Task<IActionResult> OnGet(int? claimID)
        {
            if (claimID == null)
            {
                return NotFound("Khong co claimID Nay");
            }
            Claim = await _context.RoleClaims.Where(c => c.Id == claimID).FirstOrDefaultAsync();

            if (Claim == null) return NotFound("Khong tim thay claim");

            roles = await _roleManager.FindByIdAsync(Claim.RoleId);
            if (roles == null)
            {
                return NotFound($"Don't Fin Role With ID : {Claim.RoleId}");
            }


            inputModel = new InputModel()
            {
                ClaimType = Claim.ClaimType,
                ClaimValue = Claim.ClaimValue
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? claimID)
        {
            if (claimID == null)
            {
                return NotFound("Khong co claimID Nay");
            }
            Claim = await _context.RoleClaims.Where(c => c.Id == claimID).FirstOrDefaultAsync();
            roles = await _roleManager.FindByIdAsync(Claim.RoleId);

            if (roles == null)
            {
                return NotFound($"Don't Fin Role With ID : {Claim.RoleId}");
            }
            if (!ModelState.IsValid)
            {
                return Page();
            }
            if (_context.RoleClaims.Any(c => c.RoleId == roles.Id && c.ClaimType == inputModel.ClaimType && c.ClaimValue == inputModel.ClaimValue && c.Id == claimID))
            {
                ModelState.AddModelError(string.Empty, "Claim nay da co trong role");
                return Page();
            }

            Claim.ClaimType = inputModel.ClaimType;
            Claim.ClaimValue = inputModel.ClaimValue;
            await _context.SaveChangesAsync();

            StatusMessage = "Vua cap nhat dac tinh moie {claims}";
            return RedirectToPage("./Edit", new { roleID = roles.Id });


        }

        public async Task<IActionResult> OnPostDeleteAsync(int claimID)
        {
            if (claimID == null)
            {
                return NotFound("Khong co claimID Nay");
            }
            Claim = await _context.RoleClaims.Where(c => c.Id == claimID).FirstOrDefaultAsync();
            roles = await _roleManager.FindByIdAsync(Claim.RoleId);

            if (roles == null)
            {
                return NotFound($"Don't Fin Role With ID : {Claim.RoleId}");
            }


            await _roleManager.RemoveClaimAsync(roles, new Claim(Claim.ClaimType, Claim.ClaimValue));



            StatusMessage = "Vua Xoa dac tinh moie {claims}";
            return RedirectToPage("./Edit", new { roleID = roles.Id });


        }
    }
}
