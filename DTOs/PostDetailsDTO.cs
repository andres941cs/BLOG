using BLOG.Models;

namespace BLOG.DTOs
{
    public class PostDetailsDTO
    {
        public Post Post { get; set; }
        public Comment newComment { get; set; } 
                                                        
    }
}
