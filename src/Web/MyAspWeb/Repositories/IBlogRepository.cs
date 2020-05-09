using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyAspWeb.Contexts;
using MyAspWeb.Models;

namespace MyAspWeb.Repositories
{
    public interface IBlogRepository
    {
        IEnumerable<Blog> ListAll();
        void Add(Blog blog);
        void Remove(Blog blog);
    }
}
