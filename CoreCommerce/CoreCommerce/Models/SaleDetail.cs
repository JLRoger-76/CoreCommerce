using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoreCommerce.Models
{
    public class SaleDetail
    {
        [Key]
        public int SaleDetailId { get; set; }
        [Required]
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }

        // Foreign Key
        public int ProductId { get; set; }
        // Foreign Key
        public int SaleId { get; set; }
        // Navigation property
        public Sale Sale { get; set; }
        // Navigation property
        public Product Product { get; set; }
    }
}
