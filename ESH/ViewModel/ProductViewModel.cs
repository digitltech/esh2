using ESH.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ESH.ViewModel
{
    public class ProductViewModel
    {
        public IEnumerable<Product> Product { get; set; }
        public IEnumerable<Category> Catalogy { get; set; }
    }
}