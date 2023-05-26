using ProjectShopIdentity.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectShopIdentity.Areas.Product.Models
{
    public class Cart
    {
        [Key]
        public int CartId { get; set; }
        [Column("User_ID")]
        public string UserId { get; set; }
        public virtual AppUser? AppUser { get; set; }
        public virtual ICollection<Cart_Item>? Cart_Items { get; set; }
    }
}
