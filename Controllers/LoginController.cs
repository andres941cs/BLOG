using System.Drawing;
using BLOG.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BLOG.Controllers
{
    public class LoginController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        [BindProperty]
        public LoginDTO LoginData { get; set; }
        public LoginController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public IActionResult Index()
        {
            return View("../Auth/Login");
        }

        [HttpPost]
        public async Task<IActionResult> Login()
        {
            var user = await _userManager.Users
               .Where(u => u.UserName == LoginData.Email || u.Email == LoginData.Email)
               .FirstOrDefaultAsync();
            if (user == null)
            {
                ModelState.AddModelError("", "Usuario o Email no existe.");
                return View("../Auth/Login");
            }
            var res = await _signInManager.PasswordSignInAsync(LoginData.Email, LoginData.Password,true,true);//TERCER GUARDAR COOKIESI,BLOQUE SI SE FALLA MUCHOS INTENTOS
            //return LocalRedirect("/");

            if(res.Succeeded)
            {
                return RedirectToAction("Index", "Home"); // Redirige al inicio
            }

            ModelState.AddModelError("", "Usuario o contraseña incorrectos.");
            return View("../Auth/Login");
        }

        [HttpPost]
        [ValidateAntiForgeryToken] // Protección contra ataques CSRF
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home"); // Redirige a la página de inicio
        }
    }
}
