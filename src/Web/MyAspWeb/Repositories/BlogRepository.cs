using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyAspWeb.Contexts;
using MyAspWeb.Models;

namespace MyAspWeb.Repositories
{
    public class BlogRepository : IBlogRepository
    {
        public BloggingDbContext Ctx { get; private set; }

        public BlogRepository(BloggingDbContext ctx)
        {
            Ctx = ctx;
        }

        public void Add(Blog blog)
        {
            if (Ctx.Blogs == null)
            {
                return;
            }
            Ctx.Blogs.Add(blog);
            Ctx.SaveChanges();
        }

        public IEnumerable<Blog> ListAll()
        {
            if (Ctx.Blogs == null)
            {
                return null;
            }
            return Ctx.Blogs.ToList();
        }

        public void Remove(Blog blog)
        {
            if (Ctx.Blogs == null)
            {
                return;
            }
            Ctx.Blogs.Remove(blog);
            Ctx.SaveChanges();
        }
    }

}
