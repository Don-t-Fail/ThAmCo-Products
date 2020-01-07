using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ThAmCo.Products.Models.DTOs
{
    public class ReviewDto
    {
        public int Id { get; set; }
        public int PurchaseId { get; set; }
        public bool IsVisible { get; set; }
        public int Rating { get; set; }
        public string Content { get; set; }
    }
}
