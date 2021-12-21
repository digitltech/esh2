using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using  ESH.Models;

namespace ESH.Areas.Portal.Controllers
{
    public class CategoryController : Controller
    {
        ESHDBModels.ESHDBContext  db = new ESHDBModels.ESHDBContext ();
        // GET: Portal/Category
        public ActionResult Index()
        {
            return View(db.Categories);
        }
        [HttpGet]
        public ActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Add(Category category)
        {
            db.Categories.Add(category);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Join()
        {
            return View();
        }
    }
}