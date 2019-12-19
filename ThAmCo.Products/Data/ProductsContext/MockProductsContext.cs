using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ThAmCo.Products.Data.ProductsContext
{
    public class MockProductsContext : IProductsContext
    {
        private readonly List<Product> _products;
        private List<Brand> _brands;
        private List<Category> _categories;

        public MockProductsContext(List<Product> products, List<Brand> brands, List<Category> categories)
        {
            _products = products;
            _brands = brands;
            _categories = categories;
        }
        
        public Task<IEnumerable<Product>> GetAll()
        {
            return Task.FromResult(_products.AsEnumerable());
        }

        public Task<IEnumerable<Product>> GetAllActive()
        {
            return Task.FromResult(_products.Where(p => p.Active));
        }

        public Task<Product> GetProductAsync(int id)
        {
            return Task.FromResult(_products.FirstOrDefault(p => p.Id == id));
        }

        public void AddProductAsync(Product product)
        {
            _products.Add(product);
        }

        public void SoftDeleteProductAsync(Product product = null, int? id = null)
        {
            var chosenId = 0;
            if (product == null && id != null)
                chosenId = id ?? 0;
            if (product != null && id == null)
                chosenId = product.Id;
            var productFromList = _products.FirstOrDefault(p => p.Id == chosenId);
            if (productFromList != null)
            {
                _products.Remove(product);
                _products.Add(product);
            }
        }

        public Task<IEnumerable<Brand>> GetBrandsAsync()
        {
            return Task.FromResult(_brands.AsEnumerable());
        }

        public Task<IEnumerable<Category>> GetCategoriesAsync()
        {
            return Task.FromResult(_categories.AsEnumerable());
        }

        public void SaveAndUpdateContext()
        {
            throw new NotImplementedException();
        }
    }
}