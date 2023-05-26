using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProjectShopIdentity.Areas.Admin.Models;
using ProjectShopIdentity.Areas.Product.Models;
using ProjectShopIdentity.Areas.Saler.Data;
using System.Reflection.Emit;

namespace ProjectShopIdentity.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }


        public DbSet<Category> Categories { get; set; }
        public DbSet<Shop> Shops { get; set; }   
        public DbSet<Products> Products { get; set; }
        public DbSet<Product_Catogory> Product_Catogories { get; set; }
        public DbSet<Shop_Product> shop_Products { get; set; }
        public DbSet<Order_Status> order_Statuses { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Order_Item> Items { get; set; }
        public DbSet<Cart> Carts { get; set; }  
        public DbSet<Cart_Item> cart_Items { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Category>(entity =>
            {
                entity.ToTable("CATEGORIES");
                entity.Property(e  => e.Id).HasColumnName("Category_ID");
                entity.Property(e => e.Picture).HasColumnName("Category_Img");
            });

            builder.Entity<Shop>(entity =>
            {
                entity.ToTable("Shop");
                entity.Property(s => s.Id).HasColumnName("ShopID");
                entity.HasOne(e => e.AppUser).WithMany(s => s.Shops).HasForeignKey(e => e.UserID);
            });

            builder.Entity<Products>(entity =>
            {
                entity.ToTable("PRODUCTS");
                entity.Property(p => p.Id).HasColumnName("Product_ID");       
            });

            builder.Entity<Product_Catogory>(entity =>
            {
                entity.ToTable("PRODUCT_CATEGORIES");
                entity.HasKey(pc => new { pc.ProductID, pc.CategoryID });
                entity.HasOne(p => p.Product).WithMany(pc => pc.Product_Catogories).HasForeignKey(pc => pc.ProductID);
                entity.HasOne(c => c.Category).WithMany(pc => pc.product_Catogories).HasForeignKey(pc => pc.CategoryID);
            });

            builder.Entity<Shop_Product>(entity =>
            {
                entity.ToTable("STORE_PRODUCT");
                entity.HasKey(sp => new {sp.ProductID, sp.ShopID});
                entity.HasOne(p => p.Shops).WithMany(sp => sp.Shop_Products).HasForeignKey(sp => sp.ShopID);
                entity.HasOne(p => p.Products).WithMany(sp => sp.Shop_Products).HasForeignKey(sp => sp.ProductID);
            });

            builder.Entity<Order_Status>(entity =>
            {
                entity.ToTable("ORDER_STATUS");
                entity.Property(os => os.OrderSID).HasColumnName("Order_Status_ID");
                entity.Property(os => os.OrderSName).HasColumnName("Order_Status_Name");
            });
            builder.Entity<Order>(entity =>
            {
                entity.ToTable("ORDERS");
                entity.Property(o => o.OrderID).HasColumnName("Order_ID");
                entity.Property(o => o.Order_Date).HasColumnName("Order_Date");
                entity.HasOne(u => u.User).WithMany(o => o.Orders);
                entity.HasOne(s => s.Shop).WithMany(o => o.Orders);
                entity.HasOne(os => os.Status).WithMany(o => o.Orders);
            });
            builder.Entity<Order_Item>(entity =>
            {
                entity.ToTable("ORDER_ITEMS");
                entity.HasKey(o => new {o.ProductID, o.OrderID});
                entity.HasOne(p => p.Product).WithMany(oi => oi.Order_Items).HasForeignKey(o => o.ProductID);
                entity.HasOne(o => o.Order).WithMany(oi => oi.Order_Items).HasForeignKey(o => o.OrderID);
            });

            builder.Entity<Cart>(entity =>
            {
                entity.ToTable("CARTS");
                entity.Property(c => c.CartId).HasColumnName("Cart_ID");
               
                entity.HasOne(u => u.AppUser).WithOne(c => c.Cart).HasForeignKey<Cart>(c => c.UserId);
            });

            builder.Entity<Cart_Item>(entity =>
            {
                entity.ToTable("Cart_Items");
                entity.HasKey(e => new { e.ProductID, e.CartID });
                entity.Property(e => e.ProductID).HasColumnName("Product_ID");
                entity.Property(e => e.CartID).HasColumnName("Cart_ID");
                entity.HasOne(p => p.Product).WithMany(ci => ci.Cart_Items).HasForeignKey(e => e.ProductID);
                entity.HasOne(p => p.Cart).WithMany(ci => ci.Cart_Items).HasForeignKey(ci => ci.CartID);
            });

            builder.Entity<IdentityUserLogin<string>>()
            .HasKey(login => new { login.LoginProvider, login.ProviderKey });
            builder.Entity<IdentityUserRole<string>>().HasKey(role => new { role.RoleId, role.UserId });
            builder.Entity<IdentityUserToken<string>>().HasKey(token => new { token.UserId });
        }
    }
}


//update-database