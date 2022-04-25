using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using StackOverflow.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace StackOverflow.Web.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        private string _connectionString;
        public AccountController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ConStr");
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            Repo repo = new Repo(_connectionString);
            var user = repo.Login(email, password);
            if (user == null)
            {
                return RedirectToAction("Login");
            }
            else
            {
                var claims = new List<Claim>
            {

                new Claim("user", email)
            };

                HttpContext.SignInAsync(new ClaimsPrincipal(
                    new ClaimsIdentity(claims, "Cookies", "user", "role"))).Wait();
                return Redirect("/");
            }

        }

        public IActionResult Logout()
        {
            HttpContext.SignOutAsync().Wait();
            return Redirect("/");
        }

        public IActionResult Signup()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Signup(User user, string password)
        {
            Repo repo = new Repo (_connectionString);
            repo.Signup(user, password);

            return Redirect("/");
        }

    }
}

