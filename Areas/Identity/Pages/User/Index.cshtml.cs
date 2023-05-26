using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ProjectShopIdentity.Data;

namespace ProjectShopIdentity.Areas.Identity.Pages.User
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<AppUser> _userManager;
        public IndexModel(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        [TempData]
        public string StatusMessage { get; set; }

        public class UserAndRole : AppUser
        {
            public string RoleName { get; set; }
        }

        public List<UserAndRole> user { get; set; }


        public const int ITEMS_PER_PAGE = 10;

        [BindProperty(SupportsGet = true, Name = "p")]
        public int currentPage { get; set; }

        public int countPages { get; set; }

        public int totalUser { get; set; }



        public async Task OnGet()
        {
            //user = await _userManager.Users.OrderBy(x => x.UserName).ToListAsync(); 
            var qr = _userManager.Users.OrderBy(x => x.UserName);
            totalUser = await qr.CountAsync();
            countPages = (int)Math.Ceiling((double)totalUser / ITEMS_PER_PAGE);

            if (currentPage < 1)
                currentPage = 1;
            if (currentPage > countPages)
                currentPage = countPages;
            var qr1 = qr.Skip((currentPage - 1) * ITEMS_PER_PAGE).Take(ITEMS_PER_PAGE).Select(
                    u => new UserAndRole()
                    {
                        Id = u.Id,
                        UserName = u.UserName,
                    }
                );

            user = await qr1.ToListAsync();
            foreach (var us in user)
            {
                var roles = await _userManager.GetRolesAsync(us);
                us.RoleName = string.Join(",", roles);
            }
        }

        public void OnPost() => RedirectToPage();

    }
}
