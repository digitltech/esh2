using ESH.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using static ESH.Models.ESHDBModels;

namespace ESH.Controllers
{
    public class ProductsController : Controller
    {
        ESHDBContext db = new ESHDBContext();
        // GET: Catalogy
        public async Task<ActionResult> Item(string URL)
        {
            var develiry = db.DeveliryPrices.ToList();
            ViewBag.develiry = develiry;

            string tip = null;
            if (URL != null)
            {
                Product prodUrl = await db.Products.FirstOrDefaultAsync(p => p.Url == URL);
                Product product = await db.Products.Include(c => c.Category).Include(m => m.Manufacturers).Include(p => p.SkladTypes).FirstOrDefaultAsync(p => p.id == prodUrl.id);
                var img = db.Imgs.Where(i => i.ProductId == product.id).ToList();
                ViewBag.img = img;
                decimal oldprice = 0;
                var stock = db.Stocks.Include(s => s.Products).Include(s => s.StockTypes).Where(s => s.ProductId == product.id);
                if (stock.Count() > 0)
                {
                    foreach (var st in stock)
                    {
                        switch (st.StockTypeid)
                        {
                            case 1:
                                ViewData["StockType"] = "rassrochka.png";
                                 oldprice = (product.PriceOpt * 20/100) + product.PriceOpt;
                                var i = Convert.ToInt32(oldprice);
                                if (i < 3000)
                                { if (i % 10 != 0) i = (i / 10) * 10 + 10; }
                                else
                                {
                                    if (i % 100 != 0) i = (i / 100) * 100 + 100;
                                }
                                oldprice = i;
                                break;
                            case 2:
                                ViewData["StockType"] = "rasprodaja.png";
                                oldprice = (product.PriceOpt * 20/100) + product.PriceOpt;
                                 i = Convert.ToInt32(oldprice);
                                if (i < 3000)
                                { if (i % 10 != 0) i = (i / 10) * 10 + 10; }
                                else
                                {
                                    if (i % 100 != 0) i = (i / 100) * 100 + 100;
                                }
                                oldprice = i;
                                break;
                            default:
                                ViewData["StockType"] = "no.png";
                                break;
                        }
                    }
                }
                else
                {
                    ViewData["StockType"] = "no.png";
                }
        
                if (product.CategoryId!=25 && product.CategoryId !=24)
                {
                    ViewBag.OldPrice = oldprice;
                }
                else
                {
                    ViewBag.OldPrice = 0;
                }
                var complect = db.Complects.Where(x => x.Productid == product.id).Include(p => p.Products).ToList();
                if (complect.Count() > 0) { ViewBag.complect = complect; }
                var propuctComplect = db.Products.ToList();
                ViewBag.propuctComplect = propuctComplect;
                var OS = db.PropertyAlls.Include(p => p.PropertyNames).Where(x => x.ProductId == product.id).SingleOrDefault(x => x.PropertyNames.Name == "Операционная система");
                if (OS != null)
                {
                    if (OS.PropertyValue == "Windows 10")
                    {
                        ViewData["OS"] = "Win10logo.png";
                    }
                    else
                    {
                        ViewData["OS"] = "no.png";
                    }
                }
                var proc = db.PropertyAlls.Include(p => p.PropertyNames).Where(x => x.ProductId == product.id).SingleOrDefault(x => x.PropertyNames.Name == "Процессор ноутбука");
                if (proc != null)
                {
                    switch (proc.PropertyValue)
                    {
                        case "Intel Core i3":
                            ViewData["proc"] = "i3.png";
                            break;
                        case "Intel Core i5":
                            ViewData["proc"] = "i5.png";
                            break;
                        case "Intel Core i7":
                            ViewData["proc"] = "i7.png";
                            break;
                        default:
                            ViewData["proc"] = "no.png";
                            break;
                    }
                }
         
                // вызов свойств
                var propall = db.PropertyAlls.Include(p => p.PropertyNames).Include(p => p.Products).Where(x => x.ProductId == product.id);
                ViewBag.propertyAll = propall;



                var property = db.Properties.Include(p => p.PropertyTypes).Include(p => p.PropertyValues).Where(p => p.ProductId == product.id).ToList();

                if (product.ManufacturerId != null) { ViewData["Brand"] = product.Manufacturers.Name; }
                else { ViewData["Brand"] = "не указан"; }
                var i3 = System.Convert.ToInt32(product.Price);
                var i2 = (i3 / 100) * 30;
                if (product.Price > 3000)
                {
                    ViewData["PriceCredit"] = "В кредит: " + "от " + (i3 + i2) / 24 + " руб./мес";
                }

                foreach (var tipPro in property)
                {
                    if (tipPro.PropertyTypeId == 2)
                    {
                        tip = tipPro.PropertyValues.Name;
                    }


                }
                if (tip != null) { ViewData["tip"] = tip; }
                else { ViewData["tip"] = "не указан"; }

                ViewBag.property = property;
                //---конец вызова свойств
                //походие товары
                var i4 = (i3 * 0.10) + i3;
                var i5 = (i3 * 0.10);
                var i6 = i3 - i5;
                var randowProduct = db.Products.Include(c => c.Category).Include(m => m.Manufacturers).Include(p => p.SkladTypes).Where(x => x.CategoryId == product.CategoryId).Where(x => x.ManufacturerId == product.ManufacturerId).Take(3);
                ViewBag.randowProduct = randowProduct;

                var imgrandowProduct = db.Imgs.ToList();
                ViewBag.imgrandowProduct = imgrandowProduct;
                //каталог
                var cat_children = db.Categories.Where(c => c.id == product.Category.ParentId).FirstOrDefault();

                var cat_parent = db.Categories.Where(c => c.id == cat_children.ParentId).FirstOrDefault();

                var cat = db.Categories.ToList();
                ViewBag.cat = cat;
                if (cat_parent != null)
                {
                    ViewBag.cat_parent = cat_parent.id;
                    ViewBag.cat_children = cat_children.id;
                }
                else
                {
                    ViewBag.cat_parent = cat_children.id;
                }


                var JoinProduct = db.JoinProducts.Where(p => p.ProductFirst == product.id).Include(c => c.Products);
                if (JoinProduct.Count() != 0)
                {
                    ViewBag.JoinProduct = JoinProduct;
                }
                else
                {
                    ViewBag.JoinProduct = null;
                }
                var sklad = db.Sklads.FirstOrDefault(s => s.ProductId == product.id);
                if (sklad != null)
                {
                    ViewData["sklad"] = "в  магазине - " + sklad.Count + " шт. ";
                }
                else
                {
                    ViewData["sklad"] = product.SkladTypes.Name;
                    var date = DateTime.Today.AddDays(+7);
                    ViewBag.Dostavka = date.ToString("ddd, dd MMM yyyy");
                    date = DateTime.Today.AddDays(+10);
                    ViewBag.Dostavka2 = date.ToString("ddd, dd MMM yyyy");

                }
                var proizvoditel = db.Properties.Where(p => p.PropertyTypes.id == 1).Include(p => p.PropertyTypes).Include(p => p.PropertyValues).ToList();
                ViewBag.proizvoditel = proizvoditel;
                return View(product);
            }
            else
            {
                return HttpNotFound();
            }

        }

        public ActionResult ListCatalogy()
        {

            return View(db.Categories.ToList());
        }
    }
}