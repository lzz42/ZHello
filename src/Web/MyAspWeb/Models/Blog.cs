using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyAspWeb.Models
{
    public class Blog : IValidatableObject
    {
        [Required]
        [Range(0, int.MaxValue - 1)]
        public int BlogId { get; set; }

        [Required]
        [StringLength(60)]
        public string BlogName { get; set; }

        [Required]
        [StringLength(140)]
        [Url]
        public string Url { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime CreateTime { get; set; }

        public List<Post> Posts { get; set; }

        public Blog()
        {
            Posts = new List<Post>();
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            throw new NotImplementedException();
        }
    }
}