using Microsoft.AspNetCore.Mvc;
using ProjectShopIdentity.Reponsitory;
using ProjectShopIdentity.Data;

namespace ProjectShopIdentity.ViewComponents
{
    public class ProductNewMenuViewComponent:ViewComponent
    {
        private readonly IProduct _product;
        private readonly ApplicationDbContext _context;
        public ProductNewMenuViewComponent(IProduct product, ApplicationDbContext context)
        {
            _product = product;
            _context = context;
        }
        public IViewComponentResult Invoke()
        {
            return View(_product.GetAll());
        }
    }
}
