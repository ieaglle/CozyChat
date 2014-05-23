using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using CozyChat.Model;
using CozyChat.Web.Repositories;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using CozyChat.Web.Models;

namespace CozyChat.Web.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly ICozyChatRepository _repo;

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        public AccountController(ICozyChatRepository repo)
        {
            _repo = repo;
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = await _repo.CheckLoginAsync(model.UserName, model.Password);
                if (user != null)
                {
                    SignIn(user, model.RememberMe);
                    return RedirectToLocal(returnUrl);
                }
                ModelState.AddModelError("", "Invalid username or password.");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _repo.RegisterUserAsync(model.UserName, model.Password);
                if (result != null)
                {
                    SignIn(result, false);

                    return RedirectToAction("Rooms", "Home");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Rooms", "Home");
        }

        private void SignIn(User user, bool remember)
        {
            var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.Name),
                        new Claim(ClaimTypes.IsPersistent, remember.ToString()),
                        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
                    };
            var id = new ClaimsIdentity(claims,
                    DefaultAuthenticationTypes.ApplicationCookie);
            AuthenticationManager.SignIn(new AuthenticationProperties{IsPersistent = remember}, id);
        }

        #region Helpers

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);
            return RedirectToAction("Rooms", "Home");
        }

        #endregion
    }
}