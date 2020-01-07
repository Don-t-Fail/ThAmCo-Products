using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThAmCo.Products.Data;
using ThAmCo.Products.Models.DTOs;

namespace ThAmCo.Products.Models.ViewModels
{
    public class DetailsWithReviewsModelcs
    {
        public Product Product { get; set; }
        public List<ReviewDto> Reviews { get; set; }
    }
}
