using ProjectShopIdentity.Areas.Product.Models;
using ProjectShopIdentity.Data;

namespace ProjectShopIdentity.Reponsitory
{
    public class ProductReponsitory : IProduct
    {
        private readonly ApplicationDbContext _context;
        public ProductReponsitory(ApplicationDbContext context)
        {
            _context = context;
        }

        public Products Create(Products products)
        {
            _context.Products.Add(products);
            _context.SaveChanges();
            return products;
        }

        public Products Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Products Edit(Products products)
        {
            throw new NotImplementedException();
        }

        public Products Get(int id)
        {
            return _context.Products.FirstOrDefault(x => x.Id == id);
        }

        public ICollection<Products> GetAll()
        {
            var products = _context.Products.OrderByDescending(x => x.Id).Take(8).ToList();
            return products;
        }
    }
}
