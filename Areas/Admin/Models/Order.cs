using ProjectShopIdentity.Areas.Saler.Data;
using ProjectShopIdentity.Data;
using System.ComponentModel.DataAnnotations;

namespace ProjectShopIdentity.Areas.Admin.Models
{
    public class Order
    {
        [Key]
        public int OrderID { get; set; }

        [Required]
        public DateTime Order_Date { get; set; }

        public virtual AppUser User { get; set; }
        public virtual Shop Shop { get; set; }
        public virtual Order_Status Status { get; set; }
        public virtual ICollection<Order_Item>? Order_Items { get; set; }   
    }
}
