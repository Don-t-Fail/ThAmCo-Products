using System;

namespace ThAmCo.Products.Models.DTOs
{
    public class PriceDTO
    {
        public int Id { get; set; }
        public int ProductStockId { get; set; }
        public double ProductPrice { get; set; }
        public DateTime Date { get; set; }
    }
}