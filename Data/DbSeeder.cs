using Microsoft.AspNetCore.Identity;
using ProjectShopIdentity.Constants;

namespace ProjectShopIdentity.Data
{
    public static class DbSeeder
    {
        // Seed Roles

        public static async Task SeedRolesAndAdminAsync(IServiceProvider service)
        {
            //Seed Roles
            var userManager = service.GetService<UserManager<AppUser>>();
            var roleManager = service.GetService<RoleManager<IdentityRole>>();
            await roleManager.CreateAsync(new IdentityRole(Roles.Admin.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.User.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.Saler.ToString()));


            //creating admin
            var user = new AppUser
            {
                UserName = "admin1@gmail.com",
                Email = "admin1@gmail.com",
                Address = "None",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,


            };
            var userInDb = await userManager.FindByEmailAsync(user.Email);
            if (userInDb == null)
            {
                await userManager.CreateAsync(user, "Admin@123");
                await userManager.AddToRoleAsync(user, Roles.Admin.ToString());
            }


        }
    }
}
