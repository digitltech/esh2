using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ESH.Models
{
    public class Cart
    {
        [Key]
        public int id { get; set; }
        public string CartId { get; set; }
        public int ProductId { get; set; }
        public virtual Product Products { get; set; }
        public int Count { get; set; }
        public System.DateTime DateCreate { get; set; }


    }
}