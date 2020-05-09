using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace MyAspWeb.Contexts
{
    public class BloggingDbContextFactory : IDesignTimeDbContextFactory<BloggingDbContext>
    {
        public BloggingDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<BloggingDbContext>();
            builder.UseSqlite("Data Source=blog.db");
            return new BloggingDbContext(builder.Options);
        }
    }
}
