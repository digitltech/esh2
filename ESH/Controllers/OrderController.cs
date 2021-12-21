using ESH.Models;
using ESH.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static ESH.Models.ESHDBModels;
using System.Net.Mail;
using System.Data.Entity;
using Fluentx.Mvc;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.IO;
using ESH.Core;
using System.Text;
using System.Web.WebPages;
//using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;

namespace ESH.Controllers
{
    public class OrderController : Controller
    {
        ESHDBContext db = new ESHDBContext();
        // GET: Order
        public ActionResult Index(string develiry, string status, int? idcity)
        {
            if (status == "new")
            {
                var cart = ShoppingCart.GetCart(this.HttpContext);
                var viewModel = new ShoppingCartViewModel
                {
                    CartItems = cart.GetCartItems(),
                    CartTotal = cart.GetTotal()
                };

                //расчет доставки
                var develiryPrice = db.DeveliryPrices.SingleOrDefault(x => x.Id == idcity);

                switch (develiry)
                {
                    case "magaz":
                        if (viewModel.CartTotal < 3000) { ViewBag.DeveliryPrice = develiryPrice.sklad_3000;  }
                        else { ViewBag.DeveliryPrice = develiryPrice.sklad30000; }
                        break;
                    case "kurer":
                        if (viewModel.CartTotal < 3000) { ViewBag.DeveliryPrice = develiryPrice.dver3000; }
                        else { ViewBag.DeveliryPrice = develiryPrice.dver30000; }
                        break;
                   
                }



                if (viewModel.CartTotal < 3000)
                {
                    
                }
                var img = db.Imgs.ToList();
                ViewData["city"] = db.DeveliryPrices.SingleOrDefault(x => x.Id == idcity).City;
                ViewBag.idcity = idcity;
                ViewBag.img = img;
                ViewBag.CartItems = viewModel.CartItems;
                ViewBag.CartTotal = viewModel.CartTotal;
                ViewBag.develiry = develiry;
               
            }
            else
            {
                return HttpNotFound();
            }
            return View();
        }
        [HttpPost]
        public ActionResult GetOrder(Order order, string develiry, int DeveliryPrice, string nameCostumer, string telCostumer,string emailCostumer, string cityRegion, string cityCostumer, string adresCostumer,string content1, string pay,string date_develiry,string time_develiry)
        {
            var num = Core.Helper.GetOrder(this.HttpContext);

            //Costumer costumer = new Costumer();
     
            //    costumer.Name = nameCostumer;
            //    costumer.Phone = telCostumer;
            //    costumer.Email = emailCostumer;
            //    costumer.City = cityRegion + " " + cityCostuner;
            //    costumer.Adress = adresCostumer;
            //    db.Costumers.Add(costumer);
           
            //    db.SaveChanges();
            //    var costumerID = costumer.id;



            //order.CostumerId = null;

            order.Guet = Guid.NewGuid();
            order.NumberOrder = num.GetNumberOrder() + 1;
            order.DateCreate = DateTime.Now;
            order.Content += content1 + "Доставка/ "+develiry + "по адресу: "+ cityRegion +" / "+ cityCostumer + " / " + adresCostumer + " время / "+ content1;
            order.StatusOrderId = 1;
            order.ShippingSumm = DeveliryPrice;
            order.Costumer = nameCostumer;
            order.email = emailCostumer;
            order.contactPhone = telCostumer;




            switch (develiry)
            {
                case "magaz":
                    order.DevelireryId = 1;
                    order.Content+= "Клиент хочет забрать заказ  "+date_develiry+ " в " + time_develiry;
                    
                    break;
                case "kurer":
                    order.DevelireryId = 2;
                    break;
                
            }
      
            switch (pay)
            {
                case "money":
                    order.PaymentTypeId = 1;
                    break;
                case "credit":
                    order.PaymentTypeId = 2;
                    break;
                case "beznal":
                    order.PaymentTypeId = 3;
                    break;
                case "paycart":
                    order.PaymentTypeId = 4;
                    break;
                case "paycredit":
                    order.PaymentTypeId = 5;
                    break;



            }
            if (cityCostumer == "Ульяновск") order.StatusOrderId = 2;

           



            db.Orders.Add(order);
  
            db.SaveChanges();

            var cart = ShoppingCart.GetCart(this.HttpContext);
            order = cart.CreateOrder(order);

            var a = "http://dl-store.ru/Order/OrderComplite?guid=" + order.Guet;

            MailAddress from = new MailAddress("no-reply@digitltech.ru", "Digital Store");
            MailAddress to = new MailAddress("digital@digitltech.ru");
            MailMessage m = new MailMessage(from, to);
            m.Subject = "Новый заказ с сайта!";
            m.Body = string.Format("Поступил новый заказа" + order.NumberOrder + "</br>" + "Покупатель: " + order.Costumer + " | " + "Город " + order.Content + "</br>" + "Сумма заказа " + order.Total + "| " + "Тип оплаты " + pay + "<hr>" + "Тип доставки" + develiry);
            m.IsBodyHtml = true;
            SmtpClient smtp = new System.Net.Mail.SmtpClient("smtp.mail.ru", 25);
            smtp.Credentials = new System.Net.NetworkCredential("*", "*");
            smtp.EnableSsl = true;
            smtp.Send(m);

            MailAddress to_custumer = new MailAddress(order.email);
            MailMessage m_costumer = new MailMessage(from, to_custumer);
            m_costumer.Subject = "Информация о заказе на Digital Store!";
            m_costumer.Body = string.Format("Уважаемый  " + order.Costumer + " ! " + "</br>"
                + "Ваш заказ" + "№ " + order.NumberOrder + " принят" + "</br>" + "</hr>"
                + "в ближайщие время с Вами свяжется  менеджер отдела продаж" + "</br>"
                + "Информаци о заказе " + "<a href=" + a + ">Перейти</a>" + "</br>"
                + "</hr>"
                + "сообщенине отправленно автоматически");
            m_costumer.IsBodyHtml = true;
            smtp.Send(m_costumer);


            return RedirectToAction("OrderComplite", new {guid = order.Guet});


          

   



        }
        //[HttpGet]
        public ActionResult PayOrder(Order order, string trans_id, Guid guid, string result, string resposeFromServer)
        {
          
            if (resposeFromServer!=null)
            {
                if (resposeFromServer != "Error")
                {
                   


                }
                else
                {
                    ViewBag.Message = "Произошла ошибка при выполнении запроса, транзакции не была провизведена, обратитесm в службу поддержки";
       
                    ViewBag.Result = "0";
                }

            }

            else if (guid != null)
            {

                order = db.Orders.Include(p => p.PaymentTypes).Include(s => s.StatusOrders).Include(o => o.OrderDetails).Include(d => d.Develireries).FirstOrDefault(x => x.Guet == guid);
          
                if (order.StatusOrderId != 4)
                {
                    ViewBag.Result = "tr";
                }
                else
                {

                }
            
            }
            else
            {
                ViewBag.Result = "OK";
            }
            return View(order);
        }

