using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static ESH.Models.ESHDBModels;
using System.Web.Mvc;
using System.Xml.Linq;
using ESH.Models;
using ESH.Core;

namespace ESH.Core
{
  
    public class Generator
    {

        ESHDBContext db = new ESHDBContext();
        //ProductRepository productRepository;

        //public Generator()
        //{
        //    productRepository = new ProductRepository();
        //}
        public IReadOnlyCollection <string> GetSitemapNodes(UrlHelper urlHelper)
        {
            List<string> nodes = new List<string>();
            nodes.Add(urlHelper.AbsoluteRouteUrl("Default", new { controller = "Home", action = "Index" }));
            nodes.Add(urlHelper.AbsoluteRouteUrl("Default", new { controller = "Home", action = "About" }));
            nodes.Add(urlHelper.AbsoluteRouteUrl("Default", new { controller = "Home", action = "Contact" }));
            nodes.Add(urlHelper.AbsoluteRouteUrl("Default", new { controller = "Home", action = "Garantii" }));
            nodes.Add(urlHelper.AbsoluteRouteUrl("Default", new { controller = "Home", action = "Vozvrat" }));
            nodes.Add(urlHelper.AbsoluteRouteUrl("Default", new { controller = "Home", action = "Credit" }));
            foreach (var caturl in db.Categories.ToList())
            {
                nodes.Add(urlHelper.AbsoluteRouteUrl("Default", new { controller = "Categories", action = "Catalogy", url = caturl.URL }));
            }
            foreach (var producturl in db.Products.ToList())
            {
                nodes.Add(urlHelper.AbsoluteRouteUrl("Default", new { controller = "Products", action = "item", url = producturl.Url }));
            }
          
            return nodes;
        }
        public string GetSitemapDocument(IEnumerable<string> sitemapNodes)
        {
            XNamespace xmlns = "http://www.sitemaps.org/schemas/sitemap/0.9";
            XElement root = new XElement(xmlns + "urlset");

            foreach (string sitemapNode in sitemapNodes)
            {
                XElement urlElement = new XElement(
                    xmlns + "url",
                    new XElement(xmlns + "loc", Uri.EscapeUriString(sitemapNode)));
                root.Add(urlElement);
            }

            XDocument document = new XDocument(root);
            return document.ToString();
        }
     
    }
    public static class UrlHelperExtensions
    {
        public static string AbsoluteRouteUrl(this UrlHelper urlHelper,
            string routeName, object routeValues = null)
        {
            string scheme = urlHelper.RequestContext.HttpContext.Request.Url.Scheme;
            return urlHelper.RouteUrl(routeName, routeValues, scheme);
        }
    }
    //public class ProductRepository
    //{
    //    public string[] GetProductUrl()
    //    {
    //        var p = db.Products.ToList();

    //        return p;
    //    }
    //}
}