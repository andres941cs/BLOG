using BLOG.Models;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace BLOG.DTOs
{
    public class PostDTO
    {
        public required string Title { get; set; }
        public required string Content { get; set; }
        public bool Published { get; set; }
        public string? Image { get; set; }
        public List<Comment>? Comments { get; set; }
        public DateTime createdAt { get; set; } = DateTime.UtcNow;
    }
}
