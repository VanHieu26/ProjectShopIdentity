using ProjectShopIdentity.Areas.Product.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectShopIdentity.Areas.Admin.Models
{
    public class Order_Item
    {
        [Key]
        [Column("Product_ID", Order =  0)]
        public int ProductID { get; set; }

        [Key]
        [Column ("Order_ID", Order = 1)]
        public int OrderID { get; set; } 
        
        public int Quantity { get; set; }

        public double PriceAtPurchase { get; set; }

        public virtual Products? Product { get; set; }
        public virtual Order? Order { get; set; }
    }
}
