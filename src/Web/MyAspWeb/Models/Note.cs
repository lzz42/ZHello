using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyAspWeb.Models
{
    public class Note
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(150)]
        public string Title { get; set; }
        [Required]
        public string Content { get; set; }

        public DateTime CreateTime { get; set; }
        public int TypeId { get; set; }
        public NoteType Type { get; set; }
    }
}
