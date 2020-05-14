using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyAspWeb.Contexts;
using MyAspWeb.Models;

namespace MyAspWeb.Repositories
{
    public class NoteTypeRepository : INoteTypeRepository
    {
        private NoteContext Ctx;

        public NoteTypeRepository(NoteContext ctx)
        {
            Ctx = ctx;
        }

        public Task AddAsync(NoteType notetype)
        {
            if (notetype != null)
            {
                var n = Ctx.NoteTypes.ToList().Find(t => t.Id == notetype.Id);
                if (n != null)
                {
                    n.Name = notetype.Name;
                    Ctx.Entry(n).State = EntityState.Modified;
                }
                else
                {
                    Ctx.NoteTypes.Add(notetype);
                    Ctx.Entry(notetype).State = EntityState.Added;
                }
                return Ctx.SaveChangesAsync();
            }
            return null;
        }

        public void DeleteAll()
        {
            Ctx.Notes.RemoveRange(Ctx.Notes.ToArray());
            Ctx.SaveChanges();
        }

        public Task<List<NoteType>> ListAsync()
        {
           return  Ctx.NoteTypes.ToListAsync();
        }
    }
}
