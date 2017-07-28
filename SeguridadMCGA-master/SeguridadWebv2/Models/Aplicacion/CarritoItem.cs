using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SeguridadWebv2.Models.Aplicacion
{
    [Table("CarritoItem")]
    public class CarritoItem
    {
        public CarritoItem()
        {
            this.IdCarritoItem = Guid.NewGuid().ToString();
        }

        [Key]
        public string IdCarritoItem { get; set; }

        [Required]
        public string CarritoId { get; set; }

        public string ProductoID { get; set; }

        public int Contador { get; set; }

        public DateTime FechaCreacion { get; set; }


        [ForeignKey("ProductoID")]
        public virtual Producto Producto { get; set; }

    }
}