using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyAspWeb.Models;
using MyAspWeb.Repositories;

namespace MyAspWeb.Services
{
    public class BlogStatisticsService
    {
        private readonly IBlogRepository Repo;

        public BlogStatisticsService(IBlogRepository repo)
        {
            Repo = repo;
        }

        public List<Blog> ListBolgs()
        {
            return Repo.ListAll().ToList();
        }

        public int GetCount()
        {
            return Repo.ListAll().Count();
        }

        public void Add(Blog blog)
        {
            Repo.Add(blog);
        }

        public void Remove(Blog blog)
        {
            Repo.Remove(blog);
        }

        public bool Exists(Blog blog)
        {
            return Repo.ListAll().Any(b => b.BlogId == blog.BlogId);
        }

    }
}
