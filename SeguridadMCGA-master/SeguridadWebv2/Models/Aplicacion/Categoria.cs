using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SeguridadWebv2.Models.Aplicacion
{
    [Table("Categoria")]
    public class Categoria
    {
        public Categoria()
        {
            this.IdCategoria = Guid.NewGuid().ToString();
        }

        [Key]
        public string IdCategoria { get; set; }
        public string Nombre { get; set; }
        public string Imagen { get; set; }
        public virtual ICollection<Producto> Productos { get; set; }
    }
}