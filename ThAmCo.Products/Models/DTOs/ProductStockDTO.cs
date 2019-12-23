namespace ThAmCo.Products.Models.DTOs
{
    public class ProductStockDTO
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int Stock { get; set; }
        public int PriceId { get; set; }
    }
}