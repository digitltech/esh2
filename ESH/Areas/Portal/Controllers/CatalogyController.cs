using ESH.Core;
using ESH.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static ESH.Models.ESHDBModels;
using Excel = Microsoft.Office.Interop.Excel;
using System.IO;
using System.Xml;
using System.Net;
using Newtonsoft.Json;

namespace ESH.Areas.Portal.Controllers
{
    public class CatalogyController : Controller
    {
        ESHDBContext db = new ESHDBContext();

        public ActionResult List()
        {
            return View(db.Products.Include(p => p.SkladTypes));
        }

        public ActionResult FilterCategoryList(int? category, int? manufacture)
        {

            var cat = db.Categories.ToList();
            ViewBag.cat = cat;
            var man = db.Manufacturers.ToList();
            ViewBag.man = man;
            if (category != null && manufacture!=null)
            {
                var products = db.Products.Where(x=>x.CategoryId==category).Where(x=>x.ManufacturerId==manufacture).ToList();
                ViewBag.product = products;
                var img = db.Imgs.ToList();
                ViewBag.img = img;
            }
            else if(category!=null && manufacture == null)
            {
                var products = db.Products.Where(x => x.CategoryId == category).ToList();
                ViewBag.product = products;
                var img = db.Imgs.ToList();
                ViewBag.img = img;
            }
            return View();
        }

        [HttpGet]
        public ActionResult Add()
        {
            SelectList cat = new SelectList(db.Categories, "Id", "Name");
            ViewBag.cat = cat;
            return View();
        }

        [HttpPost]
        public ActionResult Add(Product product)
        {
            product.DateUpdate = DateTime.Today;
            db.Products.Add(product);
            db.SaveChanges();
            return RedirectToAction("Details", "Catalogy", new { id = product.id });
        }

        [HttpGet]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            var property = db.Properties.Include(p => p.PropertyTypes).Include(p => p.PropertyValues).Include(p => p.Products).Where(p => p.Products.id == id).ToList();
            ViewBag.Property = property;
            var imdmass = db.Imgs.Where(i => i.ProductId == id).ToList();

            ViewBag.img = imdmass;


            var product = db.Products.Find(id);
            if (product != null)
            {
                SelectList cat = new SelectList(db.Categories, "Id", "Name", product.id);
                ViewBag.cat = cat;
                SelectList SkladType = new SelectList(db.SkladTypes, "id", "Name", product.id);
                ViewBag.SkladType = SkladType;
                SelectList Manufacturers = new SelectList(db.Manufacturers, "id", "Name", product.id);
                ViewBag.Manufacturers = Manufacturers;
                SelectList PriceType = new SelectList(db.PriceTypes, "id", "Type", product.id);
                ViewBag.PriceType = PriceType;
                var sklad = db.Sklads.Include(p => p.Products).FirstOrDefault(p => p.ProductId == id);
                if (sklad != null) { ViewData["sklad"] = sklad.Count; }
                var stock = db.Stocks.Include(s => s.StockTypes).SingleOrDefault(x => x.ProductId == product.id);
                if (stock != null) ViewBag.Stock = stock.StockTypes.Name;
                return View(product);
            }
            return HttpNotFound();


        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult DetailsSafeEdit(Product product, Stock stocks, int sklad, string stock)
        {
            Translit translit = new Translit();
            if (sklad > 0)
            {
                Sklad tabsklad = new Sklad();
                var tempsklad = db.Sklads.FirstOrDefault(s => s.ProductId == product.id);
                if (tempsklad == null)
                {
                    tabsklad.ProductId = product.id;
                    tabsklad.Count = sklad;
                    tabsklad.Content = "Добавлено администратором" + DateTime.Today;
                    db.Sklads.Add(tabsklad);
                    db.SaveChanges();
                }
                else
                {
                    tempsklad.Count = sklad;
                    tabsklad.Content = "Добавлено администратором" + DateTime.Today;
                    db.SaveChanges();
                }

            }

            switch (stock)
            {
                case "Нет":

                    break;

                case "Рассрочка":
                    stocks.ProductId = product.id;
                    stocks.StockTypeid = 1;
                    db.Stocks.Add(stocks);
                    break;

                case "Низкая цена":
                    stocks.ProductId = product.id;
                    stocks.StockTypeid = 2;
                    db.Stocks.Add(stocks);
                    break;

                case "Новинка":
                    stocks.ProductId = product.id;
                    stocks.StockTypeid = 3;
                    db.Stocks.Add(stocks);
                    break;
                case "Хиты":
                    stocks.ProductId = product.id;
                    stocks.StockTypeid = 4;
                    db.Stocks.Add(stocks);
                    break;
                case "Убрать":
                    var tempstock = db.Stocks.SingleOrDefault(x => x.ProductId == product.id);
                    if (tempstock != null) db.Stocks.Remove(tempstock);
                    break;





            }


            product.Url = translit.TranslitFileName(product.Name) + product.Article;
            product.MetaKey = "Купить " + product.Name + product.Article;
            product.MetaDescription = product.Name + " " + product.Descriptions;
            product.DateUpdate = DateTime.Today;
            db.Entry(product).State = EntityState.Modified;

            db.SaveChanges();
            return RedirectToAction("Details", new { id = product.id });
        }
        [HttpGet]
        public ActionResult Delete(int id)
        {
            Product p = db.Products.Find(id);
            if (p == null)
            {
                return HttpNotFound();
            }
            return View(p);
        }
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Product p = db.Products.Include(c => c.Category).FirstOrDefault(i => i.id == id);
            var img = db.Imgs.Where(x => x.ProductId == id);
            var property = db.Properties.Where(x => x.ProductId == id);
            if (p == null)
            {
                return HttpNotFound();
            }


