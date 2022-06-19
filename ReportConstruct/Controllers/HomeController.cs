using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ReportConstruct.Models;

namespace ReportConstruct.Controllers
{
    public class HomeController : Controller
    {
        private readonly ModelContext _context;
        private readonly ILogger<HomeController> _logger;
        private IHostingEnvironment Environment;

        public HomeController(ILogger<HomeController> logger, ModelContext context)
        {
            _logger = logger;
            _context = context;

        }

        public Users Users
        {
            get => default;
            set
            {
            }
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel(HttpContext) { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel user, bool RememberMe)
        {
            if (ModelState.IsValid)
            {


                var md5 = MD5.Create();
                byte[] inputBytes = Encoding.ASCII.GetBytes(user.Password);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                StringBuilder hash = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    hash.Append(hashBytes[i].ToString("X2"));
                }
                var obj = _context.LoginModels.FirstOrDefault(l => l.Login == user.Login && l.Password == hash.ToString());
                if (obj.Role == null)
                {
                    obj.Role = "-";
                }
                if (obj != null)
                {
                    await Authenticate(user.Login, RememberMe, obj.Role);
                    return Redirect("/Home");
                }
                ModelState.AddModelError("", "Некорректные логин и(или) пароль");
            }
            return View(user);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        private async Task Authenticate(string userName, bool rememberMe, string roles)
        {
            // создаем один claim
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, userName),

                new Claim(ClaimsIdentity.DefaultNameClaimType, roles)

            };
          
             
            // создаем объект ClaimsIdentity
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            // установка аутентификационных куки
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(id),
                new AuthenticationProperties
                {
                    IsPersistent = rememberMe
                });
        }



    }
}
