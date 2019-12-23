using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
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
        public async Task<IActionResult> Index(double? PriceLow, double? PriceHigh, string Name, string Description, int BrandId = 0, int CategoryId = 0)
        {
            var products = await _context.GetAllActive();
            
            if (BrandId != 0)
                products = products.Where(p => p.BrandId == BrandId).ToList();
            if (CategoryId != 0)
                products = products.Where(p => p.CategoryId == CategoryId).ToList();

            if (Name != null)
                products = products.Where(p => p.Name.Contains(Name)).ToList();
            if (Description != null)
                products = products.Where(p => p.Description.Contains(Description)).ToList();

            var productsWithPriceStock = new List<ProductsPriceStockModel>();

            var client = GetHttpClient("StandardRequest");
            client.DefaultRequestHeaders.Accept.ParseAdd("application/json");
            
            foreach (var p in products)
            {
                var response = await client.GetAsync("https://localhost:44385/stock/1");
                if (response.IsSuccessStatusCode)
                {
                    var stockAndPrice = await response.Content.ReadAsAsync<FromSingleStockDTO>();
                    productsWithPriceStock.Add(new ProductsPriceStockModel
                    {
                        Product = p,
                        Price = stockAndPrice?.Price,
                        Stock = stockAndPrice?.Stock
                    });
                }
                else
                    productsWithPriceStock.Add(new ProductsPriceStockModel
                    {
                        Product = p,
                        Price = null,
                        Stock = null
                    });
            }

            var productIndex = new ProductsIndexModel
            {
                Name = Name ?? "",
                Description = Description ?? "",
                Products = productsWithPriceStock,
                BrandId = BrandId,
                CategoryId = CategoryId
            };

            ViewData["BrandList"] = new SelectList(_context.GetBrandsAsync().Result, "Id", "Name");
            ViewData["CategoryList"] = new SelectList(_context.GetCategoriesAsync().Result, "Id", "Name");

            return View(productIndex);
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var product = await _context.GetProductAsync(id ?? 0);

            if (product == null)
                return NotFound();

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,Active")] Product product)
        {
            if (ModelState.IsValid)
            {
                _context.AddProductAsync(product);
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var product = await _context.GetProductAsync(id ?? 0);

            if (product == null)
                return NotFound();

            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Active")] Product product)
        {
            throw new NotImplementedException();
            /*if (id != product.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(product);*/
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
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.GetProductAsync(id);
            _context.SoftDeleteProductAsync(product);
            return RedirectToAction(nameof(Index));
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
