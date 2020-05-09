using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MyAspWeb.Models
{
    public class Person
    {
        [Display(Name = "编号")]
        public string Id { get; set; }
        [Display(Name ="姓名")]
        public string Name { get; set; }
        [Display(Name ="年龄")]
        public int Age { get; set; }
        [Display(Name ="住址")]
        public string Address { get; set; }

        [Display(Name ="邮箱")]
        [Remote(action:"VerifyEmail",controller:"Person")]
        public string Email { get; set; }
    }
}
