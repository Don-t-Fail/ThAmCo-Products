using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ThAmCo.Products.Data;
using ThAmCo.Products.Models.ViewModels;

namespace ThAmCo.Products.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ProductsDbContext _context;
        private readonly IHttpClientFactory _clientFactory;

        public ProductsController(ProductsDbContext context, IHttpClientFactory clientFactory)
        {
            _context = context;
            _clientFactory = clientFactory;
        }

        // GET: Products
        public async Task<IActionResult> Index(double? PriceLow, double? PriceHigh, string Name, string Description, int BrandId = 0, int CategoryId = 0)
        {
            var products = await _context.Products.Where(p => p.Active).Include(p => p.Category).Include(p => p.Brand).ToListAsync();
            
            if (BrandId != 0)
                products = products.Where(p => p.BrandId == BrandId).ToList();
            if (CategoryId != 0)
                products = products.Where(p => p.CategoryId == CategoryId).ToList();

            if (Name != null)
                products = products.Where(p => p.Name.Contains(Name)).ToList();
            if (Description != null)
                products = products.Where(p => p.Description.Contains(Description)).ToList();

            var productsWithPriceStock = new List<ProductsPriceStockModel>();

            var client = _clientFactory.CreateClient("StandardRequest");
            client.DefaultRequestHeaders.Accept.ParseAdd("application/json");
            
            foreach (var p in products)
            {
                var response = await client.GetAsync("https://localhost:44385/stock/" + p.Id);
                if (response.IsSuccessStatusCode)
                {
                    var stockAndPrice = await response.Content.ReadAsAsync<ProductsPriceStockModel>();
                    productsWithPriceStock.Add(new ProductsPriceStockModel
                    {
                        Product = p,
                        Price = stockAndPrice.Price,
                        Stock = stockAndPrice.Stock
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

            ViewData["BrandList"] = new SelectList(_context.Brands, "Id", "Name");
            ViewData["CategoryList"] = new SelectList(_context.Category, "Id", "Name");

            return View(productIndex);
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var product = await _context.Products.FirstOrDefaultAsync(m => m.Id == id);

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
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var product = await _context.Products.FindAsync(id);

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
            if (id != product.Id)
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
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var product = await _context.Products.FirstOrDefaultAsync(m => m.Id == id);

            if (product == null)
                return NotFound();

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            product.Active = false;
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
