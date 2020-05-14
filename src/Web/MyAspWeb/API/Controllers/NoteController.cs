using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyAspWeb.Models;
using MyAspWeb.Repositories;
using MyAspWeb.ViewModels;

namespace MyAspWeb.API.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class NoteController : Controller
    {
        private INoteRepository _repo;
        private INoteTypeRepository _typerepo;

        public NoteController(INoteRepository repo, INoteTypeRepository typerepo)
        {
            _repo = repo;
            _typerepo = typerepo;
        }

        [HttpGet("{pageindex?}")]
        public IActionResult Get(int pageindex = 1)
        {
            Console.WriteLine("api call ===> Get Index = " + pageindex);
            var pagesize = 10;
            var notes = _repo.PageList(pageindex, pagesize);
            ViewBag.PageCount = notes.Item2;
            ViewBag.PageIndex = pageindex;
            var result = notes.Item1.Select(r => new NoteViewModel()
            {
                Id = r.Id,
                Title = string.IsNullOrEmpty(r.Password) ? r.Title : "加密的标题",
                Content = string.IsNullOrEmpty(r.Password) ? r.Content : "加密的内容",
                Type = r.Type.Name,
                Attachment = string.IsNullOrEmpty(r.Password) ? r.Attachment : "",
            });
            return Ok(result);
        }

        [HttpGet("{id}/{password}")]
        [HttpGet("{id}/{psw}")]
        public async Task<IActionResult> Detail(int id,string password)
        {
            Console.WriteLine(string.Format("api call ===> Detail id ={0};password={1} ", id, password));
            var note =await _repo.GetByIdAsync(id);
            if (note == null)
                return NotFound();
            if (!string.IsNullOrEmpty(note.Password) && !note.Password.Equals(password))
                return Unauthorized();
            var result = new NoteViewModel()
            {
                Id = note.Id,
                Title = note.Title,
                Content = note.Content,
                Type = note.Type.Name,
                Attachment = note.Attachment,
            };
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] NoteModel model)
        {
            Console.WriteLine(string.Format("api call ===> Post Model ID ={0};Title={1} ", model.Id, model.Title));
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var fn = string.Empty;
            await _repo.AddAsync(new Note()
            {
                Title = model.Title,
                Content = model.Content,
                CreateTime = DateTime.Now,
                TypeId = model.TypeId,
                Password = model.Password,
                Attachment = fn,
            });
            return CreatedAtAction("Index", "");
        }

        [HttpDelete]
        public IActionResult DeleteAll()
        {
            Console.WriteLine(string.Format("api call ===> DeleteAll"));
            _repo.DeleteAll();
            _typerepo.DeleteAll();
            return Ok();
        }

    }
}
