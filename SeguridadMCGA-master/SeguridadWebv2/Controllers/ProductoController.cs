using SeguridadWebv2.Models;
using SeguridadWebv2.Models.Aplicacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace SeguridadWebv2.Controllers
{
    public class ProductoController : Controller
    {
        private UnitOfWork unitOfWork = new UnitOfWork();
        
        public ActionResult Index()
        {
            return View(unitOfWork.ProductRepository.Get());
        }
        
        public ActionResult Detalle(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Producto product = unitOfWork.ProductRepository.GetByID(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }
        
        public ActionResult Agregar()
        {
            ViewBag.CategoryId = new SelectList(new ApplicationDbContext().Categorias, "IdCategoria", "Nombre");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Agregar(Producto product)
        {
            if (ModelState.IsValid)
            {
                unitOfWork.ProductRepository.Insert(product);
                unitOfWork.Save();
                return RedirectToAction("Index");
            }
            ViewBag.CategoriaId = new SelectList(new ApplicationDbContext().Categorias, "IdCategoria", "Nombre", product.IdCategoria);
            return View(product);
        }


        public ActionResult Eliminar(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Producto product = unitOfWork.ProductRepository.GetByID(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }


        [HttpPost, ActionName("Eliminar")]
        [ValidateAntiForgeryToken]
        public ActionResult ConfirmarEliminar(int id)
        {
            Producto product = unitOfWork.ProductRepository.GetByID(id);
            unitOfWork.ProductRepository.Delete(product);
            unitOfWork.Save();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                unitOfWork.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}