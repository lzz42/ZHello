using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public async Task<IActionResult> Add(NoteModel note)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await Repository.AddAsync(new Note()
            {
                Title = note.Title,
                Content = note.Content,
                CreateTime = DateTime.Now,
                TypeId = note.Type,
            });
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
    }
}