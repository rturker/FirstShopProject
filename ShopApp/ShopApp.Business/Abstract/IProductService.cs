using ShopApp.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShopApp.Business.Abstract
{
    public interface IProductService : IValidator<Product>
    {
        List<Product> GetProductsByCategory(string name, int page, int pageSize);
        Product GetById(int id);
        Product GetProductDetails(string url);
        List<Product> GetAll();
        bool Create(Product entity);
        void Update(Product entity);
        bool Update(Product entity, int[] categoryIds);
        void Delete(Product entity);
        int GetCountByCategory(string category);
        List<Product> GetHomePageProducts();
        List<Product> GetSearchResults(string searchString);
        Product GetByIdWithCategories(int id);
    }
}
