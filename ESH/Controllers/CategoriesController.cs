using ESH.Models;
using PagedList;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using static ESH.Models.ESHDBModels;
using PagedList.Mvc;


namespace ESH.Controllers
{
    public class CategoriesController : Controller
    {
     ESHDBContext db = new ESHDBContext();
        // GET: Categories
        public ActionResult Catalogy(string url, int? page, int? manufacturer, SortProduct  sortcatalog = SortProduct.NameAsc)
         {
     
      
            if (url != null)
            {
                if (manufacturer != null)
                {
                    var img = db.Imgs.ToList();
                    ViewBag.img = img;

                    ViewData["NameSort"] = sortcatalog == SortProduct.NameAsc ? SortProduct.NameDesc : SortProduct.NameAsc;
                    ViewData["PriceSort"] = sortcatalog == SortProduct.PriceAsc ? SortProduct.PriceDesc : SortProduct.PriceAsc;

                    ViewBag.CategoryName = db.Categories.Single(c => c.URL == url);
                    ViewBag.Manufac = db.ManufacturerSorts.Include(m => m.Manufacturers).Include(c => c.Categogies).Where(x => x.Categogies.URL == url);
                    ViewBag.manufacture = manufacturer;

                    var product = db.Products.Include(p => p.Category).Where(p => p.Category.URL == url & p.ManufacturerId==manufacturer).ToList();

                    if (product.Count == 0)
                    {
                        var category = db.Categories.Where(c => c.URL == url).FirstOrDefault();


                        product = db.Products.Include(p => p.Category).Where(p => p.Category.ParentId == category.id&p.ManufacturerId==manufacturer).ToList();
                    }
                    //каталог

                    var cat_children = db.Categories.Where(c => c.URL == url).FirstOrDefault();

                    var cat_parent = db.Categories.Where(c => c.id == cat_children.ParentId).FirstOrDefault();

                    var cat = db.Categories.ToList();
                    ViewBag.cat = cat;

                    ViewBag.url = url;
                    ViewBag.manufacture = manufacturer;
                    if (cat_parent != null)
                    {
                        ViewBag.cat_parent = cat_parent.id;
                        ViewBag.cat_children = cat_children.id;
                    }
                    else
                    {
                        ViewBag.cat_parent = cat_children.id;
                    }



                    var property = db.Properties.Include(p => p.Products).Include(p => p.PropertyTypes).Where(p => p.PropertyTypes.Categories.id == cat_children.ParentId).ToList();
                    int pageSize = 30;
                    int pageNumber = (page ?? 1);
                    var productlist = product.OrderBy(s => s.Name);
                    switch (sortcatalog)
                    {
                        case SortProduct.NameDesc:

                            productlist =  product.OrderByDescending(s => s.Name);
                            break;
                        case SortProduct.PriceAsc:
                            productlist = product.OrderBy(s => s.Price);
                            break;
                        case SortProduct.PriceDesc:
                            productlist =  product.OrderByDescending(s => s.Price);
                            break;
                        default:
                            productlist =  product.OrderBy(s => s.Name);
                            break;
                    }
                    return View(productlist.ToPagedList(pageNumber, pageSize));

                }
                else
                {
                    var img = db.Imgs.ToList();
                    ViewBag.img = img;

                    ViewBag.CategoryName = db.Categories.Single(c => c.URL == url);
                    ViewBag.Manufac = db.ManufacturerSorts.Include(m => m.Manufacturers).Include(c => c.Categogies).Where(x => x.Categogies.URL == url);

                    ViewData["NameSort"] = sortcatalog == SortProduct.NameAsc ? SortProduct.NameDesc : SortProduct.NameAsc;
                    ViewData["PriceSort"] = sortcatalog == SortProduct.PriceAsc ? SortProduct.PriceDesc : SortProduct.PriceAsc;

                    var product = db.Products.Include(p => p.Category).Where(p => p.Category.URL == url).ToList();

                    if (product.Count == 0)
                    {
                        var category = db.Categories.Where(c => c.URL == url).FirstOrDefault();


                        product = db.Products.Include(p => p.Category).Where(p => p.Category.ParentId == category.id).ToList();
                    }
                    //каталог

                    var cat_children = db.Categories.Where(c => c.URL == url).FirstOrDefault();

                    var cat_parent = db.Categories.Where(c => c.id == cat_children.ParentId).FirstOrDefault();

                    var cat = db.Categories.ToList();
                    ViewBag.cat = cat;

                    ViewBag.url = url;
                    if (cat_parent != null)
                    {
                        ViewBag.cat_parent = cat_parent.id;
                        ViewBag.cat_children = cat_children.id;
                    }
                    else
                    {
                        ViewBag.cat_parent = cat_children.id;
                    }



                    var property = db.Properties.Include(p => p.Products).Include(p => p.PropertyTypes).Where(p => p.PropertyTypes.Categories.id == cat_children.ParentId).ToList();
                    int pageSize = 30;
                    int pageNumber = (page ?? 1);

                    var productlist = product.OrderBy(s => s.Name);
                    switch (sortcatalog)
                    {
                        case SortProduct.NameDesc:

                            productlist = product.OrderByDescending(s => s.Name);
                            break;
                        case SortProduct.PriceAsc:
                            productlist = product.OrderBy(s => s.Price);
                            break;
                        case SortProduct.PriceDesc:
                            productlist = product.OrderByDescending(s => s.Price);
                            break;
                        default:
                            productlist = product.OrderBy(s => s.Name);
                            break;
                    }
                    return View(productlist.ToPagedList(pageNumber, pageSize));

                }



            }
            else
            {
                return HttpNotFound();
            }

        }

        public ActionResult Filter (List<int> propertyvalue, string url)
        {
            var img = db.Imgs.ToList();
            ViewBag.img = img;
            ViewBag.CategoryName = db.Categories.Single(c => c.URL == url).Name;
            var cat_children = db.Categories.Where(c => c.URL == url).FirstOrDefault();
            var cat = db.Categories.ToList();
            ViewBag.cat = cat;
            var cat_parent = db.Categories.Where(c => c.id == cat_children.ParentId).FirstOrDefault();
      
            if (cat_parent != null)
            {
                ViewBag.cat_parent = cat_parent.id;
                ViewBag.cat_children = cat_children.id;
            }
            else
            {
                ViewBag.cat_parent = cat_children.id;
            }

            
            for (int i=0; i<propertyvalue.Count; i++)
            {
                int x = propertyvalue[i];
                if (x != 0)
                {
                   var property = db.Properties.Include(p=>p.Products).Where(p => p.PropertyValueId == x&&p.Products.Category.URL==url);

                    ViewBag.property = property;
                }
             
            }
           

            return View();
        }
    }
}