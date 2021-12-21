using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ESH.Models
{
    public class Category
    {
        public int id { get; set; }
        public string Name { get; set; }
        public int ParentId { get; set; }
        public string URL { get; set; }
    }
}