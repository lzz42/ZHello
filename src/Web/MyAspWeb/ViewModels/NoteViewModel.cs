using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace MyAspWeb.ViewModels
{
    public class NoteViewModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name ="标题")]
        public string Title { get; set; }

        [Required]
        [Display(Name ="内容")]
        public string Content { get; set; }

        [Display(Name = "类型")]
        public string Type { get; set; }

        [Display(Name = "密码")]
        public string Password { get; set; }

        [Display(Name = "附件")]
        public string Attachment { get; set; }
    }
}
