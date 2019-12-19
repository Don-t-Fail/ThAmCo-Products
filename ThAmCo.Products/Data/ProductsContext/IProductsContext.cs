using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ThAmCo.Products.Data.ProductsContext
{
    public interface IProductsContext
    {
        Task<IEnumerable<Product>> GetAll();
        Task<Product> GetProductAsync(int id);
        void AddProductAsync(Product product);
        void SoftDeleteProductAsync(Product product = null, int? id = null);
        void SaveAndUpdateContext();
    }
}