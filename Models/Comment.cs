using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;

namespace BLOG.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public required string Content { get; set; }
        
        [ForeignKey("Post")]
        public int PostId { get; set; }

        [ForeignKey("Author")]
        public required string UserId { get; set; }
        public required Post Post { get; set; }
        public required IdentityUser Author { get; set; }
        public DateTime createdAt { get; set; } = DateTime.UtcNow;
    }
}