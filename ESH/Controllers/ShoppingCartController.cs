using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static ESH.Models.ESHDBModels;
using ESH.Models;
using ESH.ViewModel;

namespace ESH.Controllers
{
    public class ShoppingCartController : Controller
    {
        ESHDBContext db = new ESHDBContext();
        // GET: ShoppingCart
        public ActionResult Index()
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);
            var viewModel = new ShoppingCartViewModel
            {
                CartItems = cart.GetCartItems(),
                CartTotal = cart.GetTotal()
            };
            var develiry = db.DeveliryPrices.ToList();
            ViewBag.develiry = develiry;
            var img = db.Imgs.ToList();
            ViewBag.img = img;
            return View(viewModel);
        }
        [HttpPost]
        public ActionResult AddToCart (int id)
        {
            var item = db.Products.Single(product => product.id == id);
            var cart = ShoppingCart.GetCart(this.HttpContext);
            int count = cart.AddToCart(item);
            var result = new ShoppingCartRemoveViewModel
            {
                Message = Server.HtmlEncode(item.Name) + " Добавлен в корзину",
                CartTotal = cart.GetTotal(),
                CartCount = cart.GetCount(),
                ItemCount = count,
                DeleteId = id
            };
            return Json(result);
        }
        [HttpPost]
        public ActionResult RemoveFromCart(int id)
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);
            string itemName = db.Products.Single(item => item.id == id).Name;
            int itemCount = cart.RemoveFromCart(id);
            var result = new ShoppingCartRemoveViewModel
            {
                Message = "Один (1)" + Server.HtmlEncode(itemName) +
                " был удален из корзины ",
                CartTotal = cart.GetTotal(),
                CartCount = cart.GetCount(),
                ItemCount = itemCount,
                DeleteId = id
            };
            return Json(result);
        }
        [ChildActionOnly]
        public ActionResult CartSumm()
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);
            if(cart.GetTotal()>0) ViewData["CartSumm"] = cart.GetTotal() + " руб.";

            if (cart.GetCount()>0)ViewData["CartCount"] = cart.GetCount();
            return PartialView("CartSumm");
        }
        [HttpPost]
        public ActionResult GetDeveliry(string develiry, string city)
        {
            var idcity = db.DeveliryPrices.SingleOrDefault(x => x.City == city).Id;
            
            return RedirectToAction("index", "Order", new { develiry = develiry, status = "new" , idcity=idcity });
        }
    }
}