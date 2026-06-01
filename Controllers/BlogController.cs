using System.Reflection.Metadata;
using BLOG.Data;
using BLOG.DTOs;
using BLOG.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace BLOG.Controllers
{
    public class BlogController : Controller
    {
        private readonly AppDBContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IWebHostEnvironment _environment;

        [BindProperty]
        public PostDTO data {  get; set; }

        public BlogController(AppDBContext context, UserManager<IdentityUser> userManager, IWebHostEnvironment environment)
        {
            _context = context;
            _userManager = userManager;
            _environment = environment;
        }
        // GET: BlogController
        public async Task<ActionResult> Index()
        {
            var posts = await _context.Posts.ToListAsync();
            foreach (var post in posts)
            {
                Console.WriteLine($"ID: {post.Id}, Título: {post.Title}, Contenido: {post.Content}");
            }
            return View(posts);
        }

        // GET: BlogController/Details/5
        //public ActionResult Details(int id)
        //{
        //    //var blog = _context.Posts.Find(id);

        //    var blog =  _context.Posts
        //    .Include(p => p.Comments)
        //    .FirstOrDefaultAsync(p => p.Id == id);
        //    return View(blog);
        //}

        // GET: BlogController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            //var blog = _context.Posts.Find(id);

            var post = await _context.Posts
            .Include(p => p.Comments)
            .ThenInclude(c => c.Author)
            .FirstOrDefaultAsync(p => p.Id == id);

            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // GET: BlogController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: BlogController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(PostDTO data, IFormFile Image)
        {
            
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                Console.WriteLine("Error: Debes estar autenticado para crear un post.");
                return View(data);
            }
            //post.createdAt = DateTime.Now;

            Console.WriteLine("ITENTANDO CREAR POST");
            Console.WriteLine("imagen"+ Image.ContentType);

            if (ModelState.IsValid)
            {
                //if (Imagen != null && Imagen.Length > 0)
                //{
                // Validación del archivo
                Console.WriteLine("imagen: " + Image.ContentType.ToLower());
                if (Image.ContentType.ToLower() != "image/jpeg" && Image.ContentType.ToLower() != "image/png")
                {
                    ModelState.AddModelError("Imagen", "Solo se permiten archivos JPEG o PNG.");
                    return View(data);
                }

                    // Guarda el archivo
                string pathFolder = Path.Combine(_environment.WebRootPath, "img");
                string nameFile = Guid.NewGuid().ToString() + Path.GetExtension(Image.FileName);
                string rutaArchivo = Path.Combine(pathFolder, nameFile);

                using (var stream = new FileStream(rutaArchivo, FileMode.Create))
                {
                    await Image.CopyToAsync(stream);
                }

                
                //}
                Post post = new Post
                {
                    Title = data.Title,
                    Content = data.Content,
                    Published = data.Published,
                    Image = "/img/" + nameFile,
                    UserId = user.Id,
                    User = user,
                    createdAt = DateTime.UtcNow
                };

                _context.Add(post);
                int rowsAffected = await _context.SaveChangesAsync();
                if (rowsAffected > 0)
                {
                    Console.WriteLine("INSERTADO CORRECTAMENTE");
                    return Redirect("../Admin/ListBlog");
                }
            }
            Console.WriteLine("ERROR VALIDACION");

            return View("../Blog/Create");
        }

        // GET: BlogController/Edit/5
        public ActionResult Edit(int id)
        {
            var blog = _context.Posts.Find(id);
            return View(blog);
        }

        // POST: BlogController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Post post)
        {        
            //if (ModelState.IsValid)
            //{
                var blog = _context.Posts.Find(id);
                if (blog == null) { return NotFound();}

                blog.Title = post.Title;
                blog.Content = post.Content;
                blog.Published = post.Published;
                blog.Image = post.Image;

                var res = _context.Update(blog);
                _context.SaveChanges();
            //}
            return View();

        }

        public ActionResult Delete(int id)
        {
            var blog = _context.Posts.Find(id);
            if (blog == null) { return NotFound(); }
            _context.Posts.Remove(blog);
            _context.SaveChanges();
            return Redirect("/Admin/ListBlog");
        }

        // POST: BlogController/Delete/5
        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var blog = _context.Posts.Find(id);
            if (blog == null) { return NotFound(); }
            _context.Posts.Remove(blog);
            _context.SaveChanges();
            return Redirect("../Admin/ListBlog");
        }
    }
}
