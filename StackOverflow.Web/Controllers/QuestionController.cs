using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using StackOverflow.Data;
using StackOverflow.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StackOverflow.Web.Controllers
{
    public class QuestionController : Controller
    {
        private string _connectionString;
        public QuestionController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ConStr");
        }
        [Authorize]
        public IActionResult AskAQuestion()
        {
            return View();
        }
        [Authorize]
        [HttpPost]
        public IActionResult AddQuestion(Question question, List<String> tags)
        {
            Repo repo = new Repo(_connectionString);
            int userId = GetCurrentUserId();
            if (userId == 0)
            {
                return Redirect("/");
            }
            question.UserId = userId;
            repo.AddQuestionTags(question, tags);


            return Redirect("/");
        }
        public IActionResult ViewQuestion(int id)
        {
         Repo repo = new Repo(_connectionString);

            var question = repo.GetQuestion(id);
            var vm = new ShowQuestionVM
            {
                Question = question,
                AlreadyLiked = question.Likes.Any(l => l.UserId == GetCurrentUserId())
            };
            return View(vm);
        }
        public void UpdateLikes(int id)
        {
            Repo repo = new Repo(_connectionString);
            int userId = GetCurrentUserId();
            if (userId == 0)
            {
                return;
            }

            repo.UpdateLikes(id, userId);
        }
        public JsonResult GetLikes(int id)
        {
            var repo = new Repo(_connectionString);
            int likes = repo.GetLikes(id);
            return Json(likes);
        }

        [HttpPost]
        [Authorize]
        public IActionResult AddAnswer(Answer answer)
        {
            var repo = new Repo(_connectionString);
            answer.DatePosted = DateTime.Now;
            int userId = GetCurrentUserId();
            if (userId == 0)
            {
                return Redirect("/");
            }
            answer.UserId = userId;
            repo.AddAnswer(answer);
            return Redirect($"/question/viewquestion?id={answer.QuestionId}");
        }
        private int GetCurrentUserId()
        {
         Repo repo = new Repo(_connectionString);
            if (!User.Identity.IsAuthenticated)
            {
                return 0;
            }
            var user = repo.GetByEmail(User.Identity.Name);
            if (user == null)
            {
                return 0;
            }

            return user.Id;
        }

    }
}
