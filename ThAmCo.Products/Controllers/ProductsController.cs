using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ThAmCo.Products.Data;
using ThAmCo.Products.Data.ProductsContext;
using ThAmCo.Products.Models.DTOs;
using ThAmCo.Products.Models.ViewModels;

namespace ThAmCo.Products.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductsContext _context;
        private readonly IHttpClientFactory _clientFactory; 

        public HttpClient HttpClient { get; set; }

        public ProductsController(IProductsContext context, IHttpClientFactory clientFactory)
        {
            _context = context;
            _clientFactory = clientFactory;
        }

        // GET: Products
        //Auth here?
        [AllowAnonymous]
        public async Task<IActionResult> Index(double? PriceLow, double? PriceHigh, string Name, string Description, int BrandId = 0, int CategoryId = 0)
        {
            var authenticated = false;

            try
            {
                var authentication = await HttpContext.AuthenticateAsync();
                authenticated = authentication.Succeeded;
            }
            catch
            {
            }

            var products = await _context.GetAllActive();

            if (BrandId != 0)
                products = products.Where(p => p.BrandId == BrandId).ToList();
            if (CategoryId != 0)
                products = products.Where(p => p.CategoryId == CategoryId).ToList();

            if (!String.IsNullOrEmpty(Name))
                products = products.Where(p => p.Name.ToLower().Contains(Name.ToLower())).ToList();
            if (!String.IsNullOrEmpty(Description))
                products = products.Where(p => p.Description.Contains(Description)).ToList();

            var productsWithPriceStock = new List<ProductsPriceStockModel>();

            var client = GetHttpClient("StandardRequest");
            client.DefaultRequestHeaders.Accept.ParseAdd("application/json");

            var response = await client.GetAsync("https://localhost:44385/stock/ProductStocks");
            if (response.IsSuccessStatusCode)
            {
                var objectResult = await response.Content.ReadAsAsync<List<MultipleStockDTO>>();
                foreach (var t in products)
                {
                    int? stock = null;
                    if (authenticated)
                        stock = objectResult.FirstOrDefault(or => or.ProductStock.ProductId == t.Id).ProductStock.Stock;
                    productsWithPriceStock.Add(new ProductsPriceStockModel
                    {
                        Product = t,
                        Price = objectResult.FirstOrDefault(or => or.ProductStock.ProductId == t.Id).Price.ProductPrice,
                        Stock = stock
                    });
                }
            }
            else
                productsWithPriceStock.AddRange(products.Select(p => new ProductsPriceStockModel { Product = p, Price = null, Stock = null }));

            var productIndex = new ProductsIndexModel
            {
                Name = Name ?? "",
                Description = Description ?? "",
                Products = productsWithPriceStock,
                BrandId = BrandId,
                CategoryId = CategoryId
            };

            var selectListBrands = new List<Brand> { new Brand { Id = 0, Name = "All Brands" } };
            selectListBrands.AddRange(await _context.GetBrandsAsync());
            var selectListCategories = new List<Category> { new Category { Id = 0, Name = "All Categories" } };
            selectListCategories.AddRange(await _context.GetCategoriesAsync());

            ViewData["BrandList"] = new SelectList(selectListBrands, "Id", "Name");
            ViewData["CategoryList"] = new SelectList(selectListCategories, "Id", "Name");

            return View(productIndex);
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int id)
        {
            if (id <= 0)
                return NotFound();

            var product = await _context.GetProductAsync(id);

            if (product == null)
                return NotFound();

            var client = GetHttpClient("ReviewRequest");
            client.DefaultRequestHeaders.Accept.ParseAdd("application/json");

            var objectResult = new List<ReviewDto>();
            HttpResponseMessage response = null;
            try
            {
                response = await client.GetAsync("https://localhost:44367/reviews/GetReviewProduct?prodid=" + id);
            }
            catch
            {

            }
            
            if (response != null && response.IsSuccessStatusCode)
            {
                objectResult = await response.Content.ReadAsAsync<List<ReviewDto>>();
            }

            return View(new DetailsWithReviewsModelcs { Product = product, Reviews = objectResult });
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var product = await _context.GetProductAsync(id ?? 0);

            if (product == null)
                return NotFound();

            return View(product);
        }

        // POST: Products/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.GetProductAsync(id);
            _context.SoftDeleteProductAsync(product);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            return Ok(await _context.GetProductAsync(id));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetAllProducts()
        {
            return Ok(await _context.GetAllActive());
        }

        private bool ProductExists(int id)
        {
            return _context.GetAll().Result.Any(e => e.Id == id);
        }

        private HttpClient GetHttpClient(string s)
        {
            if (_clientFactory == null && HttpClient != null) return HttpClient;
            
            return _clientFactory.CreateClient(s);
        }
    }
}
