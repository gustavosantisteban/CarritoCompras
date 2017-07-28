using SeguridadWebv2.Models;
using SeguridadWebv2.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SeguridadWebv2.Controllers
{
    public class HomeController : Controller
    {
        private readonly StoreService _store;
        public HomeController() : this(new StoreService()) { }
        public HomeController(StoreService service)
        {
            _store = service;
        }
        // GET: Store
        public ActionResult Index()
        {
            var categories = _store.GetCategories();
            return View(categories);
        }

        public ActionResult Browse(string id)
        {
            var products = _store.GetProductsFor(id);

            if (!products.Any())
            {
                return HttpNotFound();
            }

            return View(products);
        }
        [ChildActionOnly]
        public ActionResult CategoryMenu()
        {
            var categories = _store.GetCategories().ToList();

            return PartialView(categories);
        }

        public ActionResult Detalle(string id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            var product = _store.GetProductById(id);

            if (product == null)
            {
                return HttpNotFound();
            }

            return View(product);
        }

        public ActionResult Dashboard()
        {
            return View();
        }
	}
}