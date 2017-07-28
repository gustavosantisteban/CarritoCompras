using SeguridadWebv2.Models.Aplicacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace SeguridadWebv2.Controllers
{
    public class CategoriaController : Controller
    {
        private UnitOfWork unitOfWork = new UnitOfWork();


        public ActionResult Index()
        {
            return View(unitOfWork.CategoryRepository.Get());
        }


        public ActionResult Detalle(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Categoria category = unitOfWork.CategoryRepository.GetByID(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        public ActionResult Agregar()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Agregar(Categoria category)
        {
            if (ModelState.IsValid)
            {
                unitOfWork.CategoryRepository.Insert(category);
                unitOfWork.Save();
                return RedirectToAction("Index");
            }

            return View(category);
        }

        public ActionResult Editar(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Categoria category = unitOfWork.CategoryRepository.GetByID(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar(Categoria category)
        {
            if (ModelState.IsValid)
            {
                unitOfWork.CategoryRepository.Update(category);
                unitOfWork.Save();
                return RedirectToAction("Index");
            }
            return View(category);
        }

        public ActionResult Eliminar(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Categoria category = unitOfWork.CategoryRepository.GetByID(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        [HttpPost, ActionName("Eliminar")]
        [ValidateAntiForgeryToken]
        public ActionResult ConfirmarEliminar(string id)
        {
            Categoria category = unitOfWork.CategoryRepository.GetByID(id);
            unitOfWork.CategoryRepository.Delete(category);
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