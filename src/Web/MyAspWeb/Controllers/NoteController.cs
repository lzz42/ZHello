using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MyAspWeb.Models;
using MyAspWeb.Repositories;
using MyAspWeb.ViewModels;

namespace MyAspWeb.Controllers
{
    [Route("[controller]/[action]")]
    public class NoteController : Controller
    {
        private INoteRepository Repository;
        private INoteTypeRepository TypeRepository;

        public NoteController(INoteRepository repository,INoteTypeRepository typeRepository)
        {
            Repository = repository;
            TypeRepository = typeRepository;
        }

        //[Route("")]
        //[Route("/")]
        //public async Task<IActionResult> Index()
        //{
        //    var notes = await Repository.ListAsync();
        //    //var noteViews = new List<NoteModel>();
        //    //notes.ForEach(n => noteViews.Add(new NoteModel()
        //    //{
        //    //    Id = n.Id,
        //    //    Title = n.Title,
        //    //    Content = n.Content,
        //    //})); ;
        //    return View(notes);
        //}

        [Route("")]
        [Route("/")]
        public async Task<IActionResult> Index(int pageindex=1)
        {
            var pagesize = 2;
            var notes = Repository.PageList(pageindex, pagesize);
            ViewBag.PageCount = notes.Item2;
            ViewBag.PageIndex = pageindex;
            return View(notes.Item1);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromServices] IWebHostEnvironment env,NoteModel note)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var fn = string.Empty;
            if (note.Attachment != null)
            {
                var dir = Path.Combine(env.WebRootPath, "file");
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
                fn = Path.Combine(dir, Path.GetFileNameWithoutExtension(note.Attachment.FileName) + "_" + Guid.NewGuid().ToString() + Path.GetExtension(note.Attachment.FileName));
                using (var stream = new FileStream(fn, FileMode.CreateNew))
                {
                    note.Attachment.CopyTo(stream);
                }
            }
            var newNote = new Note()
            {
                Title = note.Title,
                Content = note.Content,
                CreateTime = DateTime.Now,
                Password = note.Password,
                Attachment = fn,
            };
            if (note.Type != null)
            {
                newNote.Type = note.Type;
                newNote.TypeId = note.Type.Id;
            }
            await Repository.AddAsync(newNote);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Add()
        {
            var types = await TypeRepository.ListAsync();
            var items = new List<SelectListItem>();
            types.ForEach(r => items.Add(new SelectListItem()
            {
                Text = r.Name,
                Value = r.Id.ToString(),
            }));
            ViewBag.Types = items;
            return View("Add");
        }

        public async Task<IActionResult> Detail(int id)
        {
            var note = await Repository.GetByIdAsync(id);
            if (!string.IsNullOrEmpty(note.Password))
            {
                return View();
            }
            return View(note);
        }

        public async Task<IActionResult> Detail(int id,string psw)
        {
            var note = await Repository.GetByIdAsync(id);
            if (!note.Password.Equals(psw))
            {
                return BadRequest("密码错误");
            }
            return View(note);
        }

    }
}