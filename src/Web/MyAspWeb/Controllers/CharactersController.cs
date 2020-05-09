using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MyAspWeb.Controllers
{
    public class Character
    {
        public Character(string str)
        {
            Str = str;
        }

        public string Str { get; private set; }
    }
    public interface ICharacterRepository
    {
        IEnumerable<Character> ListAll();
        void Add(Character character);
    }

    public class CharacterRepository : ICharacterRepository
    {
        //private readonly ApplicationDbContext _dbContext;

        public void Add(Character character)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Character> ListAll()
        {
            throw new NotImplementedException();
        }
    }

    public class CharactersController:Controller
    {
        private readonly ICharacterRepository _repo;

        public CharactersController(ICharacterRepository repo)
        {
            _repo = repo;
        }
        public IActionResult Index()
        {
            PopulateCharactersIfNotExist();
            var data = _repo.ListAll();
            return View(data);
        }

        private void PopulateCharactersIfNotExist()
        {
            if (!_repo.ListAll().Any())
            {
                _repo.Add(new Character("Alice"));
                _repo.Add(new Character("Bro"));
                _repo.Add(new Character("Aax"));
                _repo.Add(new Character("Jav"));
            }
        }
    }
}
