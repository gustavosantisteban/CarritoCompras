using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SeguridadWebv2.Models.Aplicacion
{
    [Table("OrdenHistory")]
    public class OrdenHistory
    {
        public int Id { get; set; }
        public DateTime FechaOrden { get; set; }
        public string TransactionId { get; set; }
        public decimal Total { get; set; }
        public string OrdenId { get; set; }

        [ForeignKey("OrdenId")]
        public virtual Orden Orden { get; set; }
    }
}