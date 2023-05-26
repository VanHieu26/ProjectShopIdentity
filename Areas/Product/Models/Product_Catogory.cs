using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectShopIdentity.Areas.Product.Models
{
    public class Product_Catogory
    {
        [Key]
        [Column("Product_ID", Order = 0)]
        public int ProductID { get; set; }
        [Column("Category_ID", Order = 1)]
        public int CategoryID { get; set; }
        public virtual Category? Category { get; set; }
        public virtual Products? Product { get; set; }
    }
}
