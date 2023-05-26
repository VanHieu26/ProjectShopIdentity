using ProjectShopIdentity.Areas.Admin.Models;
using ProjectShopIdentity.Areas.Product.Models;
using ProjectShopIdentity.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectShopIdentity.Areas.Saler.Data
{
    public class Shop
    {
        [Key]
        [Column("ShopID")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Tên shop không được để trống")]
        [Column("ShopName")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "{0} phải lớn hơn {2} và nhỏ hơn {1} kí tự")]
        public string Name { get; set; }

        [StringLength(100, MinimumLength = 3, ErrorMessage = "{0} phải lớn hơn {2} và nhỏ hơn {1} kí tự")]
        public string Address { get; set; }

        public string? UserID { get; set; }
        

        public virtual ICollection<Shop_Product>? Shop_Products { get; set; }
        public virtual AppUser? AppUser { get; set; }
        public virtual ICollection<Order>? Orders { get; set; }
    }
}
