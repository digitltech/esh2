using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static ESH.Models.ESHDBModels;

namespace ESH.Models
{
    public partial class ShoppingCart
    {
        ESHDBContext db = new ESHDBContext();
        string ShoppingCartId { get; set; }
        public const string CartSessionKey = "CartId";

        public static ShoppingCart GetCart (HttpContextBase context)
        {
            var cart = new ShoppingCart();
            cart.ShoppingCartId = cart.GetCartId(context);
            return cart;
        }
        public static ShoppingCart GetCart (Controller controller)
        {
            return GetCart(controller.HttpContext);
        }
        public int AddToCart (Product product)
        {
            var cartItem = db.Carts.SingleOrDefault(
                c => c.CartId == ShoppingCartId
                && c.ProductId == product.id);
            if(cartItem == null)
            {
                cartItem = new Cart
                {
                    ProductId = product.id,
                    CartId = ShoppingCartId,
                    Count = 1,
                    DateCreate = DateTime.Now
                };
                db.Carts.Add(cartItem);
            }
            else
            {
                cartItem.Count++;
            }
            db.SaveChanges();
            return cartItem.Count;
        }

        public int RemoveFromCart (int id)
        {
            var cartItem = db.Carts.Single(
                cart => cart.CartId == ShoppingCartId
                && cart.ProductId == id);

            int itemCount = 0;

            if (cartItem != null)
            {
                if (cartItem.Count > 1)
                {
                    cartItem.Count--;
                    itemCount = cartItem.Count;
                }
                else
                {
                    db.Carts.Remove(cartItem);
                }
                db.SaveChanges();
            }
            return itemCount;
        }

        public void EmptyCart()
        {
            var CartItems = db.Carts.Where(
                cart => cart.CartId == ShoppingCartId);
            foreach (var cartItem in CartItems)
            {
                db.Carts.Remove(cartItem);
            }
            db.SaveChanges();
        }

        public List<Cart> GetCartItems()
        {
            return db.Carts.Where(
                cart => cart.CartId == ShoppingCartId).ToList();
        }   

        public int GetCount()
        {
            int? count = (from cartItems in db.Carts
                          where cartItems.CartId == ShoppingCartId
                          select (int?)cartItems.Count).Sum();
            return count ?? 0;
        }

        public decimal GetTotal()
        {
            decimal? total = (from cartItems in db.Carts
                              where cartItems.CartId == ShoppingCartId
                              select (int?)cartItems.Count *
                              cartItems.Products.Price).Sum();
            return total ?? decimal.Zero;
        }
        public Order CreateOrder(Order order)
        {
            decimal orderTotal = 0;
            order.OrderDetails = new List<OrderDetails>();

            var cartItems = GetCartItems();

            foreach (var item in cartItems)
            {
                var orderDetail = new OrderDetails
                {

                    ProductId = item.ProductId,
                    OrderId = order.id,
                    UnitPrice = item.Products.Price,
                    Quantity = item.Count
                };
                orderTotal += (item.Count * item.Products.Price);
           
                order.OrderDetails.Add(orderDetail);
                db.OrderDetails.Add(orderDetail);


            }

            foreach (var temp in order.OrderDetails)
            {
                var idpost = temp.Products.ArticlePos;
                var token = Core.GetFromNetlabs.GetToken();
                var ostatok = Core.GetFromNetlabs.GetOstatok(token, idpost);
                if (temp.Products.PaymentOnline != true)
                {
                    order.PayOnline = false;
                    order.StatusOrderId = 1;
                    break;
                }
                
               
                   else if (ostatok==0)
                {
                    order.PayOnline = false;
                    order.StatusOrderId = 1;
                    break;
                }

                else
                {
                    order.PayOnline = true;
                    order.StatusOrderId = 3;
                }
               


            }

            order.Total = orderTotal;
            order.TotalSumm = orderTotal + order.ShippingSumm;
            db.Entry(order).State = EntityState.Modified;
            db.SaveChanges();  /*Все в порядке*/
            EmptyCart();
            return order;
        }

        public string GetCartId (HttpContextBase context)
        {
            if (context.Session[CartSessionKey] == null)
            {
                if (! string.IsNullOrWhiteSpace(context.User.Identity.Name))
                {
                    context.Session[CartSessionKey] =
                        context.User.Identity.Name;
                }
                else
                {
                    Guid tempCartId = Guid.NewGuid();
                    context.Session[CartSessionKey] = tempCartId.ToString();
                }
            }
            return context.Session[CartSessionKey].ToString();
        }

        public void MigrateCart(string userName)
        {
            var shoppingCart = db.Carts.Where(
                c => c.CartId == ShoppingCartId);
            foreach (Cart item in shoppingCart)
            {
                item.CartId = userName;
            }
            db.SaveChanges();
        }
    }
}