﻿using Microsoft.EntityFrameworkCore;
using ShopApp.Data.Abstract;
using ShopApp.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShopApp.Data.Concrete.EfCore
{
    public class EfCoreProductRepository :
        EfCoreGenericRepository<Product, ShopContext>, IProductRepository
    {
        public List<Product> GetPopularProducts()
        {
            using (var context = new ShopContext())
            {
                return context.Products.ToList();
            }
        }

        public List<Product> GetProductsByCategory(string name, int page, int pageSize)
        {
            using(var context = new ShopContext())
            {
                //if (!string.IsNullOrEmpty(name))
                //{
                //    var products = context.Products
                //                            .Include(i => i.ProductCategories)
                //                            .ThenInclude(i => i.Category)
                //                            .Where(c => c.ProductCategories.Any(a => a.Category.Url == name))
                //                            .ToList();
                //    return products;
                //}
                //return context.Products.ToList();

                var products = context.Products
                                        .Where(i => i.IsApproved)
                                        .AsQueryable();

                if (!string.IsNullOrEmpty(name))
                {
                    products = products
                                    .Include(i => i.ProductCategories)
                                    .ThenInclude(i => i.Category)
                                    .Where(i => i.ProductCategories.Any(a => a.Category.Url == name));
                }

                return products.Skip((page-1)*pageSize).Take(pageSize).ToList();
            }
        }

        public Product GetProductDetails(string url)
        {
            using (var context = new ShopContext())
            {
                return context.Products
                                .Where(i => i.Url == url)
                                .Include(i => i.ProductCategories)
                                .ThenInclude(i => i.Category)
                                .FirstOrDefault();
            }
        }

        public List<Product> GetTop5Products()
        {
            throw new System.NotImplementedException();
        }

        public int GetCountByCategory(string category)
        {
            using (var context = new ShopContext())
            {
                var products = context.Products
                                        .Where(i => i.IsApproved)
                                        .AsQueryable();

                if (!string.IsNullOrEmpty(category))
                {
                    products = products
                                    .Include(i => i.ProductCategories)
                                    .ThenInclude(i => i.Category)
                                    .Where(i => i.ProductCategories.Any(a => a.Category.Url == category));
                }

                return products.Count();
            }
        }

        public List<Product> GetHomePageProducts()
        {
            using(var context = new ShopContext())
            {
                return context.Products.Where(i => i.IsHome && i.IsApproved).ToList();
            }
        }

        public List<Product> GetSearchResults(string searchString)
        {
            using (var context = new ShopContext())
            {
                searchString = searchString.ToLower();
                var products = context.Products
                                        .Where(i => i.IsApproved && (i.Name.ToLower().Contains(searchString) || i.Description.ToLower().Contains(searchString)))
                                        .AsQueryable();

                return products.ToList();
            }
        }

        public Product GetByIdWithCategories(int id)
        {
            using(var context = new ShopContext())
            {
                return context.Products
                                .Where(i => i.ProductId == id)
                                .Include(i => i.ProductCategories)
                                .ThenInclude(i => i.Category)
                                .FirstOrDefault();
            }
        }

        public void Update(Product entity, int[] categoryIds)
        {
            using(var context = new ShopContext())
            {
                var product = context.Products
                                            .Include(i => i.ProductCategories)
                                            .FirstOrDefault(i => i.ProductId == entity.ProductId);
                if(product != null)
                {
                    product.Name = entity.Name;
                    product.Price = entity.Price;
                    product.Description = entity.Description;
                    product.Url = entity.Url;
                    product.IsApproved = entity.IsApproved;
                    product.IsHome = entity.IsHome;
                    product.ImageUrl = entity.ImageUrl;

                    product.ProductCategories = categoryIds.Select(catid => new ProductCategory()
                    {
                        ProductId = entity.ProductId,
                        CategoryId = catid
                    }).ToList();

                    context.SaveChanges();
                }
            }
        }
    }
}