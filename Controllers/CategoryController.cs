using BLOG.Data;
using BLOG.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BLOG.Controllers
{
    public class CategoryController : Controller
    {
        private readonly AppDBContext _context;
        //private readonly UserManager<IdentityUser> _userManager;

        public CategoryController(AppDBContext context)
        {
            _context = context;
        }
        //public IActionResult Index()
        //{
        //    return View();
        //}


        [HttpPost]
        public async Task<ActionResult> Create(Category category)
        {
            if (category.Name != "")
            {
                await _context.Categories.AddAsync(category);
                await _context.SaveChangesAsync();
                return Redirect("../Admin/ListCategory");
            }

            return View("../Admin/CreateCategory");
        }


        public ActionResult Edit(int id)
        {
            var category = _context.Categories.Find(id);
            return View("../Admin/EditCategory", category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, Category category)
        {
            var edit = _context.Categories.Find(id);

            if (edit != null && edit.Name != category.Name)
            {
                edit.Name = category.Name;
                _context.Categories.Update(edit);
                await _context.SaveChangesAsync();
                return Redirect("../Admin/ListCategory");
            }

            return View("../Admin/CreateCategory");
        }

        public ActionResult Delete(int id)
        {
            var category = _context.Categories.Find(id);
            if (category == null) { return NotFound(); }
            _context.Categories.Remove(category);
            _context.SaveChanges();
            return Redirect("/Admin/ListCategory");
        }

        public ActionResult AssignCategory()
        {
            ViewBag.Posts = _context.Posts.Include(p => p.Categories).ToList();
            ViewBag.Categories = _context.Categories.ToList();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AssignCategory(int id, List<int> SelectedCategories)
        {
            Console.WriteLine("ID POST: "+id);

            var post = await _context.Posts
            .Include(p => p.Categories)
            .FirstOrDefaultAsync(p => p.Id == id);

            if (post == null)
            {
                return NotFound();
            }

            // Limpiar las categorías existentes del post
            post.Categories.Clear();

            Console.WriteLine(" INTENTO ASIGNAR");
            // Añadir las nuevas categorías seleccionadas
            if (SelectedCategories != null)
            {
                foreach (var categoryId in SelectedCategories)
                {
                    Console.WriteLine("ID CATEGORIA: " + categoryId);
                    var category = await _context.Categories.FindAsync(categoryId);
                    if (category != null)
                    {
                        Console.WriteLine("NAME: " + category.Name);
                        post.Categories.Add(category);
                    }
                }
            }

            await _context.SaveChangesAsync();
            
            return Redirect("/Admin/AssignCategory");
        }
    }
}
