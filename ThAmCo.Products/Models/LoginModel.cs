using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ThAmCo.Products.Models
{
    public class LoginModel
    {
        //[Required, EmailAddress]
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