        [HttpPost]
        public ActionResult PayOrder(Guid guid)
        {          

      
            string ip = IPNetworking.GetIP4Address();
            var order = db.Orders.SingleOrDefault(x => x.Guet == guid);
            string emailcostum = order.email.Trim(' ');
            string pathtxt = Server.MapPath("~\\TransactionTXT");
            string namefile = Path.Combine(pathtxt, guid + ".txt"); 

            string path = Server.MapPath("~/ssl/");
            var key = path + "9295108294.key";
            var pem = path + "9295108294.pem";
            var crt = path + "chain-ecomm-ca-root-ca.crt";
            var pkcs = path + "9295108294.pkcs12";

            var url_getTrans = "*;


            ServicePointManager.ServerCertificateValidationCallback
             += OnValidateCertificate;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url_getTrans);
            request.Method = "POST";
            request.UserAgent = "User-Agent=Mozilla/5.0FireFox/1.0.7";
            request.ContentType = "application/x-www-form-urlencoded";
            X509Certificate2 Cert = new X509Certificate2(pkcs, "9295108294", X509KeyStorageFlags.MachineKeySet);
            request.ClientCertificates.Add(Cert);

            string postData = string.Format("command=v&amount={0}&currency=643&client_ip_addr={1}&description={2}&mrch_transaction_id=sms&language=ru&msg_type=SMS&server_version=2.0&ecomm_notify_email={3}", order.TotalSumm+"00", ip,"Oplata zakaza # "+order.NumberOrder, emailcostum + ","+"digital@digitltech.ru");


