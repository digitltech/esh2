using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ESH.Models
{
    public class Stock
    {
        public int id { get; set; }
        public int? StockTypeid { get; set; }
        public StockType StockTypes { get; set; }
        public int? ProductId { get; set; }
        public Product Products { get; set; }
            
    }

    public class StockType
    {
        public int id { get; set; }
        public string Name { get; set; }
    }
}