using Microsoft.EntityFrameworkCore;
using ShopApp.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShopApp.Data.Concrete.EfCore
{
    public static class SeedDatabase
    {
        public static void Seed()
        {
            var context = new ShopContext();
            if(context.Database.GetPendingMigrations().Count() == 0)
            {
                if(context.Categories.Count() == 0)
                {
                    context.Categories.AddRange(Categories);
                }

                if (context.Products.Count() == 0)
                {
                    context.Products.AddRange(Products);
                    context.AddRange(ProductCategories);
                }
            }
            context.SaveChanges();
        }

        private static List<Category> Categories = new List<Category>()
        {
            new Category {Name="Telefon", Url = "telefon"},
            new Category {Name="Bilgisayar", Url = "bilgisayar"},
            new Category {Name="Elektronik", Url = "elektronik"},
            new Category {Name="Bilgisayar Bilesenleri", Url = "bilgisayar-bilesenleri"}
        };

        private static List<Product> Products = new List<Product>()
        {
            new Product {Name="Iphone 7", Url = "iphone-7",Price=3000,Description="iyi telefon",IsApproved=false, ImageUrl="1.jpg"},
            new Product {Name="Iphone 8", Url = "iphone-8",Price=4000,Description="çok iyi telefon",IsApproved=true, ImageUrl="2.jpg"},
            new Product {Name="Iphone X", Url = "iphone-X",Price=5000,Description="çok iyi telefon",IsApproved=true, ImageUrl="3.jpg"},
            new Product {Name="Iphone 11", Url = "iphone-11",Price=7000,Description="çok iyi telefon", ImageUrl="4.jpg"}

        };

        private static List<ProductCategory> ProductCategories = new List<ProductCategory>()
        {
            new ProductCategory {Product = Products[0], Category = Categories[0]},
            new ProductCategory {Product = Products[0], Category = Categories[2]},
            new ProductCategory {Product = Products[1], Category = Categories[1]},
            new ProductCategory {Product = Products[1], Category = Categories[0]},
            new ProductCategory {Product = Products[2], Category = Categories[1]},
            new ProductCategory {Product = Products[2], Category = Categories[0]},
            new ProductCategory {Product = Products[3], Category = Categories[2]},
        };
    }
}
