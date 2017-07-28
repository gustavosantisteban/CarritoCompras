using mercadopago;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using SeguridadWebv2.Helper;
using SeguridadWebv2.Models;
using SeguridadWebv2.Models.Aplicacion;
using SeguridadWebv2.Service;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SeguridadWebv2.Controllers
{
    public class MercadoPagoController : Controller
    {
        // GET: MercadoPago
        ApplicationDbContext db = new ApplicationDbContext();
        // GET: MercadoPago
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DoCheckout(string data)
        {
            var cart = new CarritoService(HttpContext);
            var items = cart.GetCartItems();
            if (items == null)
            {
                return HttpNotFound();
            }

            CarritoViewModel model = new CarritoViewModel()
            {
                ProductoItems = items,
                Total = CalcuateCart(items)
            };

            var pf = new PreferencesMP()
            {
                items = new List<Items>()
                {
                    new Items() {
                        currency_id = "ARS",
                        unit_price = model.Total,
                        quantity = 1,
                        title = "La Cantidad de Productos por comprar son: " + model.ProductoItems.Count().ToString()
                    },
                },
            };
            MP mp = new MP(ConfigurationManager.AppSettings["MPClientID"], ConfigurationManager.AppSettings["MPSecret"]);
            mp.sandboxMode(bool.Parse(ConfigurationManager.AppSettings["MPSandbox"]));
            var datos = new
            {
                items = pf.items.Select(i => new { title = i.title, quantity = i.quantity, currency_id = i.currency_id, unit_price = i.unit_price }).ToArray(),
                back_urls = new
                {
                    success = "http://" + Request.Url.Authority + Url.RouteUrl("CheckoutStatus"),
                    failure = "http://" + Request.Url.Authority + Url.RouteUrl("CheckoutStatus"),
                    pending = "http://" + Request.Url.Authority + Url.RouteUrl("CheckoutStatus")
                }
            };
            Hashtable preference = mp.createPreference(JsonConvert.SerializeObject(datos));


            string mprefid = (string)((Hashtable)preference["response"])["id"];

            var usuario = User.Identity.GetUserId();

            db.Configuration.LazyLoadingEnabled = false;
            db.Configuration.ProxyCreationEnabled = false;

            Orden orden = new Orden()
            {
                OrdenDetalles = new List<OrdenDetalle>()
                {
                    new OrdenDetalle() {
                        currency_id = "ARS",
                        unit_price = model.Total,
                        quantity = 1,
                        title = "Compra por Shopping UAI " + model.Total.ToString(),
                        EsValido = false,
                        IdCarrito = cart.GetCartItems().FirstOrDefault().CarritoId
                    },
                },
                FechaOrden = DateTime.Now,
                MPCollectionID = "",
                Status = "",
                SessionId = usuario,
                MPRefID = mprefid,
                UsuarioId = usuario,
            };
            db.Orden.Add(orden);
            db.SaveChanges();
            //string MPRefID = (string)((Hashtable)preference["response"])["id"];

            return Json(new { url = (string)((Hashtable)preference["response"])[ConfigurationManager.AppSettings["MPUrl"]] });
        }

        private static decimal CalcuateCart(IEnumerable<CarritoItem> items)
        {
            return items.Sum(item => (item.Producto.Precio * item.Contador));
        }

        [HttpGet]
        public ActionResult CheckoutStatus(string collection_id, string collection_status, string preference_id, string external_reference, string payment_type, string merchant_order_id)
        {
            string mpRefID = Request["preference_id"];
            string status = Request["collection_status"];
            string collectionID = Request["collection_id"];

            var orden = db.Orden.Where(x => x.MPRefID == mpRefID).FirstOrDefault();

            if (string.IsNullOrWhiteSpace(mpRefID) || string.IsNullOrWhiteSpace(status) || string.IsNullOrWhiteSpace(collectionID))
            {
                return Redirect("/");
            }
            else
            {
                orden.Status = status;
                orden.MPCollectionID = collectionID;
                orden.MPRefID = mpRefID;
                db.Entry(orden).State = EntityState.Modified;
                db.SaveChanges();

                var ordenrelation = orden.OrdenDetalles.Where(x => x.IdOrden == orden.OrdenID).FirstOrDefault();
                return View("../Carrito/Status", orden);
            };
        }

    }
}