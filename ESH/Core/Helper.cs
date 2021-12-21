using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using static ESH.Models.ESHDBModels;

namespace ESH.Core
{
    public class Helper
    {
       ESHDBContext db = new ESHDBContext();
        string OrderId { get; set; }
        public static Helper GetOrder(HttpContextBase context)
        {
            var order = new Helper();
            return order;
        }
        public int GetNumberOrder()
        {
            var order = db.Orders.ToList();
            var GetLastOrder = order.Last();
            var number = GetLastOrder.NumberOrder;
            return number;
        }
    }


    public class IPNetworking
    {
        public static string GetIP4Address()
        {
            string IP4Address = String.Empty;

            foreach (IPAddress IPA in Dns.GetHostAddresses(HttpContext.Current.Request.UserHostAddress))
            {
                if (IPA.AddressFamily.ToString() == "InterNetwork")
                {
                    IP4Address = IPA.ToString();
                    break;
                }
            }

            if (IP4Address != String.Empty)
            {
                return IP4Address;
            }

            foreach (IPAddress IPA in Dns.GetHostAddresses(Dns.GetHostName()))
            {
                if (IPA.AddressFamily.ToString() == "InterNetwork")
                {
                    IP4Address = IPA.ToString();
                    break;
                }
            }

            return IP4Address;
        }
    }
}