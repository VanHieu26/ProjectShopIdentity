using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProjectShopIdentity.Data;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace ProjectShopIdentity.Areas.Identity.Pages.User
{
    public class EditUserRoleClaimModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;


        public EditUserRoleClaimModel(ApplicationDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
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


        [TempData]
        public string StatusMessage { get; set; }


        public AppUser user { get; set; }


        public NotFoundObjectResult OnGet() => NotFound("Khong duoc truy cap");

        public async Task<IActionResult> OnGetAddClaimAsync(string userID)
        {
            user = await _userManager.FindByIdAsync(userID);
            if (user == null)
            {
                return NotFound("Khong tim tahy user");
            }
            return Page();
        }
        public async Task<IActionResult> OnPostAddClaimAsync(string userID)
        {
            user = await _userManager.FindByIdAsync(userID);
            if (user == null)
            {
                return NotFound("Khong tim tahy user");
            }
            if (!ModelState.IsValid) return Page();

            var claims = _context.UserClaims.Where(c => c.UserId == user.Id);

            if (claims.Any(c => c.ClaimType == inputModel.ClaimType && c.ClaimValue == inputModel.ClaimValue))
            {
                ModelState.AddModelError(string.Empty, "Dac tinh nay da co");
                return Page();
            }

            await _userManager.AddClaimAsync(user, new Claim(inputModel.ClaimType, inputModel.ClaimValue));
            StatusMessage = $"Da them dac tinh cho {user.UserName}";

            return RedirectToPage("./AddRole", new { id = userID });
        }


        public IdentityUserClaim<string> userclaim { get; set; }


        public async Task<IActionResult> OnGetEditClaimAsync(int? claimID)
        {
            if (claimID == null)
            {
                return NotFound("Khong tim thay claim");
            }

            userclaim = _context.UserClaims.Where(c => c.Id == claimID).FirstOrDefault();

            user = await _userManager.FindByIdAsync(userclaim.UserId);
            if (user == null)
            {
                return NotFound("Khong tim tahy user");
            }

            inputModel = new InputModel()
            {
                ClaimType = userclaim.ClaimType,
                ClaimValue = userclaim.ClaimValue,
            };

            return Page();
        }

    }
}
