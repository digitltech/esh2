using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static ESH.Models.ESHDBModels;
using System.Data.Entity;
using ESH.Models;
using System.Net.Mail;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Net;
using System.Text;
using System.IO;

namespace ESH.Areas.Portal.Controllers
{
    public class OrdersController : Controller
    {
      ESHDBContext db = new ESHDBContext();
        // GET: Portal/Orders
        public ActionResult Main()
        {
            return View();
        }

        public ActionResult Index()
        {
            
            return View(db.Orders.Include(p=>p.StatusOrders).ToList());
        }

        public ActionResult Details(int? id)
        {
            Order order =  db.Orders.Include(p=>p.PaymentTypes).Include(p=>p.Develireries).SingleOrDefault(x=>x.id==id);
            var status = db.StatusOrders.Single(x => x.id == order.StatusOrderId);
            ViewBag.status = status;
      
            var product = db.OrderDetails.Where(x => x.OrderId == id);
            order.OrderDetails = product.ToList();
            return View(order);
        }
        [HttpGet]
        public ActionResult Edit (int? id)
        {
          if (id == null)
            {
                return HttpNotFound();
            }
            var order = db.Orders.Find(id);
            if (order != null)
            {
       
     
                SelectList PaymentTypes = new SelectList(db.PaymentTypes, "Id", "Name", order.id);
                ViewBag.PaymentTypes = PaymentTypes;
                SelectList Develireries = new SelectList(db.Develireries, "Id", "Name", order.id);
                ViewBag.Develireries = Develireries;
                var product = db.OrderDetails.Where(x => x.OrderId == id);
                order.OrderDetails = product.ToList();
                SelectList status = new SelectList(db.StatusOrders, "Id", "Name", order.id);
           
                ViewBag.status = status;
                return View(order);
            }
            return HttpNotFound();
        }

        [HttpPost]
        public ActionResult Edit (Order order)
        {
            order.TotalSumm = order.Total + order.ShippingSumm;

            db.Entry(order).State = EntityState.Modified;

            db.SaveChanges();
            return RedirectToAction("Details", "Orders", new { id = order.id });
        }

    
        public ActionResult OrderGet (int? id)
        {

            var order = db.Orders.Include(c=>c.Develireries).Include(p=>p.PaymentTypes).SingleOrDefault(x=>x.id==id);
            
            var a = "http://dl-store.ru/Order/OrderComplite?guid=" + order.Guet;
            MailAddress from = new MailAddress("no-reply@digitltech.ru", "Digital Store");
            MailAddress to_custumer = new MailAddress(order.email);
            MailMessage m_costumer = new MailMessage(from, to_custumer);
            m_costumer.Subject = "Информация о заказе на Digital Store!";
      
            SmtpClient smtp = new System.Net.Mail.SmtpClient("smtp.mail.ru", 25);
            smtp.Credentials = new System.Net.NetworkCredential("*", "*");
            smtp.EnableSsl = true;
            m_costumer.IsBodyHtml = true;
     
            switch (order.StatusOrderId)
            {
                case 1:
                    order.StatusOrderId = 2;
                    order.Content += "Заказ принят, звонок покупателю.";
                    m_costumer.Body = string.Format("Уважаемый  " + order.Costumer + " ! " + "</br>"
           + "Ваш заказ" + "№ " + order.NumberOrder + " обработан" + "</br>" + "</hr>"
           + "в ближайщие время с Вами свяжется  менеджер отдела продаж" + "</br>"
           + "Информаци о заказе " + "<a href=" + a + ">Перейти</a>" + "</br>"
           + "</hr>"
           + "сообщенине отправленно автоматически");
                    db.SaveChanges();
                    break;
                case 2:
                    order.StatusOrderId = 3;
                    order.PayOnline = true;
                    order.Content += "Заказ зарезервирован, ожидается оплата";
                    m_costumer.Body = string.Format("Уважаемый  " + order.Costumer + " ! " + "</br>"
+ "Ваш заказ" + "№ " + order.NumberOrder + " Зарезервирован" + "</br>" + "</hr>"
+ "Вы можите оплатить ваш заказ на на сайте, а также любым удобным способом" + "</br>"
+  "<a href=" + a + ">Перейти к оплате </a>" + "</br>"
+ "</hr>"
+ "сообщенине отправленно автоматически");
                    db.SaveChanges();
                    break;

            }
            smtp.Send(m_costumer);
            return RedirectToAction("Details", "Orders", new { id = order.id });
        }


