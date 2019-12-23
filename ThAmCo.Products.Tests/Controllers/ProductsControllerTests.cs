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
            
            public static List<ProductStockDTO> ProductStocks() => new List<ProductStockDTO>
            {
                new ProductStockDTO { Id = 1, Stock = 10, PriceId = 1, ProductId = 1 },
                new ProductStockDTO { Id = 2, Stock = 0, PriceId = 3, ProductId = 2 }
            };
            
            public static List<PriceDTO> Prices() => new List<PriceDTO>
            {
                new PriceDTO { Id = 1, ProductStockId = 1, ProductPrice = 8.99 },
                new PriceDTO { Id = 2, ProductStockId = 2, ProductPrice = 24.99 },
                new PriceDTO { Id = 3, ProductStockId = 2, ProductPrice = 19.99 }
            };
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
        
        [TestMethod]
        public async Task GetProductIndex_AllValid_AllReturned()
        {
            var expectedResult = new MultipleStockListDTO
            {
                Stocks = new List<MultipleStockDTO>
                {
                    new MultipleStockDTO { ProductStock = Data.ProductStocks()[0], Price = Data.Prices()[0]},
                    new MultipleStockDTO { ProductStock = Data.ProductStocks()[1], Price = Data.Prices()[2] }
                }
            };
            var expectedJson = JsonConvert.SerializeObject(expectedResult);
            var expectedUri = new Uri("https://localhost:44385/stock/");
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
            controller.HttpClient = new HttpClient(mock.Object);

            var result = await controller.Index(null, null, null, null);
            
            Assert.IsNotNull(result);
            var objectResult = result as ViewResult;
            Assert.IsNotNull(objectResult);
            var dtoResult = objectResult.Model as ProductsIndexModel;
            Assert.IsNotNull(dtoResult);
            
            Assert.IsNull(dtoResult.PriceLow);
            Assert.IsNull(dtoResult.PriceHigh);
            Assert.AreEqual(dtoResult.Name, string.Empty);
            Assert.AreEqual(dtoResult.Description, string.Empty);
            
            foreach (var p in dtoResult.Products)
            {
                var productExpected = Data.Products().FirstOrDefault(prod => prod.Id == p.Product.Id);
                Assert.IsNotNull(productExpected);
                var brandExpected = Data.Brands().FirstOrDefault(brand => brand.Id == productExpected.BrandId);
                Assert.IsNotNull(brandExpected);
                var categoryExpected = Data.Categories().FirstOrDefault(cat => cat.Id == productExpected.CategoryId);
                Assert.IsNotNull(categoryExpected);
                var stockExpected = Data.ProductStocks().FirstOrDefault(stocks => stocks.ProductId == p.Product.Id);
                Assert.IsNotNull(stockExpected);
                var priceExpected = Data.Prices().FirstOrDefault(price => price.Id == stockExpected.PriceId);
                Assert.IsNotNull(priceExpected);
                
                Assert.AreEqual(stockExpected.Stock, p.Stock);
                Assert.AreEqual(priceExpected.ProductPrice, p.Price);
                
                Assert.AreEqual(p.Product.Active, productExpected.Active);
                Assert.AreEqual(p.Product.Description, productExpected.Description);
                Assert.AreEqual(p.Product.Id, productExpected.Id);
                Assert.AreEqual(p.Product.Name, productExpected.Name);
                Assert.AreEqual(p.Product.BrandId, productExpected.BrandId);
                Assert.AreEqual(p.Product.CategoryId, productExpected.CategoryId);
                
                Assert.AreEqual(p.Product.Brand.Description, brandExpected.Description);
                Assert.AreEqual(p.Product.Brand.Id, brandExpected.Id);
                Assert.AreEqual(p.Product.Brand.Name, brandExpected.Name);
                Assert.AreEqual(p.Product.Brand.AvailableProductCount, brandExpected.AvailableProductCount);
                
                Assert.AreEqual(p.Product.Category.Id, categoryExpected.Id);
                Assert.AreEqual(p.Product.Category.Name, categoryExpected.Name);
                Assert.AreEqual(p.Product.Category.AvailableProductCount, categoryExpected.AvailableProductCount);
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
