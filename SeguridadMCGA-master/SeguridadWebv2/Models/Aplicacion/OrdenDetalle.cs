using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SeguridadWebv2.Models.Aplicacion
{
    [Table("OrdenDetalle")]
    public class OrdenDetalle
    {
        public OrdenDetalle() {
            this.IdOrdenDetalle = Guid.NewGuid().ToString();
        }

        [Key]
        public string IdOrdenDetalle { get; set; }
        
        public string title { get; set; }
        public string currency_id { get; set; }
        public decimal unit_price { get; set; }
        public int quantity { get; set; }
        public bool EsValido { get; set; }
        public string IdCarrito { get; set; }
        public string IdOrden { get; set; }
        public string IdProducto { get; set; }

        [ForeignKey("IdOrden")]
        public virtual Orden Orden { get; set; }
        [ForeignKey("IdProducto")]
        public virtual Producto Producto { get; set; }
    }
}