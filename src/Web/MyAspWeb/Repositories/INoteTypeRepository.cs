using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyAspWeb.Models;

namespace MyAspWeb.Repositories
{
    public interface INoteTypeRepository
    {
        Task<List<NoteType>> ListAsync();
        Task AddAsync(NoteType note);
        void DeleteAll();
    }
}
