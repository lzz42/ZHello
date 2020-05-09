using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyAspWeb.Models;

namespace MyAspWeb.Contexts
{
    public class PersonDbContext: DbContext
    {
        public DbSet<Person> Persons { get; set; }

        public PersonDbContext(DbContextOptions<PersonDbContext> options)
            : base(options)
        {
        }
    }
}
