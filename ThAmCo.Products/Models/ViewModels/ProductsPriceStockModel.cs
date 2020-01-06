using ThAmCo.Products.Data;

namespace ThAmCo.Products.Models.ViewModels
{
    public class ProductsPriceStockModel
    {
        public Product Product { get; set; }
        public int? Stock { get; set; }
        public double? Price { get; set; }
    }
}