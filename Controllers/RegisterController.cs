using BLOG.Data;
using BLOG.Models;
using BLOG.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BLOG.Controllers
{
    public class RegisterController : Controller
    {
        private AppDBContext _appDbContext;
        private readonly UserManager<IdentityUser> _userManager;

        [BindProperty]
        public RegisterDTO registerData {  get; set; }

        public RegisterController(AppDBContext appDBContext, UserManager<IdentityUser> userManager)
        {
            _appDbContext = appDBContext;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            return View("../Auth/Register");
        }

        [HttpPost]
        public async Task<IActionResult> Register()
        {

            /* Validaciones */
            if (!ModelState.IsValid)
            {
                //return View(registerData);
                return View("../Auth/Register");

            }
            if (registerData.Password != registerData.ConfirmPassword)
            {
                throw new Exception("Passwords do not match");
            }

            var user = new IdentityUser();
            user.UserName = registerData.Name;
            user.Email = registerData.Email;

            var res = await _userManager.CreateAsync(user, registerData.Password);
            if (res.Succeeded)
            {
                //await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("Index", "Home"); // Redirige a la página principal después del registro
            }

            foreach (var error in res.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            //return View(registerData);
            return View("../Auth/Register");

            //return View("../Auth/Login");
        }
    }
}
//User user = new User()
//{
//    Name = data.Name,
//    Email = data.Email,
//    Password = data.Password
//};

////await _appDbContext.Users.AddAsync(user);
//await _appDbContext.SaveChangesAsync();
//return View("../Auth/Register");