using SeguridadWebv2.Models;
using SeguridadWebv2.Models.Aplicacion;
using SeguridadWebv2.Repositorio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SeguridadWebv2
{
    public class UnitOfWork : IDisposable
    {
        private ApplicationDbContext context = new ApplicationDbContext();
        private GenericRepository<Categoria> _categoryRepository;
        private GenericRepository<Producto> _productRepository;


        public GenericRepository<Categoria> CategoryRepository
        {
            get
            {
                if (this._categoryRepository == null)
                    this._categoryRepository = new GenericRepository<Categoria>(context);
                return _categoryRepository;
            }
        }
        public GenericRepository<Producto> ProductRepository
        {
            get
            {
                if (this._productRepository == null)
                    this._productRepository = new GenericRepository<Producto>(context);
                return _productRepository;
            }
        }

        public void Save()
        {
            context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}