using Funq;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebService
{
    public class BookResponse
    {
        public string Name { get; set; }
        public string Author { get; set; }
        public float Price { get; set; }
        public int Time { get; set; }
    }

    [Route("/Book/{Name}/{Author}/{Price}")]
    public class Book : IReturn<BookResponse>
    {
        public string Name { get; set; }
        public string Author { get; set; }
        public float Price { get; set; }
    }

    public class BookServices : Service
    {
        public BookResponse Any(Book book)
        {
            return new BookResponse() { Name = book.Name, Author = book.Author, Price = book.Price, Time = Environment.TickCount };
        }
    }

    public class AppHost:AppHostBase
    {
        public AppHost() : base("WebService", typeof(ServiceModel.Book).Assembly)
        {

        }
        public override void Configure(Container container)
        {
            //throw new NotImplementedException();
        }
    }

    public class Global:System.Web.HttpApplication
    {
        protected void Application_Start(object sender,EventArgs e)
        {
            new AppHost().Init();
        }
    }
}