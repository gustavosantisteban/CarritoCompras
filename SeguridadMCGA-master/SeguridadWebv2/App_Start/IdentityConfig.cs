using System;
using System.Web;
using SeguridadWebv2.Models;
using System.Security.Claims;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Threading.Tasks;
using System.Data.Entity;
using Twilio;
using System.Collections.Generic;
using SeguridadWebv2.Models.Aplicacion;

namespace SeguridadWebv2.Models
{
    public class ApplicationUserManager : UserManager<ApplicationUser, string>
    {
        public ApplicationUserManager(IUserStore<ApplicationUser, string> store)
            : base(store)
        {
        }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options,
            IOwinContext context)
        {
            var manager = new ApplicationUserManager(new UserStore<ApplicationUser, ApplicationRole, string, ApplicationUserLogin, ApplicationUserRole, ApplicationUserClaim>(context.Get<ApplicationDbContext>()));
            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<ApplicationUser>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };
            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = true,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true,
            };
            // Configure user lockout defaults
            manager.UserLockoutEnabledByDefault = true;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            manager.MaxFailedAccessAttemptsBeforeLockout = 5;
            // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
            // You can write your own provider and plug in here.
            manager.RegisterTwoFactorProvider("PhoneCode", new PhoneNumberTokenProvider<ApplicationUser>
            {
                MessageFormat = "Su Codigo de seguridad es: {0}"
            });
            manager.RegisterTwoFactorProvider("EmailCode", new EmailTokenProvider<ApplicationUser>
            {
                Subject = "SecurityCode",
                BodyFormat = "Your security code is {0}"
            });
            manager.EmailService = new EmailService();
            manager.SmsService = new SmsService();
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider =
                    new DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }
    }

    // Configure the RoleManager used in the application. RoleManager is defined in the ASP.NET Identity core assembly
    public class ApplicationRoleManager : RoleManager<ApplicationRole>
    {
        public ApplicationRoleManager(IRoleStore<ApplicationRole, string> roleStore)
            : base(roleStore)
        {
        }

        public static ApplicationRoleManager Create(IdentityFactoryOptions<ApplicationRoleManager> options, IOwinContext context)
        {
            return new ApplicationRoleManager(new ApplicationRoleStore(context.Get<ApplicationDbContext>()));
        }
    }

    public class EmailService : IIdentityMessageService
    {
        public async Task SendAsync(IdentityMessage message)
        {
                // Plug in your email service here to send an email.
                var credentialUserName = "finalmcga@gmail.com";
                var sentFrom = "finalmcga@gmail.com";
                var pwd = "finalmcga123";

                // Configure the client:
                System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient("smtp.gmail.com");

                client.Port = 587;
                client.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;

                // Creatte the credentials:
                System.Net.NetworkCredential credentials = new System.Net.NetworkCredential(credentialUserName, pwd);
                client.Credentials = credentials;
                client.EnableSsl = true;

                // Create the message:
                var mail = new System.Net.Mail.MailMessage(sentFrom, message.Destination);
                mail.Subject = message.Subject;
                mail.Body = message.Body;

                await client.SendMailAsync(mail);
            
            // Plug in your email service here to send an email.
            //return configuracionSMTPasync(message);
            //return Task.FromResult(0);
        }

    }

    public class SmsService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            string AccountSid = "AC866294d7b802640530b4611a61feeba7";
            string AuthToken = "b881dd593ebd7176e6ff9dea9516bb73";
            string twilioPhoneNumber = "+543413544172";

            var twilio = new TwilioRestClient(AccountSid, AuthToken);

            twilio.SendMessage(twilioPhoneNumber, message.Destination, message.Body);

            // Twilio does not return an async Task, so we need this:
            return Task.FromResult(0);

            // Plug in your sms service here to send a text message.
           // return Task.FromResult(0);
        }
    }

    // This is useful if you do not want to tear down the database each time you run the application.
    // public class ApplicationDbInitializer : DropCreateDatabaseAlways<ApplicationDbContext>
    // This example shows you how to create a new database if the Model changes
    public class ApplicationDbInitializer : DropCreateDatabaseAlways<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext context)
        {
            InitializeIdentityForEF(context);
            base.Seed(context);
        }

        //Create User=Admin@Admin.com with password=Admin@123456 in the Admin role        
        public static void InitializeIdentityForEF(ApplicationDbContext db)
        {
            var userManager = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var roleManager = HttpContext.Current.GetOwinContext().Get<ApplicationRoleManager>();
            const string nombre = "Ezequiel";
            const string apellido = "Ellena";
            const bool estado = true;
            const string name = "ezequielellena0003@gmail.com";
            const string password = "Ezequiel@123456";
            const string roleName = "Admin";

            //Create Role Admin if it does not exist
            var role = roleManager.FindByName(roleName);
            if (role == null)
            {
                role = new ApplicationRole(roleName);
                var roleresult = roleManager.Create(role);
            }

            var user = userManager.FindByName(name);
            if (user == null)
            {
                user = new ApplicationUser { UserName = name, Email = name, Nombre = nombre, Apellido = apellido, Estado = estado, EmailConfirmed = true };
                var result = userManager.Create(user, password);
                result = userManager.SetLockoutEnabled(user.Id, false);
            }

            var groupManager = new GrupoManager();
            var newGroup = new ApplicationGroup("Administradores", "Acceso General al Sistema");

            groupManager.CreateGroup(newGroup);
            groupManager.SetUserGroups(user.Id, new string[] { newGroup.Id });
            groupManager.SetGroupRoles(newGroup.Id, new string[] { role.Name });

            var PermisosUsuario = new List<ApplicationRole> {
                new ApplicationRole {
                    Name = "Agregar_Usuario"
                },
                new ApplicationRole {
                    Name = "Editar_Usuario"
                },
                new ApplicationRole {
                    Name = "Detalle_Usuario"
                },
                new ApplicationRole {
                    Name = "Eliminar_Usuario"
                },
                new ApplicationRole {
                    Name = "AllUsuarios"
                }
            };
            PermisosUsuario.ForEach(c => db.Roles.Add(c));
            
            var PermisosGrupo = new List<ApplicationRole> {
                new ApplicationRole {
                    Name = "Agregar_Grupo"
                },
                new ApplicationRole {
                    Name = "Editar_Grupo"
                },
                new ApplicationRole {
                    Name = "Detalle_Grupo"
                },
                new ApplicationRole {
                    Name = "Eliminar_Grupo"
                },
                new ApplicationRole {
                    Name = "AllGrupos"
                }
            };
            PermisosGrupo.ForEach(c => db.Roles.Add(c));
            
            var PermisosAcciones = new List<ApplicationRole> {
                new ApplicationRole {
                    Name = "Agregar_Permiso"
                },
                new ApplicationRole {
                    Name = "Editar_Permiso"
                },
                new ApplicationRole {
                    Name = "Detalle_Permiso"
                },
                new ApplicationRole {
                    Name = "Eliminar_Permiso"
                },
                new ApplicationRole {
                    Name = "AllPermisos"
                }
            };
            PermisosUsuario.ForEach(c => db.Roles.Add(c));

            var PermisosProducto = new List<ApplicationRole> {
                new ApplicationRole {
                    Name = "Agregar_Producto"
                },
                new ApplicationRole {
                    Name = "Editar_Producto"
                },
                new ApplicationRole {
                    Name = "Detalle_Producto"
                },
                new ApplicationRole {
                    Name = "Eliminar_Producto"
                },
                new ApplicationRole {
                    Name = "AllProductos"
                }
            };
            PermisosProducto.ForEach(c => db.Roles.Add(c));
            
            var PermisosCategorias = new List<ApplicationRole> {
                new ApplicationRole {
                    Name = "Agregar_Categoria"
                },
                new ApplicationRole {
                    Name = "Editar_Categoria"
                },
                new ApplicationRole {
                    Name = "Detalle_Categoria"
                },
                new ApplicationRole {
                    Name = "Eliminar_Categoria"
                },
                new ApplicationRole {
                    Name = "AllCategorias"
                }
            };
            PermisosCategorias.ForEach(c => db.Roles.Add(c));
            
            var grupos = new List<ApplicationGroup> {
                new ApplicationGroup {
                    Name = "Gestionar Usuarios",
                    Description = "Gestionar Usuarios"
                },
                new ApplicationGroup {
                    Name = "Gestionar Grupos",
                    Description = "Gestionar Grupos"
                },
                new ApplicationGroup {
                    Name = "Gestionar Acciones",
                    Description = "Gestionar Acciones"
                },
                new ApplicationGroup {
                    Name = "Gestionar Categorias",
                    Description = "Gestionar Categorias"
                },
                new ApplicationGroup {
                    Name = "Gestionar Productos",
                    Description = "Gestionar Productos"
                }
             };
            grupos.ForEach(c => db.ApplicationGroups.Add(c));

            var categoria = new List<Categoria>
            {
                new Categoria() { Nombre = "Camperas", Imagen = "~/Content/img/camperas.jpg"},
                new Categoria() { Nombre = "Zapatos", Imagen = "~/Content/img/zapatos.jpg" },
                new Categoria() { Nombre = "Chombas", Imagen = "~/Content/img/chombas.jpg" },
                new Categoria() { Nombre = "Zapatillas", Imagen = "~/Content/img/zapatillas.jpg" },
                new Categoria() { Nombre = "Remeras", Imagen = "~/Content/img/remeras.jpg" },
                new Categoria() { Nombre = "Pantalones", Imagen = "~/Content/img/pantalones.jpg" }
            };
            categoria.ForEach(c => db.Categorias.Add(c));

            var productos = new List<Producto>
            {
                new Producto() {Nombre = "Campera 1", Precio = 900M, Categoria = categoria[0], ImagenURL = "~/Content/img/campera1.jpg" },
                new Producto() {Nombre = "Campera 2", Precio = 550M, Categoria = categoria[0], ImagenURL = "~/Content/img/campera2.jpg" },
                new Producto() {Nombre = "Zapatos Dolce Gabbana", Precio = 320M, Categoria = categoria[1], ImagenURL = "~/Content/img/zapatos1.jpg" },
                new Producto() {Nombre = "Chomba Azul Hombre", Precio = 560M, Categoria = categoria[2], ImagenURL = "~/Content/img/chomba1.jpg" },
                new Producto() {Nombre = "Zapatillas DG", Precio = 120M, Categoria = categoria[3], ImagenURL = "~/Content/img/zapatillas1.jpg" },
                new Producto() {Nombre = "Zapatillas Nike", Precio = 850M, Categoria = categoria[3], ImagenURL = "~/Content/img/zapatillas2.jpg"  },
                new Producto() {Nombre = "Remera Quicksilver", Precio = 950M, Categoria = categoria[4], ImagenURL = "~/Content/img/remera1.jpg" },
                new Producto() {Nombre = "Remera Duenvolke", Precio = 550M, Categoria = categoria[4], ImagenURL = "~/Content/img/remera2.jpg" },
                new Producto() {Nombre = "Pantalon Levis", Precio = 950M, Categoria = categoria[5], ImagenURL = "~/Content/img/pantalone1.jpg" }
            };
            productos.ForEach(c => db.Productos.Add(c));
            
            db.SaveChanges();
       }
    }

    public class ApplicationSignInManager : SignInManager<ApplicationUser, string>
    {
        public ApplicationSignInManager(ApplicationUserManager userManager, IAuthenticationManager authenticationManager) :
            base(userManager, authenticationManager) { }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(ApplicationUser user)
        {
            return user.GenerateUserIdentityAsync((ApplicationUserManager)UserManager);
        }

        public static ApplicationSignInManager Create(IdentityFactoryOptions<ApplicationSignInManager> options, IOwinContext context)
        {
            return new ApplicationSignInManager(context.GetUserManager<ApplicationUserManager>(), context.Authentication);
        }
    }
}