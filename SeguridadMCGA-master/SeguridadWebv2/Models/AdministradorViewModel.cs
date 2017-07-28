using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SeguridadWebv2.Models
{
    public class AccionViewModel
    {
        public string Id { get; set; }
        [Required(AllowEmptyStrings = false)]
        [Display(Name = "RoleName")]
        public string Name { get; set; }
    }

    public class EditarUsuarioViewModel
    {
        public EditarUsuarioViewModel()
        {
            this.RolesList = new List<SelectListItem>();
            this.GroupsList = new List<SelectListItem>();
        }

        public string Id { get; set; }
        [Required(AllowEmptyStrings = false)]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(AllowEmptyStrings = false)]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; }

        [Required(AllowEmptyStrings = false)]
        [Display(Name = "Apellido")]
        public string Apellido { get; set; }

        [Display(Name = "Estado")]
        public bool Estado { get; set; }

        // We will still use this, so leave it here:
        public ICollection<SelectListItem> RolesList { get; set; }

        // Add a GroupsList Property:
        public ICollection<SelectListItem> GroupsList { get; set; }
    }

    public class GrupoViewModel
    {
        public GrupoViewModel()
        {
            this.UsuariosList = new List<SelectListItem>();
            this.RolesList = new List<SelectListItem>();
        }

        [Required(AllowEmptyStrings = false)]
        public string Id { get; set; }
        [Required(AllowEmptyStrings = false)]
        public string Name { get; set; }
        public string Descripcion { get; set; }
        public ICollection<SelectListItem> UsuariosList { get; set; }
        public ICollection<SelectListItem> RolesList { get; set; }
    }
}