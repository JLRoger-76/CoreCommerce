using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CoreCommerce.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }
        public string Name { get; set; }
        public float Price { get; set; }
        public string Detail { get; set; }
        public string Image { get; set; }
        public int Stock { get; set; }
        // Foreign Key
        public int CategoryId { get; set; }
        // Navigation property
        public Category Category { get; set; }
        [NotMapped]
        [DisplayName("Upload File")]
        public IFormFile ImageFile { get; set; }
    }
}
