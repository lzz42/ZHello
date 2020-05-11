using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public Task<List<NoteType>> ListAsync()
        {
            return Task.FromResult(Ctx.NoteTypes.ToList());
        }
    }
}
