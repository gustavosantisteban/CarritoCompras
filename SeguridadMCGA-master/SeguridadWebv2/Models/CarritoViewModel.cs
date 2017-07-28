using SeguridadWebv2.Models.Aplicacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SeguridadWebv2.Models
{
    public class CarritoViewModel
    {
        public IEnumerable<CarritoItem> ProductoItems { get; set; }
        public decimal Total { get; set; }
    }
}