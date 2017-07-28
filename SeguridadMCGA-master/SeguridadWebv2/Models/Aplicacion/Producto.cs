using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SeguridadWebv2.Models.Aplicacion
{
    [Table("Producto")]
    public class Producto
    {
        public Producto()
        {
            this.IdProducto = Guid.NewGuid().ToString();
        }

        [Key]
        public string IdProducto { get; set; }
        public string Nombre { get; set; }
        public decimal Precio { get; set; }
        public string ImagenURL { get; set; }
        public string IdCategoria { get; set; }

        [ForeignKey("IdCategoria")]
        public virtual Categoria Categoria { get; set; }
    }
}