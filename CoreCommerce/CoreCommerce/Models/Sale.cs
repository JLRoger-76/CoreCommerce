using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoreCommerce.Models
{
    public class Sale
    {
        [Key]
        public int SaleId { get; set; }
        public double SalePrice { get; set; }
        public DateTime SaleDate { get; set; }
        public ICollection<SaleDetail> SaleDetails { get; set; }
    } 
}
