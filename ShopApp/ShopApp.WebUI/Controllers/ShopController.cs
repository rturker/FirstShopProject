using Microsoft.AspNetCore.Mvc;
using ShopApp.Business.Abstract;
using ShopApp.Entity;
using ShopApp.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopApp.WebUI.Controllers
{
 
    public class ShopController : Controller
    {
        private IProductService _productService;
        public ShopController(IProductService productService)
        {
            this._productService = productService;
        }

        public IActionResult List(string category, int page = 1)
        {
            const int pageSize = 6;
            var productViewModel = new ProductListViewModel()
            {
                PageInfo = new PageInfo()
                {
                    ToTalItems = _productService.GetCountByCategory(category),
                    CurrentPage = page,
                    ItemPerPage = pageSize,
                    CurrentCategory = category
                },
                Products = _productService.GetProductsByCategory(category, page, pageSize)
            };

            return View(productViewModel);
        }

        public IActionResult Details(string url)
        {
            if(url == null)
            {
                return NotFound();
            }
            Product product = _productService.GetProductDetails(url);
            if(product == null)
            {
                return NotFound();
            }
            return View(new ProductDetailModel(product, product.ProductCategories.Select(c => c.Category).ToList()));
        }

        public IActionResult Search(string q)
        {
            var productViewModel = new ProductListViewModel()
            {
                Products = _productService.GetSearchResults(q)
            };
            return View(productViewModel);
        }
    }
}
