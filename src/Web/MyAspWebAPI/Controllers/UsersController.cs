using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MyAspWebAPI.Controllers
{
    [Route("[controller]")]
    public class UsersController:Controller
    {
        public List<User> Users { get; set; }
        public UsersController()
        {
            for (int i = 0; i < 10; i++)
            {
                var user = new User()
                {
                    Name = "Alice-" + i.ToString(),
                    Age = i * 10 + 3,
                    Address = "Brokelin === " + i.ToString(),
                    Hight = 1.80f + i,
                    IsAlive = (~i & (i - 1)) == 1,
                };
            }
        }

        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "Alice", "Budy", "Candy", "Doubu", "Eson" };
        }

        [HttpGet("{id}")]
        [Route("[action]/{id}.{format?}")]
        public string Get(int id)
        {
            return $"get 42--->【{id}】";
        }

        [Route("[action]")]
        [Produces("application/proto")]
        public IEnumerable<User> GetP()
        {
            return Users;
        }

        [HttpGet("{id}")]
        [Route("GetJ")]
        public JsonResult GetJ(string id)
        {
            return Json($"get json 42--->【{id}】");
        }

        [HttpGet("{id}/{key}")]
        [Route("GetCK")]
        [Route("GetCK/{id}")]
        [Route("GetCK/{id}/{key}")]
        public ContentResult GetCK(string id,string key)
        {
            return Content($"get content 42--->【{id}】--->【{key}】");
        }

        [HttpPost()]
        public string Post([FromBody] string value)
        {
            return $"post 42--->【{value}】";
        }


        [HttpPost("{id}")]
        public string Post(int id,[FromBody] string value)
        {
            return $"post 42 ---> 【{id}】--->【{value}】";
        }

        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
            Console.WriteLine($"put 42 ---> 【{id}】--->【{value}】");
        }
        [HttpDelete("{id}")]
        public void Delete(int id, [FromBody] string value)
        {
            Console.WriteLine($"delete 42 ---> 【{id}】--->【{value}】");
        }

    }
}
