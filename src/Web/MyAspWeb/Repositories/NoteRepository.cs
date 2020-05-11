using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyAspWeb.Contexts;
using MyAspWeb.Models;

namespace MyAspWeb.Repositories
{
    public class NoteRepository : INoteRepository
    {
        private NoteContext Ctx;

        public NoteRepository(NoteContext ctx)
        {
            Ctx = ctx;
        }

        public Task AddAsync(Note note)
        {
            Ctx.Notes.Add(note);
            return Ctx.SaveChangesAsync();
        }

        public Task<Note> GetByIdAsync(int id)
        {
            return Task.FromResult(Ctx.Notes.FirstOrDefault(r => r.Id == id));
        }

        public Task<List<Note>> ListAsync()
        {
            return Task.FromResult(Ctx.Notes.Include(type=>type.Type).ToList());
        }

        public Tuple<List<Note>,int> PageList(int pageindex, int pagesize)
        {
            var notess = Ctx.Notes;
            if (notess.Count() == 0)
            {
                for (int i = 0; i < 20; i++)
                {
                    var type = new NoteType()
                    {
                        Name = "TypeName__" + (i % 5).ToString()
                    };
                    Ctx.NoteTypes.Add(type);
                    var node = new Note()
                    {
                        Title = "test00" + i.ToString(),
                        Content = "content00" + i.ToString(),
                        CreateTime = DateTime.Now.AddMinutes(-i),
                        TypeId = type.Id,
                        Type = type
                    };
                    Ctx.Notes.Add(node);
                }
            }
            Ctx.SaveChanges();
            var query = Ctx.Notes.Include(type => type.Type).AsQueryable();
            var count = query.Count();
            var pageCount = count % pagesize == 0 ? count / pagesize : count / pagesize + 1;
            var notes = query.OrderByDescending(r => r.CreateTime)
                .Skip((pageindex - 1) * pagesize)
                .Take(pagesize)
                .ToList();
            return Tuple.Create(notes, count);
        }

        public Task UpdateAsync(Note note)
        {
            Ctx.Entry(note).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            return Ctx.SaveChangesAsync();
        }
    }
}