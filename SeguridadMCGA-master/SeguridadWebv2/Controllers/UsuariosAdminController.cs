using SeguridadWebv2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace SeguridadWebv2.Controllers
{
    [Authorize(Roles = "Admin, AllUsuarios")]
    public class UsuariosAdminController : Controller
    {
        public UsuariosAdminController()
        {
        }

        public UsuariosAdminController(ApplicationUserManager userManager,
            ApplicationRoleManager roleManager)
        {
            UserManager = userManager;
            RoleManager = roleManager;
        }

        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext()
                    .GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        // Add the Group Manager (NOTE: only access through the public
        // Property, not by the instance variable!)
        private GrupoManager _groupManager;
        public GrupoManager GroupManager
        {
            get
            {
                return _groupManager ?? new GrupoManager();
            }
            private set
            {
                _groupManager = value;
            }
        }

        private ApplicationRoleManager _roleManager;
        public ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.GetOwinContext()
                    .Get<ApplicationRoleManager>();
            }
            private set
            {
                _roleManager = value;
            }
        }


        public async Task<ActionResult> Index()
        {
            return View(await UserManager.Users.ToListAsync());
        }


        [Authorize(Roles = "Admin, Detalle_Usuario")]
        public async Task<ActionResult> Detalles(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = await UserManager.FindByIdAsync(id);

            // Show the groups the user belongs to:
            var userGroups = await this.GroupManager.GetUserGroupsAsync(id);
            ViewBag.GroupNames = userGroups.Select(u => u.Name).ToList();
            return View(user);
        }


        [Authorize(Roles = "Admin, Agregar_Usuario")]
        public ActionResult Agregar()
        {
            // Show a list of available groups:
            ViewBag.GroupsList =
                new SelectList(this.GroupManager.Groups, "Id", "Name");
            return View();
        }


        [HttpPost]
        public async Task<ActionResult> Agregar(RegistrarseViewModel userViewModel,
            params string[] selectedGroups)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = userViewModel.Email,
                    Email = userViewModel.Email,
                    Nombre = userViewModel.Nombre,
                    Apellido = userViewModel.Apellido,
                    Estado = userViewModel.Estado
                };
                var adminresult = await UserManager
                    .CreateAsync(user, userViewModel.Password);

                //Add User to the selected Groups 
                if (adminresult.Succeeded)
                {
                    if (selectedGroups != null)
                    {
                        selectedGroups = selectedGroups ?? new string[] { };
                        await this.GroupManager
                            .SetUserGroupsAsync(user.Id, selectedGroups);
                    }
                    var code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    var callbackUrl = Url.Action("ConfirmarEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    await UserManager.SendEmailAsync(user.Id, "Confirmar su cuenta", "Por favor para confirmar su cuenta haga click en el siguiente enlace: <a href=\"" + callbackUrl + "\">link</a>");
                   
                    return RedirectToAction("Index");
                }
                else
                {
                    AddErrors(adminresult);
                }
            }
            ViewBag.Groups = new SelectList(
                await RoleManager.Roles.ToListAsync(), "Id", "Name");
            return View();
        }


        [Authorize(Roles = "Admin, Editar_Usuario")]
        public async Task<ActionResult> Editar(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = await UserManager.FindByIdAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            // Display a list of available Groups:
            var allGroups = this.GroupManager.Groups;
            var userGroups = await this.GroupManager.GetUserGroupsAsync(id);

            var model = new EditarUsuarioViewModel()
            {
                Id = user.Id,
                Email = user.Email,
                Nombre = user.Nombre,
                Apellido = user.Apellido,
                Estado = user.Estado
            };

            foreach (var group in allGroups)
            {
                var listItem = new SelectListItem()
                {
                    Text = group.Name,
                    Value = group.Id,
                    Selected = userGroups.Any(g => g.Id == group.Id)
                };
                model.GroupsList.Add(listItem);
            }
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Editar(
            [Bind(Include = "Email,Id, Nombre, Apellido, Estado")] EditarUsuarioViewModel editUser,
            params string[] selectedGroups)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByIdAsync(editUser.Id);
                if (user == null)
                {
                    return HttpNotFound();
                }

                // Update the User:
                user.UserName = editUser.Email;
                user.Email = editUser.Email;
                user.Nombre = editUser.Nombre;
                user.Apellido = editUser.Apellido;
                user.Estado = editUser.Estado;
                await this.UserManager.UpdateAsync(user);

                // Update the Groups:
                selectedGroups = selectedGroups ?? new string[] { };
                await this.GroupManager.SetUserGroupsAsync(user.Id, selectedGroups);
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", "Something failed.");
            return View();
        }


        [Authorize(Roles = "Admin, Eliminar_Usuario")]
        public async Task<ActionResult> Eliminar(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = await UserManager.FindByIdAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }


        [HttpPost, ActionName("Eliminar")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ConfirmarEliminar(string id)
        {
            if (ModelState.IsValid)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                var user = await UserManager.FindByIdAsync(id);
                if (user == null)
                {
                    return HttpNotFound();
                }

                // Remove all the User Group references:
                await this.GroupManager.ClearUserGroupsAsync(id);

                // Then Delete the User:
                var result = await UserManager.DeleteAsync(user);
                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", result.Errors.First());
                    return View();
                }
                return RedirectToAction("Index");
            }
            return View();
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }
    }
}