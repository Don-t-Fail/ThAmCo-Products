using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using ThAmCo.Products.Controllers;
using ThAmCo.Products.Data;
using ThAmCo.Products.Data.ProductsContext;
using ThAmCo.Products.Models.DTOs;
using ThAmCo.Products.Models.ViewModels;

namespace ThAmCo.Products.Tests.Controllers
{
    [TestClass]
    public class ProductsControllerTests
    {
        private static class Data
        {
            public static List<Brand> Brands()
            {
                return new List<Brand>
                {
                    new Brand { Id = 1, Name = "Brand 1", Description = "Description 1", AvailableProductCount = 4 },
                    new Brand { Id = 2, Name = "Brand 2", Description = "Description 2", AvailableProductCount = 2 }
                };
            }

            public static List<Category> Categories()
            {
                return new List<Category>
                {
                    new Category { Id = 1, Name = "Category 1", AvailableProductCount = 4 },
                    new Category { Id = 2, Name = "Category 2", AvailableProductCount = 12 }
                };
            }

            public static List<Product> Products()
            {
                return new List<Product>
                {
                    new Product{ Id = 1, Name = "Product 1", Description = "Description 1", BrandId = 1, Brand = Brands()[0], CategoryId = 1, Category = Categories()[0], Active = true},
                    new Product{ Id = 2, Name = "Product 2", Description = "Description 2", BrandId = 2, Brand = Brands()[1], CategoryId = 2, Category = Categories()[1], Active = true}
                };
            }
        }

        private Mock<HttpMessageHandler> CreateHttpMock(HttpResponseMessage expected)
        {
            var mock = new Mock<HttpMessageHandler>();
            mock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(expected)
                .Verifiable();
            return mock;
        }
        
        //[TestMethod]
        public async Task GetProductIndex_AllValid_AllReturned()
        {
            var expectedResult = new FromSingleStockDTO { ProductID = 1, Stock = 4, Price = 8.99 };
            var expectedJson = JsonConvert.SerializeObject(expectedResult);
            var expectedUri = new Uri("https://localhost:44385/stock/1");
            var expectedResponse = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(expectedJson,
                                            Encoding.UTF8,
                                            "application/json")
            };
            var mock = CreateHttpMock(expectedResponse);
            
            var httpClient = new HttpClient(mock.Object);

            var brands = Data.Brands();
            var categories = Data.Categories();
            var products = Data.Products();
            var context = new MockProductsContext(products, brands, categories);
            var controller = new ProductsController(context, null);
            controller._httpClient = new HttpClient(mock.Object);

            var result = await controller.Index(null, null, null, null);
            
            Assert.IsNotNull(result);
            var objectResult = result as OkObjectResult;
            Assert.IsNotNull(objectResult);
            var dtoResult = objectResult.Value as ProductsIndexModel;
            Assert.IsNotNull(dtoResult);
            
            Assert.IsNull(dtoResult.PriceLow);
            Assert.IsNull(dtoResult.PriceHigh);
            Assert.IsNull(dtoResult.Name);
            Assert.IsNull(dtoResult.Description);
            foreach (var p in dtoResult.Products)
            {
                Assert.AreEqual(4, p.Stock);
                Assert.AreEqual(8.99, p.Price);
            }
        }

        [TestMethod]
        public async Task GetProductDetails_AllValid_CorrectReturned()
        {
            var brands = Data.Brands();
            var categories = Data.Categories();
            var products = Data.Products();
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
        
        [TestMethod]
        public async Task GetProductDetails_OutOfBounds_NotFoundReturned()
        {
            var brands = Data.Brands();
            var categories = Data.Categories();
            var products = Data.Products();
            var context = new MockProductsContext(products, brands, categories);
            var controller = new ProductsController(context, null);

            var result = await controller.Details(3);
            
            Assert.IsNotNull(result);
            var productResult = result as NotFoundResult;
            Assert.IsNotNull(productResult);
        }
        
        [TestMethod]
        public async Task GetProductDetails_NullPassed_NotFoundReturned()
        {
            var brands = Data.Brands();
            var categories = Data.Categories();
            var products = Data.Products();
            var context = new MockProductsContext(products, brands, categories);
            var controller = new ProductsController(context, null);

            var result = await controller.Details(null);
            
            Assert.IsNotNull(result);
            var productResult = result as NotFoundResult;
            Assert.IsNotNull(productResult);
        }
        
        [TestMethod]
        public async Task SoftDeleteProduct_Valid_ActiveIsFalse()
        {
            var brands = Data.Brands();
            var categories = Data.Categories();
            var products = Data.Products();
            var context = new MockProductsContext(products, brands, categories);
            var controller = new ProductsController(context, null);

            var result = await controller.DeleteConfirmed(1);
            var resultContent = context.GetProductAsync(1).Result;
            var expectedResult = products.FirstOrDefault(p => p.Id == 1);
            if (expectedResult != null)
                expectedResult.Active = false;
            
            Assert.IsNotNull(expectedResult);
            Assert.IsNotNull(result);
            var productResult = result as RedirectToActionResult;
            Assert.IsNotNull(productResult);
            Assert.AreEqual(resultContent, expectedResult);
        }
        
        [TestMethod]
        public async Task SoftDeleteProduct_OutOfBounds_NoChange()
        {
            var brands = Data.Brands();
            var categories = Data.Categories();
            var products = Data.Products();
            var context = new MockProductsContext(products, brands, categories);
            var controller = new ProductsController(context, null);

            var unchangedResult = context.GetAll().Result.ToList();
            var result = await controller.DeleteConfirmed(3);
            var resultContent = context.GetAll().Result.ToList();

            Assert.AreEqual(unchangedResult.Count, resultContent.Count);
            Assert.IsNotNull(result);
            var productResult = result as RedirectToActionResult;
            Assert.IsNotNull(productResult);
            CollectionAssert.AreEqual(resultContent, unchangedResult);
        }
    }
}
