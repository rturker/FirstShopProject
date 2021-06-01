using ShopApp.Business.Abstract;
using ShopApp.Data.Abstract;
using ShopApp.Data.Concrete.EfCore;
using ShopApp.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShopApp.Business.Concrete
{
    public class ProductManager : IProductService
    {
        private IProductRepository _productRepository;
        public ProductManager(IProductRepository productRepository)
        {
            this._productRepository = productRepository;
        }


        public bool Create(Product entity)
        {
            if (Validate(entity))
            {
                _productRepository.Create(entity);
                return true;
            }
            return false;
            
        }

        public void Delete(Product entity)
        {
            _productRepository.Delete(entity);
        }

        public List<Product> GetAll()
        {
            return _productRepository.GetAll();
        }

        public Product GetById(int id)
        {
            return _productRepository.GetById(id);
        }

        public Product GetByIdWithCategories(int id)
        {
            return _productRepository.GetByIdWithCategories(id);
        }

        public int GetCountByCategory(string category)
        {
            return _productRepository.GetCountByCategory(category);
        }

        public List<Product> GetHomePageProducts()
        {
            return _productRepository.GetHomePageProducts();
        }

        public Product GetProductDetails(string url)
        {
            return _productRepository.GetProductDetails(url);
        }

        public List<Product> GetProductsByCategory(string name, int page, int pageSize)
        {
            return _productRepository.GetProductsByCategory(name, page, pageSize);
        }

        public List<Product> GetSearchResults(string searchString)
        {
            return _productRepository.GetSearchResults(searchString);
        }

        public void Update(Product entity)
        {
            //kontroller yapilacak
            _productRepository.Update(entity);
        }

        public bool Update(Product entity, int[] categoryIds)
        {
            if (Validate(entity))
            {
                if(categoryIds.Length == 0)
                {
                    ErrorMessage += "You must choose at least one category.";
                    return false;
                }
                _productRepository.Update(entity, categoryIds);
                return true;
            }
            return false;
        }

        public string ErrorMessage { get; set; }

        public bool Validate(Product entity)
        {
            var isValid = true;

            if (string.IsNullOrEmpty(entity.Name))
            {
                ErrorMessage += "You must enter product name. \n";
                isValid = false;
            }
            if (entity.Price < 0)
            {
                ErrorMessage += "You must enter a price upper zero. Price cannot be negative. \n";
                isValid = false;
            }

            return isValid;
        }
    }
}
