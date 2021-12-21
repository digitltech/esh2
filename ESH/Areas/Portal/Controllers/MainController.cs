using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ESH.Areas.Portal.Controllers
{
    public class MainController : Controller
    {
        [Authorize(Roles = "Администратор")]
        // GET: Portal/Main
        public ActionResult Index()
        {
            return View();
        }
    }
}