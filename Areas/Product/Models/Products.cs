using ProjectShopIdentity.Areas.Admin.Models;
using ProjectShopIdentity.Areas.Saler.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectShopIdentity.Areas.Product.Models
{
    public class Products
    {
        [Key]
        [Column("Product_ID")]
        public int Id { get; set; }

        [Column("Product_Name")]
        [Display(Name = "Tên sản phẩm")]
        [Required(ErrorMessage = "Tên của sản phẩm không được để trống")]
        [StringLength(60, MinimumLength = 6, ErrorMessage = "Độ dài của {0} phải lớn hơn {2} và nhỏ hơn {1} kí tự")]
        public string Name { get; set; }

        [Column("Product_Price")]
        [Required(ErrorMessage = "Vui lòng nhập gía cho sản phẩm!")]
        public double Price { get; set; }

        public string? Product_Description { get; set; }

        
        [Display(Name = "Ảnh sản phẩm")]
        [Required(ErrorMessage = "Ảnh sản phẩm không được để trống")]
        public string Product_Image { get; set; } 

        [Range(0, 1000, ErrorMessage = "{0} số lượng sản phẩm không được nhỏ hơn {1} hoặc lớn hơn {2}")]
        public int Product_Quantity { get; set; }

        public virtual ICollection<Shop_Product>? Shop_Products { get; set; }
        public virtual ICollection<Product_Catogory>? Product_Catogories { get; set; }
        public virtual ICollection<Order_Item>? Order_Items { get; set; }
        public virtual ICollection<Cart_Item>? Cart_Items { get; set; } 
        
    } 
}
