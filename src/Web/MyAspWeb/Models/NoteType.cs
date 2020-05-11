using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyAspWeb.Models
{
    public class NoteType
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        public List<Note> Notes { get; set; }
    }
}