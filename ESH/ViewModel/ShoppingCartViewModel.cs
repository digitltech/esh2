using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ESH.Models;
using System.ComponentModel.DataAnnotations;

namespace ESH.ViewModel
{
    public class ShoppingCartViewModel
    {
        [Key]
        public List<Cart> CartItems { get; set; }
        public decimal CartTotal { get; set; }
    }
}