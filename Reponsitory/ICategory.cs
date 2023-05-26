using ProjectShopIdentity.Areas.Product.Models;

namespace ProjectShopIdentity.Reponsitory
{
    public interface ICategory
    {
        Category Add(Category category);
        Category Update(Category category);
        Category Delete(int id);
        Category Get(int id);

        IEnumerable<Category> GetAll();

    }
}
