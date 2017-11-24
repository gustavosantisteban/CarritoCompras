using SeguridadWebv2.Models.Aplicacion;
using System.Collections.Generic;

namespace SeguridadWebv2.Models.ViewModels
{
    public class DashboardAdministradorVM
    {
        public List<Orden> ordenvm { get; set; }
        public List<Producto> productos { get; set; }
        public List<ApplicationUser> usuarios { get; set; }
        public List<Categoria> categorias { get; set; }
    }
}