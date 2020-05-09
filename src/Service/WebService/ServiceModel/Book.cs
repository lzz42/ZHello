using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebService.ServiceModel
{
    public class BookResponse
    {
        public string Name { get; set; }
        public string Author { get; set; }
        public float Price { get; set; }
        public int Time { get; set; }
    }

    [Route("/Book/{Name}/{Author}/{Price}")]
    public class Book:IReturn<BookResponse>
    {
        public string Name { get; set; }
        public string Author { get; set; }
        public float Price { get; set; }
    }
}