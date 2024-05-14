using _4._01_Hw.Data;
using _4._01_Hw.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace _4._01_Hw.Web.Controllers
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
            var repo = new QuestionRepository(_connectionString);
          
            return View(new QuestionViewModel
            {
                Questions = repo.GetQuestions()
            }); ;
        }
        [Authorize]
        public IActionResult AskAQuestion()
        {
            return View();

        }
      
        [HttpPost]
        public IActionResult AddQuestion(Question question, List<string> tags)
        {
            var repo = new QuestionRepository(_connectionString);
            var user = new UserRepository(_connectionString);
            question.UserId= user.GetByEmail(User.Identity.Name).Id;
            question.Date = DateTime.Today;
            repo.AddQuestion(question, tags);          
            return Redirect($"/home/viewquestion?id={question.Id}");
        }
        public IActionResult ViewQuestion(int id)
        {
            var repo = new QuestionRepository(_connectionString);
            return View(new QuestionViewModel
            {
                Question = repo.GetQuestion(id)
            });
        }
        [HttpPost]
        public IActionResult AddAnswer(Answer answer)
        {
            var repo = new QuestionRepository(_connectionString);
            var user = new UserRepository(_connectionString);
            answer.UserId = user.GetByEmail(User.Identity.Name).Id;
            answer.Date = DateTime.Today;
            repo.AddAnswer(answer);
            return Redirect($"/home/viewquestion?id={answer.QuestionId}");
        }



    }
}