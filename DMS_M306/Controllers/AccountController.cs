using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using DMS_M306.Interfaces;
using DMS_M306.ViewModels.Account;
using DMS_M306.Models;
using DMS_M306.Interfaces.Repositories;
using System.Linq;

namespace DMS_M306.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private IUserManager _userManager;
        private IUserRepository _userRepository;
        private IUnitOfWork _unitOfWork;

        public AccountController(IUserManager userManager, IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _userRepository = userRepository;
        }
        
        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(ViewModels.Account.LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                var firstUser = _userRepository.Get().FirstOrDefault();
                if(firstUser == null)
                {
                    Models.User user = new Models.User()
                    {
                        Email = "demo.user@dms.com",
                        FirstName = "Demo",
                        LastName = "User",
                        Password = "12345",
                        Role = Enums.UserRoles.Management,
                        UserName = "demo.user"
                    };
                    _userRepository.Add(user);
                    _unitOfWork.SaveChanges();
                }
                return View(model);
            }

            var ident = _userManager.GetIdentityByLoginCredentials(model.UserName, model.Password);

            if (ident != null)
            {
                HttpContext.GetOwinContext().Authentication.SignIn(
                   new AuthenticationProperties { IsPersistent = model.RememberMe }, ident);
                return RedirectToAction("Index","Home"); // auth succeed 
            }
            // invalid username or password
            ModelState.AddModelError("", "invalid username or password");

            return View(model);

        }
                
        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            HttpContext.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(CreateUserViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            Models.User user = new Models.User()
            {
                Email = vm.Email,
                FirstName = vm.FirstName,
                LastName = vm.LastName,
                UserName = vm.UserName,
                Password = vm.Password,
                Role = vm.Role
            };

            _userRepository.Add(user);
            _unitOfWork.SaveChanges();

            return RedirectToAction("Index", "Home");
        }
    }
}