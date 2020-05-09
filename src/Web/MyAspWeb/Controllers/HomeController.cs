using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MyAspWeb
{
    [Route("Home")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly Services.ISystemTime _time;
        /// <summary>
        /// 构造函数依赖注入
        /// </summary>
        /// <param name="logger"></param>
        public HomeController(ILogger<HomeController> logger, Services.ISystemTime time)
        {
            _logger = logger;
            _time = time;
        }

        [Route("")]
        [Route("/")]
        [Route("Index")]
        public IActionResult Index()
        {
            _logger.LogInformation("访问主页");
            _logger.LogDebug("调试信息");
            _logger.LogWarning("警告信息");
            _logger.LogError("错误信息");
            _logger.LogCritical("崩溃信息");
            ViewData["Time"] = _time.Now.ToString("yyyy-MM-dd") +" - From Constructor Injection";
            return View();
        }

        [Route("About")]
        public IActionResult About([FromServices]Services.ISystemTime time)
        {
            ViewData["Time"] = _time.Now.ToString("yyyy-MM-dd")+" - FromService";
            ViewData["Message"] = "主页描述信息";
            return View();
        }

        [Route("Contact")]
        public IActionResult Contact()
        {
            ViewData["Message"] = "主页联系方式";
            return View();
        }

        [Route("Error")]
        public IActionResult Error()
        {
            return View();
        }
    }

    /*
     依赖注入 Dependency Injection DI
     一种实现对象与合作者、依赖项之间松散耦合的技术
     将类的对象以某种方式提供给使用者，而不是使用者直接实例化或者使用静态引用的方式使用
     通常使用者通过构造函数声明依赖关系，允许遵循显式依赖原则 Explicit Dependencies Principle,即构造函数注入 constructor injection
     在构造时提供抽象（通常为接口），而不是特定的实现
     容器container ：用来创建构造注入的使用者类和被注入的被使用者实例，IoC 控制反转 或者依赖注入容器

     */
}