            var cat = db.Categories.SingleOrDefault(x => x.id == p.CategoryId);
            if (img.Count() > 0)
            {
                foreach (var i in img)
                {
                    db.Imgs.Remove(i);
                }
                string path = Server.MapPath("~/Files/Images/Catalog/" + p.Category.URL + "/" + p.Article);
                if (System.IO.Directory.Exists(path))
                {
                    System.IO.DirectoryInfo di = new DirectoryInfo(path);
                    foreach (FileInfo file in di.GetFiles())
                    {
                        file.Delete();
                    }
                    foreach (DirectoryInfo dir in di.GetDirectories())
                    {
                        dir.Delete(true);
                    }


                }
            }
            if (p.OneImg == true)
            {
                string path = Server.MapPath("~/Files/Images/Catalog/" + p.Category.URL + "/" + p.Article);
                if (System.IO.Directory.Exists(path))
                {
                    System.IO.DirectoryInfo di = new DirectoryInfo(path);
                    foreach (FileInfo file in di.GetFiles())
                    {
                        file.Delete();
                    }
                    foreach (DirectoryInfo dir in di.GetDirectories())
                    {
                        dir.Delete(true);
                    }
                }
            }
            if (property.Count() > 0)
            {
                foreach (var i in property)
                {
                    db.Properties.Remove(i);
                }
            }
            var propertyall = db.PropertyAlls.Where(x => x.ProductId == p.id);
                
                if (propertyall.Count() > 0)
                {
                    foreach (var i in propertyall)
                    {
                    db.PropertyAlls.Remove(i);
                    }
                }

            
            db.Products.Remove(p);
            db.SaveChanges();
            return RedirectToAction("List");
        }
        [HttpPost]
        public JsonResult GetURL(int id)
        {
            var product = db.Products.Find(id);
            Translit translit = new Translit();

            product.Url = translit.TranslitFileName(product.Name);
            product.MetaKey = "Купить " + product.Name;
            product.MetaDescription = product.Name + " " + product.Descriptions;

            db.SaveChanges();

            return Json("Данные обновлены");
        }

