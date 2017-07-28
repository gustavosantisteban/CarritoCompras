using Microsoft.AspNet.Identity;
using SeguridadWebv2.Models;
using SeguridadWebv2.Models.Aplicacion;
using SeguridadWebv2.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SeguridadWebv2.Controllers
{
    public class CarritoController : Controller
    {
        // GET: Carrito
        public ActionResult Index()
        {

            var cart = new CarritoService(HttpContext);
            var items = cart.GetCartItems();

            return View(new CarritoViewModel
            {
                ProductoItems = items,
                Total = CalcuateCart(items)
            });
        }

        public ActionResult AddToCart(string id)
        {
            var cart = new CarritoService(HttpContext);

            cart.Add(id);

            return RedirectToAction("Index");
        }

        public ActionResult RemoveFromCart(string id)
        {
            var cart = new CarritoService(HttpContext);

            cart.Remove(id);

            return RedirectToAction("index");
        }

        [Authorize]
        public ActionResult Checkout()
        {
            return View();
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Checkout(CheckoutViewModel model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View(model);
        //    }

        //    var cart = new CarritoService(HttpContext);

        //    var result = cart.Checkout(model);

        //    if (result.Message == "Pass")
        //    {
        //        TempData["transactionId"] = result.TransactionId;
        //        cart.ClearCart(HttpContext);
        //        return RedirectToAction("Complete");
        //    }

        //    ModelState.AddModelError(string.Empty, result.Message);

        //    return View(model);
        //}

        public ActionResult Complete()
        {
            ViewBag.TransactionId = (string)TempData["transactionId"];

            return View();
        }

        private static decimal CalcuateCart(IEnumerable<CarritoItem> items)
        {
            return items.Sum(item => (item.Producto.Precio * item.Contador));
        }

        [ChildActionOnly]
        public ActionResult CartSummary()
        {
            var cart = new CarritoService(HttpContext);


            ViewData["CartCount"] = cart.Count();

            return PartialView("CartSummary");
        }

        //public ActionResult ViewOrders()
        //{
        //    ApplicationDbContext db = new ApplicationDbContext();
        //    var userId = HttpContext.User.Identity.GetUserId();
        //    var orders = db.Orden.Where(u => u.UsuarioId == userId).ToList();
        //    var result = (from x in orders
        //                  where x.UsuarioId == userId
        //                  select new OrdenHistory
        //                  {

        //                      FechaOrden = x.FechaOrden,
        //                      OrdenId = x.OrdenID,
        //                      TransactionId = x.TransactionId,
        //                      Total = x.Total


        //                  }).ToList();

        //    return View(result);
        //}
    }
}