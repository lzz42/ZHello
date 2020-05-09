using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using MyAspWeb.Contexts;
using MyAspWeb.Models;

namespace MyAspWeb.ViewComponents
{
    [ViewComponent(Name ="BlogList")]
    public class SideBarViewComponent : ViewComponent
    {
        private readonly BloggingDbContext db;

        private IMemoryCache _memoryCache;
        private string CacheKey = "BlogCacheKey";

        public SideBarViewComponent(BloggingDbContext db, IMemoryCache memoryCache)
        {
            this.db = db;
            _memoryCache = memoryCache;
        }

        //public IViewComponentResult Invoke()
        //{
        //    var items = new List<Blog>();
        //    if (_memoryCache.TryGetValue(CacheKey, out items))
        //    {
        //        foreach (var item in db.Blogs)
        //        {
        //            items.Add(item);
        //        }
        //        _memoryCache.Set(CacheKey, items, TimeSpan.FromMinutes(5));
        //    }
        //    return View(items);
        //}

        public IViewComponentResult Invoke(string prefix)
        {
            var items = new List<Blog>();
            if (_memoryCache.TryGetValue(CacheKey, out items))
            {
                foreach (var item in db.Blogs)
                {
                    if (item.BlogName.StartsWith(prefix))
                    {
                        items.Add(item);
                    }
                }
                _memoryCache.Set(CacheKey, items, TimeSpan.FromMinutes(5));
            }
            return View(items);
        }

        //public IViewComponentResult InvokeAsync()
        //{
        //    return View();
        //}
    }
}