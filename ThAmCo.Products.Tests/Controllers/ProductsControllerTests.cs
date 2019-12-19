using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ThAmCo.Products.Controllers;
using ThAmCo.Products.Data;
using ThAmCo.Products.Data.ProductsContext;
using System.Net.Http;

namespace ThAmCo.Products.Tests.Controllers
{
    [TestClass]
    public class ProductsControllerTests
    {
        //[TestMethod]
        public async Task GetProductIndex_AllValid_AllReturned()
        {
            var brands = new List<Brand>()
            {
                new Brand { Id = 1, Name = "Brand 1"},
                new Brand { Id = 2, Name = "Brand 2"}
            };
            var categories = new List<Category>()
            {
                new Category { Id = 1, Name = "Category 1"},
                new Category { Id = 2, Name = "Category 2"}
            };
            var products = new List<Product>()
            {
                new Product{ Id = 1, Name = "Product 1", Description = "Description 1", BrandId = 1, Brand = brands[0], CategoryId = 1, Category = categories[0], Active = true},
                new Product{ Id = 2, Name = "Product 2", Description = "Description 2", BrandId = 2, Brand = brands[1], CategoryId = 2, Category = categories[1], Active = true}
            };
            var context = new MockProductsContext(products, brands, categories);
            var controller = new ProductsController(context, null);

            var result = await controller.Index(null, null, null, null);
            
            Assert.IsNotNull(result);
            var objectResult = result as OkObjectResult;
            Assert.IsNotNull(objectResult);
            var enumerableResult = objectResult.Value as IEnumerable<Product>;
            Assert.IsNotNull(enumerableResult);
            var enumerableResultList = enumerableResult.ToList();
            Assert.AreEqual(products, enumerableResultList);
        }

        [TestMethod]
        public async Task GetProductDetails_AllValid_CorrectReturned()
        {
            var brands = new List<Brand>()
            {
                new Brand { Id = 1, Name = "Brand 1"},
                new Brand { Id = 2, Name = "Brand 2"}
            };
            var categories = new List<Category>()
            {
                new Category { Id = 1, Name = "Category 1"},
                new Category { Id = 2, Name = "Category 2"}
            };
            var products = new List<Product>()
            {
                new Product{ Id = 1, Name = "Product 1", Description = "Description 1", BrandId = 1, Brand = brands[0], CategoryId = 1, Category = categories[0], Active = true},
                new Product{ Id = 2, Name = "Product 2", Description = "Description 2", BrandId = 2, Brand = brands[1], CategoryId = 2, Category = categories[1], Active = true}
            };
            var context = new MockProductsContext(products, brands, categories);
            var controller = new ProductsController(context, null);

            var result = await controller.Details(2);
            
            Assert.IsNotNull(result);
            var productResult = result as ViewResult;
            Assert.IsNotNull(productResult);
            var objectResult = productResult.Model as Product;
            Assert.IsNotNull(objectResult);
            Assert.AreEqual(objectResult, products[1]);
        }
    }
}