            ASCIIEncoding ascii = new ASCIIEncoding();
            byte[] postBytes = ascii.GetBytes(postData.ToString());

            request.ContentLength = postBytes.Length;
            HttpWebResponse response;
            string resposeFromServer = " " ;
            string errormessage = " ";




            try
            {
                using (Stream writeStream = request.GetRequestStream())
                {
                    writeStream.Write(postBytes, 0, postBytes.Length);
                }


                using ( response = (HttpWebResponse)request.GetResponse())
                {
                    using (Stream readStream = response.GetResponseStream())
                    {
                        using (StreamReader reader = new StreamReader(readStream))
                        {
                           
                            resposeFromServer = reader.ReadToEnd();
                            var trans = resposeFromServer.Substring(16);
                          string  trans_id = trans.Substring(0, trans.Length - 25);


                            Directory.CreateDirectory(pathtxt);
                            System.IO.File.WriteAllText(namefile, resposeFromServer + "ip: "+ip);

                            if (resposeFromServer.StartsWith("TRANSACTION_ID: ") == true)

                            {
                               pathtxt = Server.MapPath("~\\TransactionTXT");
                                namefile = Path.Combine(pathtxt, "TransOK.txt");
                              
                                using (StreamWriter sw = new StreamWriter(namefile, false, System.Text.Encoding.Default))
                                {
                                    sw.WriteLine(trans_id);
                                    
                                }

                                order.Amountid = trans_id;
                                db.SaveChanges();
                                return Redirect("*" + trans_id);

                            }
                            else
                            {
                                errormessage = resposeFromServer;
                                resposeFromServer = "Error";
                             
                            }
                           
                        }
                    }
                }
            }
            catch (WebException ex)
            {
        
                response = (HttpWebResponse)ex.Response;
                if (response != null) // timeoutR
                {
                    resposeFromServer = response.StatusCode.ToString();
                }
            }
          
         

         

            return RedirectToAction("PayOrder", new { guid = guid, resposeFromServer = resposeFromServer, errormessage = errormessage });


        }

        public ActionResult Paid(string return_url) 
        {

            ViewBag.Message = return_url;


            return View();

            
        }
        [HttpPost]
        public ActionResult Paid()
        {
            string ip = IPNetworking.GetIP4Address();
            string pathtxt = Server.MapPath("~\\TransactionTXT");
            string namefile = Path.Combine(pathtxt, "TransOK.txt");
            string trans_id = " ";
            string result = "";
            var result_message = " ";
            Directory.CreateDirectory(pathtxt);
            string path = Server.MapPath("~/ssl/");
            var key = path + "9295108294.key";
            var pem = path + "9295108294.pem";
            var crt = path + "chain-ecomm-ca-root-ca.crt";
            var pkcs = path + "9295108294.pkcs12";
            using (StreamReader sr = new StreamReader(namefile, System.Text.Encoding.Default))
            {
                trans_id = sr.ReadLine();
              


            }

            using (StreamWriter sw = new StreamWriter(namefile, false, System.Text.Encoding.Default))
            {
                sw.Write(" ");
                sw.Close();
            }


            var order = db.Orders.SingleOrDefault(x => x.Amountid == trans_id);
            var url_getTrans = "*";
            ServicePointManager.ServerCertificateValidationCallback
         += OnValidateCertificate;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url_getTrans);
            request.Method = "POST";
            request.UserAgent = "User-Agent=Mozilla/5.0FireFox/1.0.7";
            request.ContentType = "application/x-www-form-urlencoded";
            X509Certificate2 Cert = new X509Certificate2(pkcs, "9295108294", X509KeyStorageFlags.MachineKeySet);
            request.ClientCertificates.Add(Cert);

