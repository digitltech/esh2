using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static ESH.Models.ESHDBModels;
using System.Data.Entity;
using ESH.Core;
using System.Net.Mail;
using ESH.Models;

namespace ESH.Controllers
{
    public class HomeController : Controller
    {
        ESHDBContext db = new ESHDBContext();
        public ActionResult Index()
        {
            var img = db.Imgs.ToList();
            ViewBag.img = img;
            var proizvoditel = db.Properties.Where(p => p.PropertyTypes.id == 1).Include(p => p.PropertyTypes).Include(p => p.PropertyValues).ToList();
            ViewBag.proizvoditel = proizvoditel;

            var cat = db.Categories.ToList();
            ViewBag.cat = cat;
            var stock = db.Stocks.Include(s => s.Products).Include(s => s.StockTypes).Where(s => s.StockTypes.id != 4).ToList();

            var StockRassrochka = db.Stocks.Include(p => p.Products).Include(s => s.StockTypes).Where(x => x.StockTypeid == 1).OrderBy(p => p.Products.Price).Take(4).ToList();
            ViewBag.StockRassrochka = StockRassrochka;

            var stockSkidki = db.Stocks.Include(p => p.Products).Include(s => s.StockTypes).Where(x => x.StockTypeid == 2).OrderBy(p => p.Products.Price).Take(4).ToList();
            ViewBag.stockSkidki = stockSkidki;

            var stockNovinki = db.Stocks.Include(p => p.Products).Include(s => s.StockTypes).Where(x => x.StockTypeid == 3).OrderBy(p => p.Products.Price).Take(4).ToList();
            ViewBag.stockNovinki = stockNovinki;
            var hits = db.Stocks.Include(s => s.Products).Where(s => s.StockTypes.id == 4);
            ViewBag.Hits = hits;
            var JoinProduct = db.JoinProducts.Include(c => c.Products);

            ViewBag.JoinProduct = JoinProduct;


            return View(stock);
        }
        public ActionResult Sitemap()
        {
            Generator sitemapGenerator = new Generator();
            var sitemapNodes = sitemapGenerator.GetSitemapNodes(this.Url);
            string xml = sitemapGenerator.GetSitemapDocument(sitemapNodes);
            return this.Content(xml, "text/xml", System.Text.Encoding.UTF8);
        }

