using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;

namespace ProjectShopIdentity.Models
{
    public class RegisterAsSalerViewModel
    {
        [BindProperty]
        public string RoleName { get; set; }
    }
}
