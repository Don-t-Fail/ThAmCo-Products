using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ThAmCo.Products.Data.ProductsContext
{
    public class ProductsContext : IProductsContext
    {
        private readonly ProductsDbContext _context;

        public ProductsContext(ProductsDbContext context)
        {
            _context = context;
        }
        
        public async Task<IEnumerable<Product>> GetAll()
        {
            return await _context.Products.Include(p => p.Category).Include(p => p.Brand).ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetAllActive()
        {
            return await _context.Products.Where(p => p.Active).Include(p => p.Category).Include(p => p.Brand).ToListAsync();
        }

        public async Task<Product> GetProductAsync(int id)
        {
            return await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
        }

        public void AddProductAsync(Product product)
        {
            _context.Add(product);
            SaveAndUpdateContext();
        }

        public async void SoftDeleteProductAsync(Product product = null, int? id = null)
        {
            if (product != null)
            {
                product.Active = false;
                _context.Update(product);
                SaveAndUpdateContext();
                return;
            }

            if (id == null || id <= 0) return;
            var productToChange = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
            if (productToChange != null)
            {
                productToChange.Active = false;
                _context.Update(productToChange);
                SaveAndUpdateContext();
            }
        }

        public Task<IEnumerable<Brand>> GetBrandsAsync()
        {
            return Task.FromResult(_context.Brands.AsEnumerable());
        }

        public Task<IEnumerable<Category>> GetCategoriesAsync()
        {
            return Task.FromResult(_context.Category.AsEnumerable());
        }

        public async void SaveAndUpdateContext()
        {
            await _context.SaveChangesAsync();
        }
    }
}