        public ActionResult Robots()
        {
            Response.ContentType = "text/plain";
            return View();
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {


            return View();
        }
        public ActionResult Garantii()
        {
            return View();
        }
        public ActionResult Vozvrat()
        {
            return View();
        }
        public ActionResult Credit()
        {
            return View();
        }
        public ActionResult Stock(int? id)
        {
            if (id != null)
            {
                var cat = db.Categories.ToList();
                ViewBag.cat = cat;
                var img = db.Imgs.ToList();
                ViewBag.img = img;
                var stock = db.Stocks.Include(s => s.Products).Include(s => s.StockTypes).Where(x => x.StockTypeid == id).ToList();

                var JoinProduct = db.JoinProducts.Include(c => c.Products);

                ViewBag.JoinProduct = JoinProduct;

                switch (id)
                {
                    case 1:
                        ViewBag.NameStore = "Товары в рссрочку";
                        ViewBag.Text = "Рекламная акция «Рассрочка без переплат» действует при оформлении кредита в магазине Digital STORE. Кредит предоставляет Банк Русский Стандарт, Генеральная лицензия Банка России № 2289 выдана бессрочно 19 ноября 2014 года. Первоначальный взнос – 0% от цены товара, срок кредитования –  10 и 24 месяца. Процентная ставка 11% годовых, полная стоимость кредита составляет 23.6% годовых, сумма кредита от 3 000 до 300 000 руб. Организатор акции предоставляет скидку на товар, в итоге сумма, подлежащая выплате банку, не превышает стоимости товара без учета акции товара (зачеркнутой цены) или первоначальные стоимости товара при условии, что дополнительные услуги банка не приобретаются. Не является публичной офертой. Банк самостоятельно принимает решение о предоставлении либо отказе в предоставлении кредита без объяснения причин.Список товаров ограничен.Подробности приобретения товаров в кредит в рамках акции уточняйте у сотрудников магазинах «Digital STORE». Организатор акции и продавец: ИП Миннияров Т.И.";

                    

                        break;
                    case 2:
                        ViewBag.NameStore = "Товары со скидкой";
                        ViewBag.Text = "";
                        break;
                    case 3:
                        ViewBag.NameStore = "Новинки";
                        ViewBag.Text = "";
                        break;

                    case 4:
                        ViewBag.NameStore = "Хиты продаж";
                        ViewBag.Text = "";
                        break;
                    case 5:
                        ViewBag.NameStore = "Товары со скидкой недели";
                        ViewBag.Text = "Спец предложение от Digital STORE. Только с 25.09.17 по 30.09.2017 скидка на все ноутбуки Asus до 30%. Скидка распространяется при заказе через интернет магазин. Рассрочка не весь перечень товаров по данной акции не распространяется.   ";
                        break;

                }
                return View(stock);
            }

            else
            {
                return HttpNotFound();
            }

        }
        [HttpPost]
        public ActionResult AutocompleteSearch(string name)
        {
            var models = db.Products.Where(a => a.Name.Contains(name))
                            .Select(a => new { value = a.Name })
                            .Distinct();

            return Json(models, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SendContact(string name, string email, string tel, string text, int model, string city, string develiry, string pay, Order order, OrderDetails orderdetails)
        {
            if (model == 0)
            {
                MailAddress from = new MailAddress("no-reply@digitltech.ru", "Digital Store");
                MailAddress to = new MailAddress("digital@digitltech.ru");
                MailMessage m = new MailMessage(from, to);
                m.Subject = "Обрашение с сайта!";
                m.Body = string.Format("Покупатель" + name + "<br>" + "Телефон" + tel + "<br>" + "Почта " + email + "<br>" + "Текст обрашения " + text
                    + "<br>" + "<hr/>" + model);
                m.IsBodyHtml = true;
                SmtpClient smtp = new System.Net.Mail.SmtpClient("smtp.mail.ru", 25);
                smtp.Credentials = new System.Net.NetworkCredential("*", "*");
                smtp.EnableSsl = true;
                smtp.Send(m);
                return RedirectToAction("Index");
            }
            else
            {
                var product = db.Products.Find(model);
                var num = Core.Helper.GetOrder(this.HttpContext);

               
       
               

                order.Guet = Guid.NewGuid();
                order.NumberOrder = num.GetNumberOrder() + 1;
                order.DateCreate = DateTime.Now;
                order.Content += "Быстрый закзаз" + "город /"+city;
                order.Costumer = name;
                order.contactPhone = tel;
                order.email = email;
                order.DevelireryId = 1;
                order.PaymentTypeId = 4;
                order.StatusOrderId = 1;
                db.Orders.Add(order);





                orderdetails.ProductId = product.id;
                orderdetails.Quantity = 1;
                orderdetails.UnitPrice = product.Price;
                orderdetails.OrderId = order.id;
                db.OrderDetails.Add(orderdetails);
                order.TotalSumm = orderdetails.UnitPrice;


                db.SaveChanges();

                var countProduct = product.Ostatok;
          
                MailAddress from = new MailAddress("no-reply@digitltech.ru", "Отдел продаж Digital Store");
                MailAddress to = new MailAddress(email);
                MailMessage m = new MailMessage(from, to);
                m.Subject = "Ваш заказ принят!";
                m.Body = string.Format("Уважаемый" + name + "<br>" + "Ваш заказ принят" + "<br>" + "В ближайшие время с Вами свяжется менеджер отдела продаж для уточнения деталий заказа ");
                m.IsBodyHtml = true;
                SmtpClient smtp = new System.Net.Mail.SmtpClient("smtp.mail.ru", 25);
                smtp.Credentials = new System.Net.NetworkCredential("*", "*");
                smtp.EnableSsl = true;
                smtp.Send(m);


                MailAddress to2 = new MailAddress("digital@digitltech.ru");
                MailMessage m2 = new MailMessage(from, to2);
                m2.Subject = "Заказ с сайта без оформления";
                m2.Body = string.Format("Покупатель" + name + "<br>" + "Телефон" + tel + "<br>" + "Почта " + email + "<br>" + "Текст обрашения " + text
                    + "<br>" + "<hr/>" + "Продукт " + model);
                m2.IsBodyHtml = true;



                smtp.Send(m2);
                return RedirectToAction("Index");
            }

        }

        public ActionResult Kakzakazat()
        {
            return View();
        }


        public ActionResult Search(string name, int? page, SortProduct sortcatalog = SortProduct.NameAsc)
        {
            var img = db.Imgs.ToList();
            ViewBag.img = img;
            ViewData["NameSort"] = sortcatalog == SortProduct.NameAsc ? SortProduct.NameDesc : SortProduct.NameAsc;
            ViewData["PriceSort"] = sortcatalog == SortProduct.PriceAsc ? SortProduct.PriceDesc : SortProduct.PriceAsc;
            var result = db.Products.Include(p => p.Category).Where(x => x.Name.Contains(name)).ToList();
                

            //int pageSize = 30;
            //int pageNumber = (page ?? 1);
            //var productlist = result.OrderBy(s => s);
            if (result != null)
            {
                ViewBag.result = result;
                return View();
            }
            else
            {

                return View();


            }
        }
    }
}