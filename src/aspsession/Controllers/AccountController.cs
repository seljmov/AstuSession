using aspsession.Contexts;
using aspsession.Models;
using aspsession.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace aspsession.Controllers
{
    /// <summary>
    /// Контроллер аккаунтов
    /// </summary>
    public class AccountController : Controller
    {
        private MySessionContext _context;

        /// <summary>
        /// Конструктор класса <see cref="AccountController"/>
        /// </summary>
        /// <param name="context">Контекст базы данных</param>
        public AccountController(MySessionContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Страница логина
        /// </summary>
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        /// <summary>
        /// Страница логина
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel data)
        {
            if (ModelState.IsValid)
            {
                var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == data.Email);
                if (user == null)
                {
                    ModelState.AddModelError("", "Пользователя с таким Email не существует.");
                }
                else
                {
                    if (user.Password != data.Password)
                    {
                        ModelState.AddModelError("", "Введен неправильный пароль.");
                    }
                    else
                    {
                        var role = _context.Roles.First(x => x.Id == user.RoleId).Name;
                        await Authenticate(user.Email, role);
                        return RedirectToAction("Index", "Home");
                    }
                }
            }

            return View(data);
        }

        /// <summary>
        /// Страница выхода из системы
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }

        /// <summary>
        /// Авторизация пользователя
        /// </summary>
        private async Task Authenticate(string email, string role)
        {
            var claims = new List<Claim> 
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, email),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, role),
            };

            var id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }
    }
}
