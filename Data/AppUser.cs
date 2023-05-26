using Microsoft.AspNetCore.Identity;
using ProjectShopIdentity.Areas.Admin.Models;
using ProjectShopIdentity.Areas.Product.Models;
using ProjectShopIdentity.Areas.Saler.Data;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace ProjectShopIdentity.Data
{
    public class AppUser:IdentityUser
    {

        [StringLength(200)]
        [AllowNull]
        public string Address { get; set; }

        public virtual ICollection<Shop> Shops { get; set; }
        public virtual ICollection<Order>? Orders { get; set; }
        public virtual Cart? Cart { get; set; }
    }
}
