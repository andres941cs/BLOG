using BLOG.Data;
using BLOG.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BLOG.Controllers
{
    public class AdminController : Controller
    {
        private readonly AppDBContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public AdminController(AppDBContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            int totalUser = await _context.Users.CountAsync();
            int totalPost = await _context.Posts.CountAsync();
            int totalCategory = await _context.Categories.CountAsync();

            ViewBag.TotalUser = totalUser;
            ViewBag.TotalPost = totalPost;
            ViewBag.TotalCategory = totalCategory;

            return View();
        }

        public ActionResult AddBlog()
        {
            return PartialView("../Blog/Create");
        }

        public async Task<ActionResult> ListBlog()
        {
            var posts = await _context.Posts.ToListAsync();
            foreach (var post in posts)
            {
                Console.WriteLine($"ID: {post.Id}, Título: {post.Title}, Contenido: {post.Content}");
            }
            return View(posts);
            //return PartialView("../Admin/ListBlog");
        }

        public ActionResult AddUser()
        {
            return View("../Admin/CreateUser");
        }

        public async Task<ActionResult> ListUser()
        {
            var users = await _context.Users.ToListAsync();
            return View(users);
        }

        public ActionResult AddCategory()
        {
            return View("../Admin/CreateCategory");
        }

        public async Task<ActionResult> ListCategory()
        {
            var categories = await _context.Categories.ToListAsync();
            return View(categories);
        }


        public async Task<IActionResult> AssignCategory()
        {
            ViewBag.Posts = await _context.Posts
                .Include(p => p.Categories)
                .ToListAsync();

            ViewBag.Categorias = await _context.Categories.ToListAsync();

            return View();
        }
    }
}