        [HttpPost]
        public JsonResult UploadImg(int id, Img img)
        {

            foreach (string file in Request.Files)
            {
                var upload = Request.Files[file];
                if (upload != null)
                {
                    string fileName = System.IO.Path.GetFileName(upload.FileName);

                    var product = db.Products.FirstOrDefault(p => p.id == id);
                    var cat = db.Categories.Where(c => c.id == product.CategoryId).FirstOrDefault();
                    string path = Server.MapPath("~/Files/Images/Catalog/" + cat.URL + "/" + product.Article + "/");

                    try
                    {

                        upload.SaveAs(Server.MapPath("~/Files/Images/Catalog/" + cat.URL + "/" + product.Article + "/" + fileName));

                        img.ProductId = product.id;
                        img.img = fileName;
                        img.path_img = "/Files/Images/Catalog/" + cat.URL + "/" + product.Article + "/";


                        db.Imgs.Add(img); db.SaveChanges();
                    }
                    catch
                    {
                        System.IO.Directory.CreateDirectory(Server.MapPath("~/Files/Images/Catalog/" + cat.URL + "/" + product.Article + "/"));

                        upload.SaveAs(Server.MapPath("~/Files/Images/Catalog/" + cat.URL + "/" + product.Article + "/" + fileName));
                        img.ProductId = product.id;

                        img.img = fileName;
                        img.path_img = "/Files/Images/Catalog/" + cat.URL + "/" + product.Article + "/";
                        db.Imgs.Add(img); db.SaveChanges();

                    }


                }
            }

            return Json("Загружен");
        }
        [HttpPost]
        public JsonResult UploadOneImg(int id)
        {

            foreach (string file in Request.Files)
            {
                var upload = Request.Files[file];
                if (upload != null)
                {
                    string fileName = System.IO.Path.GetFileName(upload.FileName);

                    Product product = db.Products.SingleOrDefault(x => x.id == id);
                    var cat = db.Categories.Where(c => c.id == product.CategoryId).FirstOrDefault();
                    string path = Server.MapPath("~/Files/Images/Catalog/" + cat.URL + "/" + product.Article + "/");

                    try
                    {

                        upload.SaveAs(Server.MapPath("~/Files/Images/Catalog/" + cat.URL + "/" + product.Article + "/" + fileName));



                        product.Img = "/Files/Images/Catalog/" + cat.URL + "/" + product.Article + "/" + fileName;
                        product.OneImg = true;

                        db.SaveChanges();
                    }
                    catch
                    {
                        System.IO.Directory.CreateDirectory(Server.MapPath("~/Files/Images/Catalog/" + cat.URL + "/" + product.Article + "/"));

                        upload.SaveAs(Server.MapPath("~/Files/Images/Catalog/" + cat.URL + "/" + product.Article + "/" + fileName));


                        product.OneImg = true;
                        product.Img = "/Files/Images/Catalog/" + cat.URL + "/" + product.Article + "/" + fileName;
                        db.SaveChanges();

                    }


                }
            }

            return Json("Загружен");
        }
        [HttpGet]
        public ActionResult PropertyProductAdd(int? productId, int? Catid)
        {
            var cat = db.Categories.FirstOrDefault(c => c.id == Catid);
            SelectList property_type = new SelectList(db.PropertyTypes.Where(c => c.CategoryId == cat.ParentId || c.CategoryId == 7), "id", "Name");
            ViewBag.Property_type = property_type;
            SelectList property_value = new SelectList(db.PropertyValues, "id", "Name");
            ViewBag.property_Value = property_value;
            ViewBag.Product = productId;
            return View();
        }
        [HttpPost]
        public ActionResult PropertyProductAdd(Property property, int prodid)
        {
            property.ProductId = prodid;
            db.Properties.Add(property);
            db.SaveChanges();
            return RedirectToAction("Details", new { id = prodid });

        }

