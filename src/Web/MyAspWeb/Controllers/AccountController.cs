using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyAspWeb.Migrations.Note;
using MyAspWeb.ViewModels;

namespace MyAspWeb.Controllers
{
    public class AccountController:Controller
    {
        private readonly ILogger<AccountController> logger;

        public AccountController(ILogger<AccountController> logger, UserManager<NoteUser> userManager, SignInManager<NoteUser> signInManager)
        {
            this.logger = logger;
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public UserManager<NoteUser> UserManager { get; }

        public SignInManager<NoteUser> SignInManager { get; }

        public IActionResult Login(string returnurl = null)
        {
            ViewBag.ReturnUrl = returnurl;
            return View();
        }

        public async Task<IActionResult> Login(LoginViewModel model,string returnurl= null)
        {
            if (!ModelState.IsValid)
                return View(model);
            return View();
        }

    }
}
