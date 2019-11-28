using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThAmCo.Products.Data;

namespace ThAmCo.Products.Models.ViewModels
{
    public class ProductsIndexModel
    {
        public IEnumerable<Product> Products { get; set; }
        public int BrandId { get; set; }
        public int CategoryId { get; set; }
        public double PriceLow { get; set; }
        public double PriceHigh { get; set; }
    }
}
