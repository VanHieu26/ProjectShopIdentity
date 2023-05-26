using Microsoft.EntityFrameworkCore;
using ProjectShopIdentity.Areas.Product.Models;
using ProjectShopIdentity.Data;

namespace ProjectShopIdentity.Reponsitory
{
    public class CategoryReponsetory : ICategory
    {
        public readonly ApplicationDbContext _context;
        public CategoryReponsetory(ApplicationDbContext context)
        {
            _context = context;
        }

        public Category Add(Category category)
        {
            _context.Categories.Add(category);
            _context.SaveChanges();
            return category;
        }

        public Category Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Category Get(int id)
        {
            return _context.Categories.FirstOrDefault(c => c.Id == id);
        }

        public IEnumerable<Category> GetAll()
        {
            var categories = _context.Categories.Include(c => c.product_Catogories);

            var categoriespr = categories.Select(c =>
            
                new Category
                {
                    Id = c.Id,
                    Name = c.Name,
                    Picture = c.Picture,
                    ProductInCategory = c.product_Catogories.Count
                }
            );
            return categoriespr;
        }

        public Category Update(Category category)
        {
            throw new NotImplementedException();
        }
    }
}
