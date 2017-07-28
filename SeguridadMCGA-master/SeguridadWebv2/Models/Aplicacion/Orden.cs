
using SeguridadWebv2.Models.Aplicacion;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SeguridadWebv2.Models
{
    [Table("Orden")]
    public class Orden
    {
        public Orden()
        {
            this.OrdenID = Guid.NewGuid().ToString();
            OrdenDetalles = new List<OrdenDetalle>();
        }
        
        [Key]
        public string OrdenID { get; set; }
        
        public DateTime FechaOrden { get; set; }
        
        [Required]
        public decimal Total { get; set; }

        public string UsuarioId { get; set; }

        public string SessionId { get; set; }
        public string Status { get; set; }
        public string MPRefID { get; set; }
        public string MPCollectionID { get; set; }
        
        public virtual List<OrdenDetalle> OrdenDetalles { get; set; }
        
        [ForeignKey("UsuarioId")]
        public virtual ApplicationUser Usuario { get; set; }

    }
}