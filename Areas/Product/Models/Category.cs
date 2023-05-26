using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectShopIdentity.Areas.Product.Models
{
    public class Category
    {
        [Key]
        [Column("Category_ID")]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Tên loại sản phẩm")]
        [StringLength(60, MinimumLength = 3, ErrorMessage = "Tên của {0} không được nhỏ hơn {2} và lớn hơn {1} kí tự")]
        [Column("Category_Name")]
        public string Name { get; set; }


       
        [Display(Name = "Ảnh loại sản phẩm")]
        [Column("Category_Img")]
        public string Picture { get; set; }

        [NotMapped]
        public int ProductInCategory { get; set; }
        public ICollection<Product_Catogory>? product_Catogories { get; set; }
    }
}
