using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebService.ServiceModel;

namespace WebService.ServiceInterface
{
    public class BookServices:Service
    {
        public BookResponse Any(Book book)
        {
            return new BookResponse() { Name = book.Name, Author = book.Author, Price = book.Price, Time = Environment.TickCount };
        }
    }
}