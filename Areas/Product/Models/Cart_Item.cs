

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ProjectShopIdentity.Areas.Product.Models
{
    public class Cart_Item
    {
        [Key]
        [Column("Product_ID", Order = 0)]
        public int ProductID { get; set; }
        [Column("Cart_ID", Order = 1)]
        public int CartID { get; set; }

        public int Quantity { get; set; }
        public virtual Cart? Cart { get; set; } // Updated property name
        public virtual Products? Product { get; set; }
    }
}
