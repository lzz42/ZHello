using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Moq;
using MyAspWeb.Contexts;
using Microsoft.Extensions.Options;
using MyAspWeb.Models;
using MyAspWeb.Controllers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory.Storage.Internal;
using Microsoft.AspNetCore.Mvc;

namespace MyWebxUnitTest.Controller
{
    public class BlogControllerTest
    {
        [Fact]
        public async Task BlogIndex()
        {
            var mockCtx = new Mock<BloggingDbContext>();
            var mockOpt = new Mock<IOptions<GameSetting>>();

            var controller = new BlogController(mockCtx.Object, mockOpt.Object);

            var result = controller.Index();

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Blog>>(viewResult.ViewData.Model);
            //Assert.True(3, model.Count());  
        }
    }
}