        [HttpPost]
        public JsonResult CancelTransaction(Guid guid)
        {
            string result = " ";
            var order = db.Orders.SingleOrDefault(x => x.Guet == guid);
            var trans_id = order.Amountid;
            if (trans_id != null)
            {

                string path = Server.MapPath("~/ssl/");
                var key = path + "9295108294.key";
                var pem = path + "9295108294.pem";
                var crt = path + "chain-ecomm-ca-root-ca.crt";
                var pkcs = path + "9295108294.pkcs12";
                var url_getTrans = "https://securepay.rsb.ru:9443/ecomm2/MerchantHandler";
            ServicePointManager.ServerCertificateValidationCallback
         += OnValidateCertificate;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url_getTrans);
            request.Method = "POST";
            request.UserAgent = "User-Agent=Mozilla/5.0FireFox/1.0.7";
            request.ContentType = "application/x-www-form-urlencoded";
            X509Certificate2 Cert = new X509Certificate2(pkcs, "9295108294", X509KeyStorageFlags.MachineKeySet);
            request.ClientCertificates.Add(Cert);

            string postData = string.Format("command=r&trans_id={0}&server_version={1}", trans_id, "2.0");


            ASCIIEncoding ascii = new ASCIIEncoding();
            byte[] postBytes = ascii.GetBytes(postData.ToString());

            request.ContentLength = postBytes.Length;
            HttpWebResponse response;

            try
            {
                using (Stream writeStream = request.GetRequestStream())
                {
                    writeStream.Write(postBytes, 0, postBytes.Length);
                }


                using (response = (HttpWebResponse)request.GetResponse())
                {

                    using (Stream readStream = response.GetResponseStream())
                    {
                        using (StreamReader reader = new StreamReader(readStream))
                        {
                            result = reader.ReadToEnd();



                        }
                    }
                }
            }
            catch (WebException ex)
            {

                response = (HttpWebResponse)ex.Response;
                if (response != null) // timeout
                {
                    result = response.StatusCode.ToString();
                }
            }
            }
            else
            {
                result = "trans id не найден";
            }
            return Json(result);
        }
        [HttpPost]
        public JsonResult EndDayTransaction()
        {
            string result = " ";
            string path = Server.MapPath("~/ssl/");
            var key = path + "9295108294.key";
            var pem = path + "9295108294.pem";
            var crt = path + "chain-ecomm-ca-root-ca.crt";
            var pkcs = path + "9295108294.pkcs12";
            var url_getTrans = "https://securepay.rsb.ru:9443/ecomm2/MerchantHandler";
            ServicePointManager.ServerCertificateValidationCallback
         += OnValidateCertificate;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url_getTrans);
            request.Method = "POST";
            request.UserAgent = "User-Agent=Mozilla/5.0FireFox/1.0.7";
            request.ContentType = "application/x-www-form-urlencoded";
            X509Certificate2 Cert = new X509Certificate2(pkcs, "9295108294", X509KeyStorageFlags.MachineKeySet);
            request.ClientCertificates.Add(Cert);

            string postData = string.Format("command=b&server_version=2.0");


            ASCIIEncoding ascii = new ASCIIEncoding();
            byte[] postBytes = ascii.GetBytes(postData.ToString());

            request.ContentLength = postBytes.Length;
            HttpWebResponse response;

            try
            {
                using (Stream writeStream = request.GetRequestStream())
                {
                    writeStream.Write(postBytes, 0, postBytes.Length);
                }


                using (response = (HttpWebResponse)request.GetResponse())
                {

                    using (Stream readStream = response.GetResponseStream())
                    {
                        using (StreamReader reader = new StreamReader(readStream))
                        {
                            result = reader.ReadToEnd();



                        }
                    }
                }
            }
            catch (WebException ex)
            {

                response = (HttpWebResponse)ex.Response;
                if (response != null) // timeout
                {
                    result = response.StatusCode.ToString();
                }
            }

            return Json(result);
        }

        private bool OnValidateCertificate(object sender,
          X509Certificate certificate, X509Chain chain,
           SslPolicyErrors sslPolicyErrors)
        {
            //Console.WriteLine("ServerCertificateValidation!");
            HttpWebRequest requestV = sender as HttpWebRequest;
            if (requestV != null)
            {
                if (requestV.RequestUri.ToString() == "https://testsecurepay.rsb.ru:9443/ecomm2/MerchantHandler")
                {
                    if (sslPolicyErrors != (SslPolicyErrors.RemoteCertificateNameMismatch))
                    {
                        return true;
                    }
                }
                else if (requestV.RequestUri.ToString() != "https://testsecurepay.rsb.ru:9443/ecomm2/MerchantHandler")
                {

                    return true;

                }
                return false;
            }
            return false;
        }
    }
}