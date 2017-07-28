using SeguridadWebv2.Models;
using SeguridadWebv2.Models.Aplicacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SeguridadWebv2.Service
{
    public class StoreService
    {
        private readonly ApplicationDbContext _db;

        public StoreService() : this(new ApplicationDbContext()) { }
        public StoreService(ApplicationDbContext context)
        {
            _db = context;
        }

        public IEnumerable<Categoria> GetCategories()
        {
            return _db.Categorias.OrderBy(c => c.Nombre).ToArray();
        }

        public IEnumerable<Producto> GetProductsFor(string category)
        {
            return _db.Productos.Include("Categoria")
                .Where(p => p.Categoria.Nombre == category).ToArray();
        }

        public Producto GetProductById(string id)
        {
            return _db.Productos.Include("Categoria")
                .Where(p => p.IdProducto == id).SingleOrDefault();
        }
    }
}