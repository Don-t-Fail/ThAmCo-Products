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
        private List<Product> _products;

        public MockProductsContext(List<Product> context)
        {
            _products = context;
        }
        
        public Task<IEnumerable<Product>> GetAll()
        {
            return Task.FromResult(_products.AsEnumerable());
        }

        public Task<Product> GetProductAsync(int id)
        {
            return Task.FromResult(_products.FirstOrDefault(p => p.Id == id));
        }

        public void AddProductAsync(Product product)
        {
            _products.Add(product);
        }

        public async void SoftDeleteProductAsync(Product product = null, int? id = null)
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

        public async void SaveAndUpdateContext()
        {
            throw new NotImplementedException();
        }
    }
}