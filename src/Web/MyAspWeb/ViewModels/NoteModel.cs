using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using MyAspWeb.Models;

namespace MyAspWeb.ViewModels
{
    public class NoteModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreateTime { get; set; }
        public int TypeId { get; set; }
        public NoteType Type { get; set; }
        public string Password { get; set; }
        public IFormFile Attachment { get; set; }
    }
}
