using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreCommerce.Models
{
    public class Parameter
    {
        public int categoryId { get; set; }
        public int productsByPage { get; set; }
        public int sort { get; set; }
        public int currentPage { get; set; }
        public string searchTerm { get; set; }
        public Parameter(int CategoryId, int ProductsByPage, int Sort, int CurrentPage, string SearchTerm)
        {
            categoryId = CategoryId;
            productsByPage = ProductsByPage;
            sort = Sort;
            currentPage = CurrentPage;
            searchTerm = SearchTerm;
        }
    }
}
