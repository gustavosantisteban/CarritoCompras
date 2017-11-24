using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Newtonsoft.Json;
using SeguridadWebv2.Models;
using SeguridadWebv2.Models.Aplicacion;
using SeguridadWebv2.Models.ViewModels;
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
        private ApplicationDbContext db = new ApplicationDbContext();

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
            ApplicationUserManager UserManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var usuario = User.Identity.GetUserId();
            if (usuario == null)
            {
                return HttpNotFound();
            }
            var rol = UserManager.GetRoles(usuario);
            if (rol.Contains("Cliente") || rol.Contains("AllCliente"))
            {
                var viewmodel = this.DashboardCliente(usuario);
                return View(viewmodel);
            }
            if (rol.Contains("Vendedor") || rol.Contains("AllVendedor"))
            {
                var viewmodel = this.DashboardVendedor(usuario);
                return View(viewmodel);
            }
            if (rol.Contains("Admin"))
            {
                var viewmodel = this.DashboardAdministrator();
                return View(viewmodel);
            }
            return View();
        }


        [HttpGet]
        [Authorize]
        public DashboardClienteVM DashboardCliente(string PacienteID)
        {
            return null;
            //IEnumerable<Turno> turnospacientes = (from turnos in db.Turnos
            //                                      join paciente in db.Pacientes on turnos.Paciente.Id equals paciente.Id
            //                                      where turnos.Paciente.Id == PacienteID && turnos.EstadoTurno == Estado.Pendiente
            //                                      select turnos).ToList();


            //ICollection<TurnosViewModel> _turnosvm = turnospacientes.Select(b => new TurnosViewModel
            //{
            //    IdTurno = b.IdTurno,
            //    Dia = b.Dia.Date,
            //    HoraInicio = b.HoraInicio,
            //    HoraFin = b.HoraFin,
            //    Precio = b.Precio,
            //    EstadoTurno = b.EstadoTurno,
            //    Especialista = b.Especialista
            //}).ToList();

            //var viewmodel = new DashboardClienteVM()
            //{
            //    TurnosViewModel = _turnosvm
            //};
            //return viewmodel;
        }


        [HttpGet]
        [Authorize]
        public DashboardVendedorVM DashboardVendedor(string EspecialistaID)
        {
            //IEnumerable<Turno> turnospacientes = (from turnos in db.Turnos
            //                                      join especialistas in db.Especialistas on turnos.Especialista.Id equals especialistas.Id
            //                                      where especialistas.Id == EspecialistaID && turnos.EstadoTurno == Estado.Pendiente
            //                                      select turnos).ToList();

            //ICollection<TurnosViewModel> _turnosvm = turnospacientes.Select(b => new TurnosViewModel
            //{
            //    IdTurno = b.IdTurno,
            //    Dia = b.Dia.Date,
            //    HoraInicio = b.HoraInicio,
            //    HoraFin = b.HoraFin,
            //    Precio = b.Precio,
            //    EstadoTurno = b.EstadoTurno,
            //    Especialista = b.Especialista,
            //    Paciente = b.Paciente
            //}).ToList();

            //var viewmodel = new DashboardEspecialistaVM()
            //{
            //    TurnosViewModel = _turnosvm
            //};
            //return viewmodel;
            return null;
        }
        
        [HttpGet]
        [Authorize]
        public DashboardAdministradorVM DashboardAdministrator()
        {
            IEnumerable<Orden> _ordenes = (from t in db.Orden
                                          where t.Status == "approved"
                                          select t).ToList();

            IEnumerable<Producto> _productos = (from t in db.Productos
                                                select t).ToList();

            IEnumerable<ApplicationUser> _usuarios = (from t in db.Users
                                                        where t.Estado == true
                                                        select t).ToList();

            var _ordenturno = (from e in db.Orden
                               where e.Status == "approved"
                               orderby e.FechaOrden
                               group e by new { data = e.FechaOrden.Month } into e
                               select new
                               {
                                   e.Key.data
                               }).ToList();
            
            ViewBag.ordenturno = JsonConvert.SerializeObject(_ordenturno);
            //ViewBag.reportemensual = _reportemensual;
            //ViewBag.turnos20M = turnos20M;
            //ViewBag.turnos30M = turnos30M;
            var viewmodel = new DashboardAdministradorVM()
            {
                ordenvm = _ordenes.ToList(),
                usuarios = _usuarios.ToList(),
                productos = _productos.ToList(),
                categorias = null,
            };

            return viewmodel;
        }

        public JsonResult SendNotification(string username, string message)
        {
            return Json(true);
        }
    }
}