        [HttpGet]
        public ActionResult ImportPrice()
        {
            var cat = db.Categories.ToList();
            ViewBag.cat = cat;
            var PriceType = db.PriceTypes.ToList();
            ViewBag.price = PriceType;
            var manufacture = db.Manufacturers.ToList();
            ViewBag.manufacture = manufacture;
            return View();
        }
        [HttpPost]
        public ActionResult ImportPrice(int? catalogy, HttpPostedFileBase excelfile, int? nacenka, int? manufacture)
        {
            var cat = db.Categories.ToList();
            ViewBag.cat = cat;
            var PriceType = db.PriceTypes.ToList();
            ViewBag.price = PriceType;
            var manufactur = db.Manufacturers.ToList();
            ViewBag.manufacture = manufactur;
            if (excelfile == null || excelfile.ContentLength == 0)
            {
                ViewBag.Error = "Фаил не выбран";

                return View();
            }
            else
            {
                if (excelfile.FileName.EndsWith("xls") || excelfile.FileName.EndsWith("xlsx"))
                {
                    int i;
                    string filename = System.IO.Path.GetFileName(excelfile.FileName);
                    var path = Server.MapPath("~/Files");
                    if (System.IO.File.Exists(path + filename))
                        System.IO.File.Delete(path + filename);
                    excelfile.SaveAs(path + filename);

                    Excel.Application appplication = new Excel.Application();
                    Excel.Workbook workbook = appplication.Workbooks.Open(path + filename);
                    Excel.Worksheet worksheet = workbook.ActiveSheet;
                    Excel.Range range = worksheet.UsedRange;
                    List<Product> listProduct = new List<Product>();
                    for (int row = 1; row < range.Rows.Count; row++)
                    {
                        if (((Excel.Range)range.Cells[row, 1]).Text != "Доп.")
                        {
                            string product_article = ((Excel.Range)range.Cells[row, 4]).Text;
                            string product_idpost = ((Excel.Range)range.Cells[row, 2]).Text;
                            var product = db.Products.Where(x => x.ArticlePos == product_idpost).SingleOrDefault();


                            if (product != null)
                            {
                                if (manufacture != null)
                                {
                                    product.ManufacturerId = manufacture;
                                }
                                product.PriceOpt = Math.Floor(System.Convert.ToDecimal(((Excel.Range)range.Cells[row, 10]).Text));
                                var price = System.Convert.ToDecimal(((Excel.Range)range.Cells[row, 11]).Text);


                                switch (nacenka)
                                {
                                    case 1:
                                        i = Convert.ToInt32(price);
                                        if (price < 3000)
                                        { if (i % 10 != 0) i = (i / 10) * 10 + 10; }
                                        else
                                        {
                                            if (i % 100 != 0) i = (i / 100) * 100 + 100;
                                        }

                                        product.Price = i;
                                        product.PriceTypiesId = 1;

                                        break;
                                    case 2:
                                        var i2 = System.Convert.ToInt32(product.PriceOpt);
                                        var procent = (i2 / 100) * 20;
                                        i = procent + Convert.ToInt32(product.PriceOpt);
                                        if (price < 3000)
                                        { if (i % 10 != 0) i = (i / 10) * 10 + 10; }
                                        else
                                        {
                                            if (i % 100 != 0) i = (i / 100) * 100 + 100;
                                        }
                                        product.Price = i;
                                        product.PriceTypiesId = 2;


                                        break;
                                    case 3:
                                        var i3 = System.Convert.ToInt32(product.PriceOpt);
                                        procent = (i3 / 100) * 10;
                                        i = procent + Convert.ToInt32(product.PriceOpt);
                                        if (price < 3000)
                                        { if (i % 10 != 0) i = (i / 10) * 10 + 10; }
                                        else
                                        {
                                            if (i % 100 != 0) i = (i / 100) * 100 + 100;
                                        }
                                        product.Price = i;
                                        product.PriceTypiesId = 3;
                                        break;
                                    case 4:
                                        var i4 = System.Convert.ToInt32(product.PriceOpt);
                                        procent = (i4 / 100) * 5;
                                        i = procent + Convert.ToInt32(product.PriceOpt);
                                        //if (price < 3000)
                                        //{ if (i % 10 != 0) i = (i / 10) * 10 + 10; }
                                        //else
                                        //{
                                        //    if (i % 100 != 0) i = (i / 100) * 100 + 100;
                                        //}
                                        product.Price = i;
                                        product.PriceTypiesId = 4;

                                        break;
                                    case 5:

                                        product.PriceTypiesId = 5;
                                        product.Price = price;
                                        break;
                                }


                                product.Ostatok = System.Convert.ToInt32(((Excel.Range)range.Cells[row, 7]).Text) + System.Convert.ToInt32(((Excel.Range)range.Cells[row, 6]).Text);
                                if (product.Ostatok == 0) product.SkladTypesId = 4;
                                product.CategoryId = catalogy;
                                product.DateUpdate = DateTime.Today;
                                if (product.Article == null) product.Article = product.ArticlePos;
                                db.Entry(product).State = EntityState.Modified;

                                // xml



                                //
                                db.SaveChanges();



                            }
                            else
                            {
                                Product p = new Product();
                                if (manufacture != null)
                                {
                                    p.ManufacturerId = manufacture;
                                }
                                p.Article = ((Excel.Range)range.Cells[row, 4]).Text;
                                p.ArticlePos = ((Excel.Range)range.Cells[row, 2]).Text;
                                p.Name = ((Excel.Range)range.Cells[row, 3]).Text;
                                p.Url = ((Excel.Range)range.Cells[row, 4]).Text;
                                p.PriceOpt = Math.Floor(System.Convert.ToDecimal(((Excel.Range)range.Cells[row, 10]).Text));
                                p.OneImg = true;
                                p.Img = "/Files/noimage.png";
                                var price = System.Convert.ToDecimal(((Excel.Range)range.Cells[row, 11]).Text);

                                switch (nacenka)
                                {
                                    case 1:
                                        i = Convert.ToInt32(price);
                                        if (price < 3000)
                                        { if (i % 10 != 0) i = (i / 10) * 10 + 10; }
                                        else
                                        {
                                            if (i % 100 != 0) i = (i / 100) * 100 + 100;
                                        }

                                        p.Price = i;
                                        p.PriceTypiesId = 1;

                                        break;
                                    case 2:
                                        var i2 = System.Convert.ToInt32(p.PriceOpt);
                                        var procent = (i2 / 100) * 20;
                                        i = procent + Convert.ToInt32(p.PriceOpt);
                                        if (price < 3000)
                                        { if (i % 10 != 0) i = (i / 10) * 10 + 10; }
                                        else
                                        {
                                            if (i % 100 != 0) i = (i / 100) * 100 + 100;
                                        }
                                        p.Price = i;
                                        p.PriceTypiesId = 2;


                                        break;
                                    case 3:
                                        var i3 = System.Convert.ToInt32(p.PriceOpt);
                                        procent = (i3 / 100) * 10;
                                        i = procent + Convert.ToInt32(p.PriceOpt);
                                        if (price < 3000)
                                        { if (i % 10 != 0) i = (i / 10) * 10 + 10; }
                                        else
                                        {
                                            if (i % 100 != 0) i = (i / 100) * 100 + 100;
                                        }
                                        p.Price = i;
                                        p.PriceTypiesId = 3;
                                        break;
                                    case 4:
                                        var i4 = System.Convert.ToInt32(p.PriceOpt);
                                        procent = (i4 / 100) * 5;
                                        i = procent + Convert.ToInt32(p.PriceOpt);
                                        //if (price < 3000)
                                        //{ if (i % 10 != 0) i = (i / 10) * 10 + 10; }
                                        //else
                                        //{
                                        //    if (i % 100 != 0) i = (i / 100) * 100 + 100;
                                        //}
                                        p.Price = i;
                                        p.PriceTypiesId = 4;

                                        break;
                                    case 5:

                                        p.PriceTypiesId = 5;
                                        p.Price = price;
                                        break;
                                }


                                p.Ostatok = System.Convert.ToInt32(((Excel.Range)range.Cells[row, 7]).Text) + System.Convert.ToInt32(((Excel.Range)range.Cells[row, 6]).Text);
                                if (p.Ostatok == 0) p.SkladTypesId = 4;
                                p.CategoryId = catalogy;
                                p.DateUpdate = DateTime.Today;
                                if (p.Article == null) p.Article = p.ArticlePos;
                                db.Products.Add(p);
                                db.SaveChanges();

                            }
                        }

                    }
                }
            }

            return View();
        }

