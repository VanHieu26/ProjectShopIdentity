using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ProjectShopIdentity.Data;

namespace ProjectShopIdentity.Areas.Identity.Pages.Role
{
    public class IndexModel : RolePageModel
    {
        public IndexModel(RoleManager<IdentityRole> roleManager, ApplicationDbContext applicationDbContext) : base(roleManager, applicationDbContext)
        {
        }
        public class RoleMode : IdentityRole
        {
            public string[] Claims { get; set; }
        }
        public List<RoleMode> Roles { get; set; }




        public async Task OnGet()
        {
            // await _roleManager.GetClaimsAsync();
            var r = await _roleManager.Roles.OrderByDescending(r => r.Name).ToListAsync();
            Roles = new List<RoleMode>();
            foreach (var r2 in r)
            {
                var claims = await _roleManager.GetClaimsAsync(r2);
                var claimstring = claims.Select(c => c.Type + "=" + c.Value);
                var rm = new RoleMode()
                {
                    Name = r2.Name,
                    Id = r2.Id,
                    Claims = claimstring.ToArray(),
                };
                Roles.Add(rm);
            }
        }

        public void OnPost() => RedirectToPage();


    }
}
