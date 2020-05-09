using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MyAspWeb.Attributes;
using MyAspWeb.Contexts;
using MyAspWeb.Filters;
using MyAspWeb.Models;
using MyAspWeb.Routers;

namespace MyAspWeb.Controllers
{
    /// <summary>
    /// Code First
    /// </summary>
    [Route("Blog")]
    [Route("[controller]/[action]")]//标记替换
    [MyRoute()]
    [AddHeaderFilter("Blog","Class")]
    public class BlogController : Controller
    {
        private BloggingDbContext _ctx;
        private GameSetting _setting;
        public BlogController(BloggingDbContext ctx,IOptions<Models.GameSetting> options)
        {
            _ctx = ctx;
            _setting = options.Value;
        }

        [Route("")]
        [Route("/")]
        [Route("Index")]
        [HttpGet]
        [ShortCircuitingResourceFilter]
        public IActionResult Index()
        {
            if(_setting!=null)
            {
                ViewData["GameSetting"] = _setting;
            }
            PopulateCharactersIfNotExist();
            if (_ctx.Blogs.Any())
            {
                ViewData["Tilte"] = "浏览Blog";
                return View("Index", _ctx.Blogs.ToList());
            }
            else
            {
                var url = Url.RouteUrl("Register");
                ViewData["Tip"] = $"Goto {url}";
                ViewData["Goto"] = url;
                return View("Index", _ctx.Blogs.ToList());
            }
        }

        [Route("About")]
        [HttpGet]
        [TypeFilter(typeof(AddHeaderFilterAttribute),Arguments =new object[] { "BlogAuthor","Steve Hocking"})]
        public IActionResult About()
        {
            ViewData["Tilte"] = "关于Blog";
            ViewData["Message"] = "Here is the message content...";
            return View("About");
        }

        [Route("Register")]
        [HttpGet]
        [ServiceFilter(typeof(AddHeaderFilterAttribute))]
        public IActionResult Register()
        {
            ViewData["Title"] = "注册Blog";
            return View("Register");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Register")]
        public IActionResult Register(Blog blog)
        {
            if (ModelState.IsValid)
            {
                _ctx.Blogs.Add(blog);
                _ctx.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(blog);
        }

        #region 内部方法

        private void PopulateCharactersIfNotExist()
        {
            if (_ctx.Blogs.Any())
            {
                return;
            }
            _ctx.Blogs.Add(new Blog()
            {
                BlogId = 1024,
                BlogName = "EN Alice",
                Url = "https://www.cnblogs.com/",
            });
            _ctx.Blogs.Add(new Blog()
            {
                BlogId = 1025,
                BlogName = "En Blice",
                Url = "https://www.github.com/",
            });
            _ctx.Blogs.Add(new Blog()
            {
                BlogId = 1026,
                BlogName = "US Clice",
                Url = "https://www.sina.com/",
            });
            _ctx.Blogs.Add(new Blog()
            {
                BlogId = 1027,
                BlogName = "US Dlice",
                Url = "https://www.souhu.com/",
            });
            _ctx.Blogs.Add(new Blog()
            {
                BlogId = 1028,
                BlogName = "CN Li",
                Url = "https://blog.csdn.net/",
            });
            _ctx.Blogs.Add(new Blog()
            {
                BlogId = 1029,
                BlogName = "CN Chen",
                Url = "https://news.cnblogs.com/",
            });
            _ctx.SaveChanges();
        }

        #endregion 内部方法
    }
}