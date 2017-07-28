using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using SeguridadWebv2.Models;
using SeguridadWebv2.Controllers;
using SeguridadWebv2;
using Moq;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Web.Mvc;
using System.Web;
using System.Web.Routing;

namespace TestSeguridad
{
    [TestClass]
    public class AccountControllerTest
    {
        [TestMethod]
        public void TestIngresoExitosoLogin()
        {
            //Arrange
            var userStore = new Mock<IUserStore<ApplicationUser>>();
            var userManager = new Mock<ApplicationUserManager>(userStore.Object);
            var authenticationManager = new Mock<IAuthenticationManager>();
            var signInManager = new Mock<ApplicationSignInManager>(userManager.Object, authenticationManager.Object);
            
            
            signInManager.Setup(s => s.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>()))
                .Returns<string, string, bool, bool>(MockPasswordSignInAsync);
            
            
            var controller = new AccountController(userManager.Object, signInManager.Object);


            var contextMock = new Mock<HttpContextBase>();
            controller.Url = new UrlHelper(new RequestContext(contextMock.Object, new RouteData()));
            
            var loginViewmodel = new LoginViewModel
            {
                Email = "administrador@mcga.com",
                Password = "Mcga@123456",
                RememberMe = false
            };
            var returnUrl = "/Home/Index";
            
            //Act
            var result = controller.Login(loginViewmodel, returnUrl);

            //Assert
            
            Assert.IsInstanceOfType(result.Result, typeof(RedirectResult));
            Assert.AreEqual(returnUrl, (result.Result as RedirectResult).Url);

        }

        [TestMethod]
        public void TestLoginIngresoFallido()
        {
            var userStore = new Mock<IUserStore<ApplicationUser>>();
            var userManager = new Mock<ApplicationUserManager>(userStore.Object);
            var authenticationManager = new Mock<IAuthenticationManager>();
            var signInManager = new Mock<ApplicationSignInManager>(userManager.Object, authenticationManager.Object);


            signInManager.Setup(s => s.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>()))
                .Returns<string, string, bool, bool>(MockPasswordSignInAsync);


            var controller = new AccountController(userManager.Object, signInManager.Object);


            var contextMock = new Mock<HttpContextBase>();
            controller.Url = new UrlHelper(new RequestContext(contextMock.Object, new RouteData()));

            var loginViewmodel = new LoginViewModel
            {
                Email = "admin@gmail.com",
                Password = "Mcga@789456",
                RememberMe = false
            };
            var returnUrl = "/Home/Index";

            //Act
            var result = controller.Login(loginViewmodel, returnUrl);

            //Assert

            Assert.IsInstanceOfType(result.Result, typeof(ViewResult));
            Assert.AreEqual("Usuario o Clave incorrectos", (result.Result as ViewResult).ViewData.ModelState[""].Errors[0].ErrorMessage);

        }

        private async Task<SignInStatus> MockPasswordSignInAsync(string usuario, string clave, bool Persiste, bool souldLockout)
        {
            if (usuario == "administrador@mcga.com" && clave == "Mcga@123456")
            {
                return SignInStatus.Success;
            }
            else
            {
                return SignInStatus.Failure;
            }
        }
    }
}
