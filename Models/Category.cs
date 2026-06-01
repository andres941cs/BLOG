using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace BLOG.Models
{
    public class Category
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public List<Post>? Posts { get; set; }
        public DateTime createdAt { get; set; } = DateTime.UtcNow;
    }
}
