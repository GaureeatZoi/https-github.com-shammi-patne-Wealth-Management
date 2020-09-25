using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ComponentSpace.Saml2;
//using ComponentSpace.Saml2;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ZOI.APP.Models;
using ZOI.BAL.Models;

namespace ZOI.APP.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly ISamlServiceProvider _samlServiceProvider;
        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ISamlServiceProvider samlServiceProvider, IConfiguration configuration)
      //    public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,  IConfiguration configuration)
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
            this._samlServiceProvider = samlServiceProvider;
            this._configuration = configuration;
        }
        public IActionResult Index()
        {
            LoginViewModel model = new LoginViewModel();
            return View(model);
        }

        public async Task<IActionResult> Login(string returnUrl = null)
        {
            var partnerName = _configuration["PartnerName"];
            // To login automatically at the service provider,
            // initiate single sign-on to the identity provider (SP-initiated SSO).            
            // The return URL is remembered as SAML relay state.
            await _samlServiceProvider.InitiateSsoAsync(partnerName, returnUrl);

            return new EmptyResult();

            //LoginViewModel model = new LoginViewModel();
            //return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError(string.Empty, "Invalid Login Attempt");
            }

            return View("Index", model);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email, IsActive = true, CreatedOn = DateTime.Now };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Logout(string returnUrl = null)
        {
            LoginViewModel loginViewModel = new LoginViewModel();
            //var partnerName = _configuration["PartnerName"];
            //await _samlServiceProvider.InitiateSloAsync(partnerName,"Application Logout");

            //return new EmptyResult();
            //var user = await _userManager.FindByEmailAsync(User.Identity.Name);
            var prop = new AuthenticationProperties()
            {
                RedirectUri = "/LogOut"
            };
            await _signInManager.SignOutAsync();

            //await HttpContext.SignOutAsync();
            //await HttpContext.SignOutAsync("oidc", prop);

            // await _signInManager.SignOutAsync();
            return View("LogOut");
            //return RedirectToAction("LogOut", "Account");
            //var sloResult = await _samlServiceProvider.ReceiveSloAsync();

            //if (sloResult.IsResponse)
            //{
            //    // SP-initiated SLO has completed.
            //    if (!string.IsNullOrEmpty(sloResult.RelayState))
            //    {
            //        return LocalRedirect(sloResult.RelayState);
            //    }

            //    return RedirectToAction("Login", "Account");
            //}
            //else
            //{
            //    // Logout locally.
            //    await _signInManager.SignOutAsync();

            //    // Respond to the IdP-initiated SLO request indicating successful logout.
            //    await _samlServiceProvider.SendSloAsync();
            //}

            //return new EmptyResult();
        }
    }
}