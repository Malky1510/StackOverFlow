using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using StackOverflow.Data;
using StackOverflow.Web.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace StackOverflow.Web.Controllers
{
    public class HomeController : Controller
    { 

        private string _connectionString;
        public HomeController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ConStr");
        }

        public IActionResult Index()
        {
            Repo repo = new Repo(_connectionString);
            List<Question> questions = repo.GetQuestions();
            return View(questions);
        }
    }
}
