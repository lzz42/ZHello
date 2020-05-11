using Microsoft.EntityFrameworkCore;
using MyAspWeb.Models;

namespace MyAspWeb.Contexts
{
    public class NoteContext : DbContext
    {
        public DbSet<Note> Notes { get; set; }
        public DbSet<NoteType> NoteTypes { get; set; }

        public NoteContext(DbContextOptions<NoteContext> options) : base(options)
        {
        }
    }
}