using Microsoft.AspNetCore.Mvc;
using ProjectShopIdentity.Data;
using ProjectShopIdentity.Reponsitory;

namespace ProjectShopIdentity.ViewComponents
{
    public class CategoryMenuViewComponent:ViewComponent
    {
        private readonly ICategory _category;
        private readonly ApplicationDbContext _context;

        public CategoryMenuViewComponent(ICategory category, ApplicationDbContext context)
        {
            _category = category;
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            return View(_category.GetAll());
        }
    }
}
