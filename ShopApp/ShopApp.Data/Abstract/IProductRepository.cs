using ShopApp.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShopApp.Data.Abstract
{
    public interface IProductRepository : IRepository<Product>
    {
        List<Product> GetPopularProducts();
        List<Product> GetTop5Products();
        Product GetProductDetails(string url);
        List<Product> GetProductsByCategory(string name, int page, int pageSize);
        int GetCountByCategory(string category);
        List<Product> GetHomePageProducts();
        List<Product> GetSearchResults(string searchString);
        Product GetByIdWithCategories(int id);
        void Update(Product entity, int[] categoryIds);
    }
}
