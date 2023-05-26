using ProjectShopIdentity.Areas.Product.Models;


namespace ProjectShopIdentity.Reponsitory
{
    public interface IProduct
    {
       Products Create(Products products);
       Products Edit(Products products);
       Products Delete(int id);
       Products Get(int id);
       ICollection<Products> GetAll();
    }
}
