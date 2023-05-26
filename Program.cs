using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProjectShopIdentity.Data;
using ProjectShopIdentity.Reponsitory;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

//builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
//   .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

//configure identity
builder.Services.AddIdentity<AppUser, IdentityRole>().
    AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultUI().AddDefaultTokenProviders();
//configure identity login
builder.Services.Configure<IdentityOptions>(options =>
{
    //Password
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 3;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredUniqueChars = 1;


    //Lokout user
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    // CofirmAlterLogin
    options.SignIn.RequireConfirmedPhoneNumber = false;
    options.SignIn.RequireConfirmedEmail = false;
    options.SignIn.RequireConfirmedAccount = false;
});

//Menu component
builder.Services.AddScoped<ICategory, CategoryReponsetory>();
builder.Services.AddScoped<IProduct, ProductReponsitory>();

builder.Services.AddMvcCore();

// Authentication
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Identity/Account/Login";
    options.LogoutPath = "/Identity/Account/Logout";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
});




var app = builder.Build();




// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}





app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

//app.UseAuthentication();









app.MapAreaControllerRoute(
    name: "default",
    areaName: "Admin",
    pattern: "Admin/{controller=Home}/{action=Index}/{id?}"
    );
app.MapAreaControllerRoute(
    name: "default",
    areaName: "Product",
    pattern: "Product/{controller=Home}/{action=Index}/{id?}"
    );
app.MapAreaControllerRoute(
    name: "default",
    areaName: "Saler",
    pattern: "Saler/{controller=Home}/{action=Index}/{id?}"
    );
app.MapAreaControllerRoute(
    name: "default",
    areaName: "Identity",
    pattern: "Identity/{controller=Home}/{action=Index}/{id?}"
    );
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
    );



app.MapRazorPages();
using (var scope = app.Services.CreateScope())
//{
//    var serviceProvider = scope.ServiceProvider;
//    var dbContext = serviceProvider.GetRequiredService<ApplicationDbContext>();
//    DbSeeder.SeedRolesAndAdminAsync(serviceProvider).Wait(); // Đợi hoàn thành đồng bộ
//}
app.Run();
