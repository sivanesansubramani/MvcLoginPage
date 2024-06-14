using Microsoft.AspNetCore.Mvc;

using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Leavemanagement.Models;



namespace Leavemanagement.Controllers
{
    public class AccessController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            ClaimsPrincipal claimUser = HttpContext.User;

            if (claimUser.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Dashboard");


            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(Loginmodel modelLogin)
        {
            if (modelLogin.Email == "hello@gmail.com" && modelLogin.PassWord == "123")
            {
                List<Claim> claims = new List<Claim>() {
            new Claim(ClaimTypes.NameIdentifier, modelLogin.Email),
            new Claim(ClaimTypes.NameIdentifier, modelLogin.PassWord)
             };

                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                AuthenticationProperties properties = new AuthenticationProperties()
                {
                    AllowRefresh = true,
                    IsPersistent = modelLogin.KeepLoggedIn
                };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity), properties);

                // Store user details in session
                HttpContext.Session.SetString("UserName", modelLogin.Email);
                HttpContext.Session.SetString("Password", modelLogin.PassWord);

                // You can store other details similarly

                return RedirectToAction("Index", "Dashboard");
            }

            ViewData["ValidateMessage"] = "User not found";
            return View();
        }


    }
}