        [HttpGet]
        public ActionResult AddFromExcel()
        {
            var cat = db.Categories.ToList();
            ViewBag.cat = cat;
            return View();
        }
        [HttpPost]
        public ActionResult AddFromExcel(int? catalogy, HttpPostedFileBase excelfile, int? nacenka, int? OneImg, HttpPostedFileBase img)
        {
            var cat1 = db.Categories.Where(c => c.id == catalogy).FirstOrDefault();
            var cat = db.Categories.ToList();
            ViewBag.cat = cat;
            if (excelfile == null || excelfile.ContentLength == 0)
            {
                ViewBag.Error = "Фаил не выбран";

                return View();
            }
            else
            {
                if (excelfile.FileName.EndsWith("xls") || excelfile.FileName.EndsWith("xlsx"))
                {
                    int i;
                    string filename = System.IO.Path.GetFileName(excelfile.FileName);
                    var path = Server.MapPath("~/Files");
                    if (System.IO.File.Exists(path + filename))
                        System.IO.File.Delete(path + filename);
                    excelfile.SaveAs(path + filename);

                    Excel.Application appplication = new Excel.Application();
                    Excel.Workbook workbook = appplication.Workbooks.Open(path + filename);
                    Excel.Worksheet worksheet = workbook.ActiveSheet;
                    Excel.Range range = worksheet.UsedRange;
                    List<Product> listProduct = new List<Product>();
                    for (int row = 1; row < range.Rows.Count; row++)
                    {
                        string product_article = ((Excel.Range)range.Cells[row, 3]).Text;
                        string product_idpost = ((Excel.Range)range.Cells[row, 1]).Text;
                        var product = db.Products.Where(x => x.Article == product_article).SingleOrDefault();
                        var product_id = db.Products.Where(x => x.ArticlePos == product_idpost).SingleOrDefault();

                        Product p = new Product();
                        p.Article = ((Excel.Range)range.Cells[row, 3]).Text;
                        p.ArticlePos = ((Excel.Range)range.Cells[row, 1]).Text;
                        p.Name = cat1.Name + " " + ((Excel.Range)range.Cells[row, 2]).Text;
                        p.SkladTypesId = 1;
                        p.Url = cat1.URL + "_" + ((Excel.Range)range.Cells[row, 3]).Text;
                        p.PriceOpt = Math.Floor(System.Convert.ToDecimal(((Excel.Range)range.Cells[row, 9]).Text));
                        var price = System.Convert.ToDecimal(((Excel.Range)range.Cells[row, 10]).Text);

                        switch (nacenka)
                        {
                            case 1:
                                i = Convert.ToInt32(price);
                                if (price < 3000)
                                { if (i % 10 != 0) i = (i / 10) * 10 + 10; }
                                else
                                {
                                    if (i % 100 != 0) i = (i / 100) * 100 + 100;
                                }

                                p.Price = i;
                                break;
                            case 2:
                                var i2 = System.Convert.ToInt32(p.PriceOpt);
                                var procent10 = (i2 / 100) * 10;
                                i = procent10 + Convert.ToInt32(p.PriceOpt);
                                if (price < 3000)
                                { if (i % 10 != 0) i = (i / 10) * 10 + 10; }
                                else
                                {
                                    if (i % 100 != 0) i = (i / 100) * 100 + 100;
                                }
                                p.Price = i;
                                break;
                            case 3:
                                var i3 = System.Convert.ToInt32(p.PriceOpt);
                                var procent30 = (i3 / 100) * 30;
                                i = procent30 + Convert.ToInt32(p.PriceOpt);
                                if (price < 3000)
                                { if (i % 10 != 0) i = (i / 10) * 10 + 10; }
                                else
                                {
                                    if (i % 100 != 0) i = (i / 100) * 100 + 100;
                                }
                                p.Price = i;
                                break;

                        }


                        p.Ostatok = System.Convert.ToInt32(((Excel.Range)range.Cells[row, 6]).Text) + System.Convert.ToInt32(((Excel.Range)range.Cells[row, 5]).Text);
                        if (p.Ostatok == 0) p.SkladTypesId = 4;
                        p.CategoryId = catalogy;
                        p.DateUpdate = DateTime.Today;
                        p.MetaKey = "Купить " + p.Name;
                        p.MetaDescription = p.Name;
                        if (OneImg == 1) p.OneImg = true;

                        if (img != null)
                        {
                            string fileNameImg = System.IO.Path.GetFileName(img.FileName);
                            string pathimg = Server.MapPath("~/Files/Images/Catalog/" + cat1.URL + "/");
                            System.IO.Directory.CreateDirectory(Server.MapPath("~/Files/Images/Catalog/" + cat1.URL));

                            img.SaveAs(Server.MapPath("~/Files/Images/Catalog/" + cat1.URL + "/" + fileNameImg));
                            p.Img = "/Files/Images/Catalog/" + cat1.URL + "/" + fileNameImg;
                        }
                        db.Products.Add(p);
                        db.SaveChanges();


                    }
                }
            }

            return View();
        }

