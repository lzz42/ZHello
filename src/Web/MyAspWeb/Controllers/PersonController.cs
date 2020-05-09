using Microsoft.AspNetCore.Mvc;
using MyAspWeb.Models;

namespace MyAspWeb.Controllers
{
    [Route("Person")]
    public class PersonController : Controller
    {
        [Route("Index")]
        public IActionResult Index()
        {
            Person p = new Person();
            if (Request.Form.Count > 0)
            {
                p.Id = Request.Form["Id"];
                p.Name = Request.Form["Name"];
                p.Age = int.Parse(Request.Form["Age"]);
                p.Address = Request.Form["Address"];
                //TryUpdateModel(p);
                ViewBag.StatusMessage = p.Name + "  编号是" + p.Id + "!";
            }
            return View();
        }

        public IActionResult Index2()
        {
            return View();
        }

        [HttpPost]
        [Route("Index2")]
        public IActionResult Index2(Person p)
        {
            ViewBag.StatusMessage = p.Name + "  编号是" + p.Id + "!";
            return View();
        }

        [AcceptVerbs("Get","Post")]
        public IActionResult VerifyEmail(string email)
        {
            var r = false;
            if (!string.IsNullOrEmpty(email))
            {
                if (email.Contains('@'))
                {
                    r = true;
                }
            }
            return Json(data: r);
        }

    }
}