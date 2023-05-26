using System.ComponentModel.DataAnnotations;

namespace ProjectShopIdentity.Areas.Admin.Models
{
    public class Order_Status
    {
        [Key]
        public int OrderSID { get; set; }


        [Required(ErrorMessage = "Vui long nhap trang thai don hang")]
        [Display(Name = "Oreder Status Name")]
        [StringLength(60, MinimumLength =1 , ErrorMessage = "{0} phai nhieu hon {2} va it hon {1} ki tu ")]
        public string OrderSName { get; set; }
        public virtual ICollection<Order>? Orders { get; set; }
    }
}
