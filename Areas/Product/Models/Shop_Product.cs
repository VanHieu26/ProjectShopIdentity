using ProjectShopIdentity.Areas.Saler.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectShopIdentity.Areas.Product.Models
{
    public class Shop_Product
    {
        [Key]
        [Column("Product_ID", Order = 0)]
        public int ProductID { get; set; }

        [Key]
        [Column("ShopID", Order = 1)]
        public int ShopID { get; set; }
        

        public virtual Shop? Shops { get; set; }
        public virtual Products? Products { get; set; }
    }
}
