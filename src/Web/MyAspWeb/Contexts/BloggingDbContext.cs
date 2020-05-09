using Microsoft.EntityFrameworkCore;
using MyAspWeb.Models;

namespace MyAspWeb.Contexts
{
    public class BloggingDbContext : DbContext
    {
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Post> Posts { get; set; }

        public BloggingDbContext(DbContextOptions<BloggingDbContext> options)
            : base(options)
        {
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlite(@"Filename=./blogging.db");
        //    //base.OnConfiguring(optionsBuilder);
        //}
    }
}