        [HttpPost]
        public ActionResult AddPropertyFromXml(int id, PropertyName propertyadd, PropertyAll propertyValue)
        {
            string xmlfile = Server.MapPath("~/Files/GoodsProperties.xml");
            //using (var xmlReader = XmlReader.Create(xmlfile))
            //{
            //    while (xmlReader.Read())
            //    {
            //        if (xmlReader.ReadToFollowing("items"))
            //        {
            //            XmlReader personSubtree = xmlReader.ReadSubtree();
            //            XElement personElement = XElement.Load(personSubtree);
            //        }
            //    }
            //}
            var product = db.Products.Find(id);
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(xmlfile);
            XmlElement xRoot = xDoc.DocumentElement;
            string i1 = "/xml_catalog/items/item[@id='" + product.ArticlePos + "']";

            XmlNodeList childnodes = xRoot.SelectNodes("/xml_catalog/items/item");
            XmlNode childnode = xRoot.SelectSingleNode("item[@id='1386374']");
            foreach (XmlNode item in xRoot.SelectSingleNode(i1))
            {
                var i = item.InnerText;
                var a = item.Name;
                string i2 = "/xml_catalog/properties/property[@id='" + a + "']";


                XmlNode property = xRoot.SelectSingleNode(i2);
                var findPropertyName = db.PropertyNames.SingleOrDefault(x => x.PropertyId == a);
                if (findPropertyName == null)
                {
                    propertyadd.PropertyId = a;
                    propertyadd.Name = property.InnerText;
                    db.PropertyNames.Add(propertyadd);
                    propertyValue.PropertyNameId = propertyadd.id;



                }

                else
                {
                    propertyValue.PropertyNameId = findPropertyName.id;
                }

                propertyValue.ProductId = product.id;
                //propertyValue.PropertyNameId = tempIdPro;
                propertyValue.PropertyValue = i;
                propertyValue.CategoryId = product.CategoryId;
                db.PropertyAlls.Add(propertyValue);




                db.SaveChanges();





            }

            var ProizTemd = db.PropertyAlls.Include(x => x.PropertyNames).Where(x => x.ProductId == product.id).SingleOrDefault(x => x.PropertyNames.Name == "Производитель");
            var IdProiz = db.Manufacturers.SingleOrDefault(x => x.Name == ProizTemd.PropertyValue);
            product.ManufacturerId = IdProiz.id;
            db.Entry(product).State = EntityState.Modified;

            db.SaveChanges();
            return Json("ок");
        }

