using System.Collections.Generic;

namespace ThAmCo.Products.Models.DTOs
{
    public class MultipleStockDTO
    {
        public ProductStockDTO ProductStock { get; set; }
        public PriceDTO Price { get; set; }
    }
}