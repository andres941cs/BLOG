using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography.Xml;
using System.Xml.Linq;

namespace BLOG.Models
{
    public class Post
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public required string Content { get; set; }
        public bool Published { get; set; }
        public string? Image { get; set; }

        [ForeignKey("User")]
        public required string UserId { get; set; }
        //navegacion a usuario.
        public required IdentityUser User { get; set; }
        //navegacion a comentarios.
        public List<Comment>? Comments { get; set; }
        public List<Category>? Categories { get; set; }
        public DateTime createdAt { get; set; } = DateTime.UtcNow;
    }
}
