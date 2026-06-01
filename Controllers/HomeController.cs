using System.Diagnostics;
using BLOG.Data;
using BLOG.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace BLOG.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDBContext _context;
        public HomeController(ILogger<HomeController> logger,AppDBContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var lastPost = await _context.Posts
            .OrderByDescending(p => p.createdAt)
            .FirstOrDefaultAsync();

            var featuredPost = await _context.Posts
            .Include(p => p.Comments) // Incluye los comentarios relacionados
            .OrderByDescending(p => p.Comments.Count)
            .Take(4)
            .ToListAsync();

            var latestPosts = await _context.Posts
            .OrderByDescending(p => p.createdAt)
            .Skip(1)
            .Take(4)
            .ToListAsync();

            // FILTRAR POR CATEGOR�AS
            var categoriesWithPosts = await _context.Categories
                .Include(c => c.Posts)
                    .ThenInclude(p => p.User)
                .Include(c => c.Posts)
                    .ThenInclude(p => p.Categories)
                .Where(c => c.Posts.Any(p => p.Published))
                .Select(c => new
                {
                    Category = c,
                    Posts = c.Posts
                        .Where(p => p.Published)
                        .OrderByDescending(p => p.createdAt)
                        .Take(4) // Limitar posts por categor�a
                        .ToList()
                })
                .ToListAsync();



            /* POSTS RANDOM */
            //var totalPosts = await _context.Posts.CountAsync();

            //if (totalPosts <= 4)
            //{
            //    // Si hay 4 o menos posts, devuelve todos
            //    var todosLosPosts = await _context.Posts.ToListAsync();
            //    return View(todosLosPosts);
            //}

            //var random = new Random();
            //var postsAleatorios = await _context.Posts
            //    .OrderBy(p => random.Next()) // Ordena aleatoriamente
            //    .Take(4)
            //    .ToListAsync();

            if (lastPost == null)
            {
                return NotFound(); // O muestra un mensaje si no hay posts
            }
            // ViewBag/ViewData => ACONSEJABLE CREAR UN MODEL DTO PARA TIPARLO 
            ViewBag.LastPost = lastPost;
            ViewBag.FeaturedPost = featuredPost;
            ViewBag.LatestPosts = featuredPost;
            ViewBag.CategoryPosts = categoriesWithPosts;

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
