using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoreCommerce.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }
        [MaxLength(55)]
        public string Name { get; set; }
        public int ParentId { get; set; }
    }
}
