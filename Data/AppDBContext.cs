using BLOG.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
namespace BLOG.Data
{
    public class AppDBContext: IdentityDbContext<IdentityUser>
    {
        public AppDBContext(DbContextOptions<AppDBContext> options):base(options)
        {
            
        }
        //public DbSet<User> Users { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Comment> Comments { get; set; }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    base.OnModelCreating(modelBuilder);

        //    modelBuilder.Entity<User>(tb =>
        //    {
        //        tb.HasKey(col => col.Id);
        //        tb.Property(col => col.Id)
        //        .UseIdentityColumn()
        //        .ValueGeneratedOnAdd();
        //        tb.HasIndex(col => col.Email).IsUnique();
        //        tb.Property(col => col.Name).HasMaxLength(50);
        //        tb.Property(col => col.Email).HasMaxLength(100);
        //        tb.Property(col => col.Password).HasMaxLength(50);
        //    });
        //}
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Post>()
           .HasMany(p => p.Categories)
           .WithMany(c => c.Posts);

            // 1. Crear usuarios de prueba
            //var user1 = new IdentityUser { Id = "1", UserName = "admin", Email = "admin@gmail.com" };
            //var user2 = new IdentityUser { Id = "2", UserName = "andres", Email = "andres@example.com" };

            //var passwordHasher = new PasswordHasher<IdentityUser>();
            //user1.PasswordHash = passwordHasher.HashPassword(user1, "Admin123");
            //user2.PasswordHash = passwordHasher.HashPassword(user2, "P@$$wOrd123");

            //builder.Entity<IdentityUser>().HasData(user1, user2);

            //// Seed data para Posts

            //var post1 = new Post { Id = 1, Title = "Oregairu Zoku", Image = "img/0c173125-7bd8-45d7-9188-a6a1ff93163a.png", Content = "Es un segundo videojuego desarrollado y publicado por 5pb que se basa libremente en la serie de novelas ligeras Yahari Ore no Seishun Love Come wa Machigatteiru. Fue lanzado el 27 de octubre de 2016 en Japón. El segundo OVA se incluyó con ediciones especiales del juego.", createdAt = DateTime.UtcNow.AddDays(-10), UserId = "1", User = user1 };
            //var post2 = new Post { Id = 2, Title = "Date a Live Rio Reincarnation", Image = "img/68bf259a-f43f-4cee-9cb4-d2917ab3951a.png", Content = "Este es el segundo post de prueba.", createdAt = DateTime.UtcNow.AddDays(-5), UserId = "1", User = user1 };

            //builder.Entity<Post>().HasData(post1, post2);
            ////// Seed data para Comments (asegúrate de que los PostId y UserId existan)
            //builder.Entity<Comment>().HasData(
            //    new Comment { Id = 1, PostId = 1, Post = post1, UserId = "1", Author= user1, Content = "Comentario de prueba en el primer post.", createdAt = DateTime.UtcNow.AddDays(-8) },
            //    new Comment { Id = 2, PostId = 1, Post = post1, UserId = "2", Author = user2, Content = "Otro comentario.", createdAt = DateTime.UtcNow.AddDays(-7) },
            //    new Comment { Id = 3, PostId = 2, Post = post2, UserId = "2", Author = user2, Content = "Comentario en el segundo post.", createdAt = DateTime.UtcNow.AddDays(-3) }
            //);


        }
    }
}
