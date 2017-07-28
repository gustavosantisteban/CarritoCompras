using Microsoft.AspNet.Identity;
using SeguridadWebv2.Models;
using SeguridadWebv2.Models.Aplicacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SeguridadWebv2.Service
{
    public class CarritoService
    {
        private readonly ApplicationDbContext _db;
        private readonly string _cartId;

        public CarritoService(HttpContextBase context)
            : this(context, new ApplicationDbContext())
        {
        }

        public CarritoService(HttpContextBase httpContext, ApplicationDbContext storeContext)
        {
            _db = storeContext;
            _cartId = GetCartId(httpContext);
        }

        public void Add(string productId)
        {
            var product = _db.Productos
                .SingleOrDefault(p => p.IdProducto == productId);

            if (product == null)
            {
                // TODO: throw exception or do something
                return;
            }

            var cartItem = _db.Carrito
                .SingleOrDefault(c => c.ProductoID == productId && c.CarritoId == _cartId);

            if (cartItem != null)
            {
                cartItem.Contador++;
            }
            else
            {
                cartItem = new CarritoItem
                {
                    ProductoID = productId,
                    CarritoId = _cartId,
                    Contador = 1,
                    FechaCreacion = DateTime.Now
                };

                _db.Carrito.Add(cartItem);
            }

            _db.SaveChanges();
        }

        public int Remove(string productId)
        {
            var cartItem = _db.Carrito
                .SingleOrDefault(c => c.ProductoID == productId && c.CarritoId == _cartId);

            var itemCount = 0;

            if (cartItem == null)
            {
                return itemCount;
            }

            if (cartItem.Contador > 1)
            {
                cartItem.Contador--;
                itemCount = cartItem.Contador;
            }
            else
            {
                _db.Carrito.Remove(cartItem);
            }

            _db.SaveChanges();

            return itemCount;
        }
        public int Count()
        {
            return _db.Carrito
                            .Count(c => c.CarritoId == _cartId);

        }
        public IEnumerable<CarritoItem> GetCartItems()
        {
            return _db.Carrito.Include("Producto")
                .Where(c => c.CarritoId == _cartId).ToArray();
        }

        //public PaymentResult Checkout(CheckoutViewModel model)
        //{
        //    var items = GetCartItems();

        //    var order = new Orden()
        //    {
        //        FirstName = model.FirstName,
        //        LastName = model.LastName,

        //        Email = model.Email,
        //        OrderDate = DateTime.Now,
        //        UserId = HttpContext.Current.User.Identity.GetUserId()

        //    };

        //    foreach (var item in items)
        //    {
        //        var detail = new OrderDetalle()
        //        {
        //            ProductId = item.ProductId,
        //            UnitPrice = item.Product.Price,
        //            Quantity = item.Count
        //        };

        //        order.Total += (item.Product.Price * item.Count);

        //        order.OrderDetails.Add(detail);
        //    }

        //    model.Total = order.Total;


        //    var request = new TransactionRequest()
        //    {
        //        PrivateKey = "c4660e3571ca74392d20cc91f4568748",
        //        PublicKey = "y5yfh9kcnvt8qf4x",
        //        Amount = model.Total,

        //        TransactionCreditCardRequest = new CreditCardDetails()
        //        {
        //            Number = model.CardNumber,
        //            CVV = model.Cvv,
        //            ExpirationMonth = model.Month,
        //            ExpirationYear = model.Year
        //        }
        //    };
        //    var result = Utility.Request(request);
        //    if (result.Message == "Pass")
        //    {
        //        order.TransactionId = result.TransactionId;
        //        _db.Orders.Add(order);
        //        _db.SaveChanges();
        //    }

        //    return result;
        //}

        public void ClearCart(HttpContextBase http)
        {
            if (http.Request.Cookies["Carrito"] != null)
            {
                var c = new HttpCookie("Carrito");
                c.Expires = DateTime.Now.AddDays(-1);
                http.Response.Cookies.Add(c);
            }


        }
        private string GetCartId(HttpContextBase http)
        {
            var cookie = http.Request.Cookies.Get("Carrito");
            var cartId = string.Empty;

            if (cookie == null || string.IsNullOrWhiteSpace(cookie.Value))
            {
                cookie = new HttpCookie("Carrito");
                cartId = Guid.NewGuid().ToString();

                cookie.Value = cartId;
                http.Response.Cookies.Add(cookie);
            }
            else
            {
                cartId = cookie.Value;
            }
            return cartId;
        }

    }
}