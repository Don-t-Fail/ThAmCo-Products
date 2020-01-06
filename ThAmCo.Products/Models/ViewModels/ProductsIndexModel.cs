using System.Collections.Generic;

namespace ThAmCo.Products.Models.ViewModels
{
    public class ProductsIndexModel
    {
        public IEnumerable<ProductsPriceStockModel> Products { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? BrandId { get; set; }
        public int? CategoryId { get; set; }
        public double? PriceLow { get; set; }
        public double? PriceHigh { get; set; }
    }
}
