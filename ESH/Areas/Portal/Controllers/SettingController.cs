using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Excel = Microsoft.Office.Interop.Excel;
using static ESH.Models.ESHDBModels;
using ESH.Models;

namespace ESH.Areas.Portal.Controllers
{
    public class SettingController : Controller
    {
        ESHDBContext db = new ESHDBContext();
        // GET: Portal/Setting
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Add_Sales()
        {
            var manufacture = db.Manufacturers.ToList();
            ViewBag.manufacture = manufacture;
            var catalogy = db.Categories.ToList();
            ViewBag.catalogy = catalogy;
            var StokTypes = db.StockTypes.ToList();
            ViewBag.StockTypes = StokTypes;
            return View();
        }
        [HttpPost]
        public ActionResult Add_Sales(int catalogy, int StockTypes,int manufacture)
        {
            Stock stock = new Stock();
            var product = db.Products.Where(x => x.CategoryId == catalogy).Where(x => x.ManufacturerId == manufacture).ToList();
            foreach (var item in product)
            {
                stock.ProductId = item.id;
                stock.StockTypeid = StockTypes;
                db.Stocks.Add(stock);
                db.SaveChanges();
            }
            return View();
        }
        public ActionResult AddPriceDeveliry()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddPriceDeveliry(HttpPostedFileBase excelfile)
        {

            if (excelfile == null || excelfile.ContentLength == 0)
            {
                ViewBag.Error = "Фаил не выбран";

                return View();
            }
            else
            {
                if (excelfile.FileName.EndsWith("xml") || excelfile.FileName.EndsWith("xlsx"))
                {
                    string filename = System.IO.Path.GetFileName(excelfile.FileName);
                    var path = Server.MapPath("~/Files");
                    if (System.IO.File.Exists(path + filename))
                        System.IO.File.Delete(path + filename);
                    excelfile.SaveAs(path + filename);
                    Excel.Application appplication = new Excel.Application();
                    Excel.Workbook workbook = appplication.Workbooks.Open(path + filename);
                    Excel.Worksheet worksheet = workbook.ActiveSheet;
                    Excel.Range range = worksheet.UsedRange;
                    for (int row = 1; row < range.Rows.Count; row++)
                    {
                        if (((Excel.Range)range.Cells[row, 1]).Text != "Город" || ((Excel.Range)range.Cells[row, 1]).Text != "end")

                        {
                            DeveliryPrice dev = new DeveliryPrice();
                            dev.City = ((Excel.Range)range.Cells[row, 1]).Text;
                            dev.Tarif = Convert.ToInt32(((Excel.Range)range.Cells[row, 2]).Text);
                            dev.dver3000 = Convert.ToDecimal(((Excel.Range)range.Cells[row, 4]).Text);
                            dev.sklad_3000 = Convert.ToDecimal(((Excel.Range)range.Cells[row, 3]).Text);
                            dev.sklad30000 = Convert.ToDecimal(((Excel.Range)range.Cells[row, 5]).Text);
                            dev.dver30000 = Convert.ToDecimal(((Excel.Range)range.Cells[row, 6]).Text);

                            db.DeveliryPrices.Add(dev);
                            db.SaveChanges();

                        }

                    }
                }
            }
            return View();
        }
    }
    }
 