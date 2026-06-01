using BLOG.Data;
using BLOG.DTOs;
using BLOG.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BLOG.Controllers
{
    public class CommentController : Controller
    {
        private readonly AppDBContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        //[BindProperty]
        //public PostDTO data { get; set; }

        public CommentController(AppDBContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: CommentController
        public ActionResult Index()
        {
            return View();
        }

        // GET: CommentController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CommentController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CommentController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(int PostId , string Content)
        {
            Console.WriteLine("BLOG ID:"+ PostId);
            Console.WriteLine("Content:" + Content);

            if (Content != "")
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    Console.WriteLine("Error: Debes estar autenticado para crear un post.");
                    return Redirect("../Blog/Details/" + PostId);
                }
                var post = _context.Posts.FirstOrDefault(p => p.Id == PostId);
                var comment = new Comment()
                {
                    Content = Content,
                    UserId = user.Id,
                    Author = user,
                    PostId = PostId,
                    Post = post,
                    createdAt = DateTime.Now
                };
                //comment.Content = Content;
                //comment.UserId = user.Id;
                //comment.Author = user;
                //comment.createdAt = DateTime.Now;

                //post.Comments.Add(comment);
                _context.Add(comment);
                int rowsAffected = await _context.SaveChangesAsync();
                if (rowsAffected > 0)
                {
                    Console.WriteLine("INSERTADO CORRECTAMENTE");
                    Redirect("../Blog/Details/" + PostId);
                }
            }
            Console.WriteLine("ERROR VALIDACION");

            return Redirect("../Blog/Details/"+PostId);
        }

        // GET: CommentController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CommentController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CommentController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CommentController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