        public ActionResult ProductSearch(string name)
        {
            var product = db.Products.Where(a => a.Article.Contains(name) || a.Name.Contains(name)).ToList();
            return PartialView(product);
        }
        [HttpPost]
        public ActionResult GetProductFromNetlab(string idpost)
        {


            var token = Core.GetFromNetlabs.GetToken();
            var price = Core.GetFromNetlabs.GetPrice(token, idpost);
            var kurs = Core.GetFromNetlabs.GetKursRub( token);var ostatok = Core.GetFromNetlabs.GetOstatok(token, idpost);

   
            var priceProduct = price * kurs;

            Product products = db.Products.Include(p=>p.PriceTypies).SingleOrDefault(x => x.ArticlePos == idpost);
            if (products != null)
            {
                products.PriceOpt = Convert.ToDecimal( priceProduct);
                products.Ostatok = ostatok;
                if (products.PriceTypiesId != 5)
                {
                    products.Price = (products.PriceOpt * products.PriceTypies.Proccent) + products.PriceOpt;
                    var i = Convert.ToInt32(products.Price);
                    if (i < 3000)
                    { if (i % 10 != 0) i = (i / 10) * 10 + 10; }
                    else
                    {
                        if (i % 100 != 0) i = (i / 100) * 100 + 100;
                    }

                    products.Price = i;
                   
                }
                else
                {
                    var i2 = System.Convert.ToInt32(products.PriceOpt);
                    var procent10 = (i2 / 100) * 10;
                    var i = procent10 + Convert.ToInt32(products.PriceOpt);
                    if (i < 3000)
                    { if (i % 10 != 0) i = (i / 10) * 10 + 10; }
                    else
                    {
                        if (i % 100 != 0) i = (i / 100) * 100 + 100;
                    }

                    products.Price = i;
                }


            }
            db.SaveChanges();
            return Json("Данные о товаре: " + products.Name + " изменены. Количетство "+ostatok + " цена " + products.Price );

        }