            string postData = string.Format("command=c&trans_id={0}&client_ip_addr={1}&server_version={2}", trans_id, ip, "2.0");


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
                            order.Amountid = result;
                            //DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(MyThingy));

                            //RESULT: FAILED RESULT_PS: CANCELLED RESULT_CODE: 116 RRN: 716617343430 APPROVAL_CODE: 201836 CARD_NUMBER: 5 * **********5471 MRCH_TRANSACTION_ID: sms
                            string[] result_temp = result.Split(new char[] { '\n' });
                            foreach(string i in result_temp)
                            {
                                if (i == "RESULT: OK") result_message = "OK";
                                if (i == "RESULT_PS: FINISHED") order.AmountSum = order.TotalSumm; order.StatusOrderId = 4;
                            }
                       
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
            if (result_message == "OK")
            { ViewBag.message = "Транзакция произведина успешно, статус оплаты вы можете отследить на страници заказа";
                DateTime now = DateTime.Now;
                order.AmountDate = now.ToString("yyyy-MM-ddTHH:mm:ss.fff");
                order.Content += result;
                order.Amountid = trans_id;
           
                db.SaveChanges();
                return RedirectToAction("OrderComplite", new { guid = order.Guet });


            }
            else
            { ViewBag.message = "Транзакция не выполнена. Произошла ошибка при выполнение операции."; }
            return View();
        }
    
        public ActionResult CreditRSB(Guid guid)
        {
            var order = db.Orders.SingleOrDefault(x => x.Guet == guid);
            var orderdetails = db.OrderDetails.Where(x => x.OrderId == order.id);
            int count = orderdetails.Count();
            var parametr = "";
            Translit translit = new Translit();


            int i = 1;
            foreach (var item in orderdetails)
            {
                
                if (i <= count)
                {
                    parametr+="&TCount="+count+"&TC_"+i+"="+item.Quantity+"&TPr_"+i+"="+item.Products.Price+"&TName_"+i+"="+ translit.TranslitFileName(item.Products.Name);
                    i++;
                }
            }


            return Redirect("*"+order.NumberOrder+ parametr+"&UserLastName=&UserFatherName=&UserBirthday=&UserMail=&Spassport=&Npassport=&NPhone="+order.contactPhone);
        }

        [HttpPost]
        public ActionResult CreditGetOrder (Order order, int productid, string name, string Familia, string Otchestco, string DateRozhdenia,string Spasport, string Npasport, string tel, string email, string city)
        {
            var product = db.Products.SingleOrDefault(x => x.id == productid);
            if (product != null)
            {
                var num = Core.Helper.GetOrder(this.HttpContext);

                Costumer costumer = new Costumer();

                costumer.Name = name + " "+Familia + " " + Otchestco;
                costumer.Phone = tel;
                costumer.Email = email;
                costumer.City = city;
                costumer.Adress = " ";
                db.Costumers.Add(costumer);

                db.SaveChanges();
                var costumerID = costumer.id;

                order.CostumerId = costumerID;

                order.Guet = Guid.NewGuid();
                order.NumberOrder = num.GetNumberOrder() + 1;
                order.DateCreate = DateTime.Now;
                order.Content += " Онлайн заявка на кредит";
                order.Content += " паспорт" + Spasport + "/" + Npasport;
                    order.StatusOrderId = 1;
                order.DevelireryId = 1;
                order.PaymentTypeId = 5;
                order.TotalSumm = product.Price;
                order.Total = product.Price;
                db.Orders.Add(order);

                OrderDetails orderdetails = new OrderDetails();
                orderdetails.OrderId = order.id;
                orderdetails.ProductId = product.id;
                orderdetails.Quantity = 1;
                orderdetails.UnitPrice = product.Price;
                db.OrderDetails.Add(orderdetails);
                db.SaveChanges();
                var idpost = product.ArticlePos;
                var token = Core.GetFromNetlabs.GetToken();
                var ostatok = Core.GetFromNetlabs.GetOstatok(token, idpost);

                if (product.PaymentOnline==true & ostatok!= 0 )
                {
                    if (city == "г. Ульяновск")
                    {
                        order.PayOnline = true;
                        order.StatusOrderId = 2;
                        Translit translit = new Translit();
                        var parametr = "";
                        parametr += "&TCount=" + orderdetails.Quantity + "&TC_1=" + orderdetails.Quantity + "&TPr_1" + "=" + orderdetails.Products.Price + "&TName_1=" + translit.TranslitFileName(product.Name);


                        MailAddress from = new MailAddress("no-reply@digitltech.ru", "Digital Store");
                        MailAddress to = new MailAddress("digital@digitltech.ru");
                        MailMessage m = new MailMessage(from, to);
                        m.Subject = "Новый кредит заявка";
                        m.Body = string.Format("Поступил новый заказа" + order.NumberOrder + "</br>" + "Покупатель: " + order.Costumer + " | " + "Город " + order.Content+ "</br>" + "Сумма заказа " + order.Total + "| " + "Тип оплаты  онлайн креди" + "<hr>");
                        m.IsBodyHtml = true;
                        SmtpClient smtp = new System.Net.Mail.SmtpClient("smtp.mail.ru", 25);
                        smtp.Credentials = new System.Net.NetworkCredential("*", "*");
                        smtp.EnableSsl = true;
                        smtp.Send(m);

                        return Redirect("*" + order.NumberOrder + parametr + "&UserName=" + name + "&UserLastName=" + Familia + "&UserFatherName=" + Otchestco + "&UserBirthday=" + DateRozhdenia + "&UserMail=" + email + "&Spassport=" + Spasport + "&Npassport=" + Npasport + "&NPhone=" + order.contactPhone);

                    }
                    else
                    {
                        return RedirectToAction("OrderComplite", new { guid = order.Guet });
                    }
                }
            }
                

            return HttpNotFound();
        }





        private bool OnValidateCertificate(object sender,
           X509Certificate certificate, X509Chain chain,
            SslPolicyErrors sslPolicyErrors)
        {
            //Console.WriteLine("ServerCertificateValidation!");
            HttpWebRequest requestV = sender as HttpWebRequest;
            if (requestV != null)
            {
                if (requestV.RequestUri.ToString() == "*")
                {
                    if (sslPolicyErrors != (SslPolicyErrors.RemoteCertificateNameMismatch))
                    {
                        return true;
                    }
                }
                else if (requestV.RequestUri.ToString() != "*")
                {
                   
                        return true;
                    
                }
                return false;
            }
            return false;
        }
        [HttpGet]
        public ActionResult PayOrderMethod(Guid guid)
        {
            return View();

            
        }
        public ActionResult OrderComplite(Guid guid)
        {
            Order order = db.Orders.Include(p=>p.PaymentTypes).Include(s=>s.StatusOrders).Include(o=>o.OrderDetails).Include(d=>d.Develireries).FirstOrDefault(x => x.Guet == guid);
            if (order != null)
            {
               
                ViewBag.Status = order.StatusOrderId;
                return View(order);

           
                
            }

            return HttpNotFound();
           
        }
        [HttpGet]
        public ActionResult Fast(int productId)
        {
            var product = db.Products.Single(p => p.id == productId);
            if (product != null)
            {
                return View(product);
            }
            return View();
                
        }

    }
}