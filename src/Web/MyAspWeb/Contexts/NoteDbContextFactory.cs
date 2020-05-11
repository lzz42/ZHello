using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace MyAspWeb.Contexts
{
    public class NoteDbContextFactory : IDesignTimeDbContextFactory<NoteContext>
    {
        public NoteContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<NoteContext>();
            builder.UseSqlite("Data Source=note.db");
            return new NoteContext(builder.Options);
        }
    }
}