        [HttpPost]
        public ActionResult Join(int idModel, int idJoin)
        {
            return Json("Добавлено");
        }


        public ActionResult Setting_GetOstatok_for_Netlab ()
        {
            var cat = db.Categories.ToList();
            ViewBag.cat = cat;
            var manufacturer = db.Manufacturers.ToList();
            ViewBag.manufacturer = manufacturer;
            return View();
        }
        [HttpPost]
        public ActionResult Setting_GetOstatok_for_Netlab (int? catalogy, int? manufacturer)
        {
            var cat = db.Categories.ToList();
            ViewBag.cat = cat;
            var manufacture = db.Manufacturers.ToList();
            ViewBag.manufacturer = manufacture;
            var token = Core.GetFromNetlabs.GetToken();
            var kurs = Core.GetFromNetlabs.GetKursRub(token);
            if (catalogy != null&& manufacturer!=null)
            {
                var products = db.Products.Where(x => x.CategoryId == catalogy &&x.ManufacturerId==manufacturer).Include(p=>p.PriceTypies).ToList();
                foreach (var item in products)
                {
                    var idpost = item.ArticlePos;
                    var ostatok = Core.GetFromNetlabs.GetOstatok(token, idpost);
                    var price = Core.GetFromNetlabs.GetPrice(token, idpost);
                    var priceProduct = price * kurs;
                    item.PriceOpt = Convert.ToDecimal(priceProduct);
                    item.Ostatok = ostatok;
                    if (item.PriceTypiesId != 5)
                    {
                        item.Price = (item.PriceOpt * item.PriceTypies.Proccent) + item.PriceOpt;
                        var i = Convert.ToInt32(item.Price);
                        if (item.Price < 3000)
                        { if (i % 10 != 0) i = (i / 10) * 10 + 10; }
                        else
                        {
                            if (i % 100 != 0) i = (i / 100) * 100 + 100;
                        }

                        item.Price = i;

                    }
                    db.SaveChanges();
                }
            }
            else if (catalogy != null && manufacturer == null)
            {
                var products = db.Products.Where(x => x.CategoryId == catalogy).Include(p => p.PriceTypies).ToList();
                foreach (var item in products)
                {
                    var idpost = item.ArticlePos;
                    var ostatok = Core.GetFromNetlabs.GetOstatok(token, idpost);
                    var price = Core.GetFromNetlabs.GetPrice(token, idpost);
                    var priceProduct = price * kurs;
                    item.PriceOpt = Convert.ToDecimal(priceProduct);
                    item.Ostatok = ostatok;
                    if (item.PriceTypiesId != 5)
                    {
                        item.Price = (item.PriceOpt * item.PriceTypies.Proccent) + item.PriceOpt;
                        var i = Convert.ToInt32(item.Price);
                        if (item.Price < 3000)
                        { if (i % 10 != 0) i = (i / 10) * 10 + 10; }
                        else
                        {
                            if (i % 100 != 0) i = (i / 100) * 100 + 100;
                        }

                        item.Price = i;

                    }
                    db.SaveChanges();
                }
            }
            else if (catalogy == null && manufacturer != null)
            {
                var products = db.Products.Where(x => x.CategoryId == catalogy).Include(p => p.PriceTypies).ToList();
                foreach (var item in products)
                {
                    var idpost = item.ArticlePos;
                    var ostatok = Core.GetFromNetlabs.GetOstatok(token, idpost);
                    var price = Core.GetFromNetlabs.GetPrice(token, idpost);
                    var priceProduct = price * kurs;
                    item.PriceOpt = Convert.ToDecimal(priceProduct);
                    item.Ostatok = ostatok;
                    if (item.PriceTypiesId != 5)
                    {
                        item.Price = (item.PriceOpt * item.PriceTypies.Proccent) + item.PriceOpt;
                        var i = Convert.ToInt32(item.Price);
                        if (item.Price < 3000)
                        { if (i % 10 != 0) i = (i / 10) * 10 + 10; }
                        else
                        {
                            if (i % 100 != 0) i = (i / 100) * 100 + 100;
                        }

                        item.Price = i;

                    }
                    db.SaveChanges();
                }
            }
            return View();
        
           
        }



    }
}