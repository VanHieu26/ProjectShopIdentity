using ProjectShopIdentity.Areas.Product.Models;

namespace ProjectShopIdentity.BussinessLogic.Interface
{
    public interface ICategoty
    {
        public Category Create(Category category);
        public Category Update(Category category, int id);
        public Category Delete(int id);
        public Category Get(int id);
        IEnumerable<Category> GetAll();
    }
}
