using _4._01_Hw.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace _4._01_Hw.Web.Controllers
{
    public class AccountController : Controller
    {

        private readonly string _connectionString;

        public AccountController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ConStr");
        }

        public IActionResult Signup()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Signup(User user, string password)
        {
            var repo = new UserRepository(_connectionString);
            repo.AddUser(user, password);
            return Redirect("/account/login");
        }

        public IActionResult Login()
        {
            if (TempData["Message"] != null)
            {
                ViewBag.Message = (string)TempData["Message"];
            }
            return View();
        }

        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            var repo = new UserRepository(_connectionString);
            var user = repo.Login(email, password);
            if (user == null)
            {
                TempData["Message"] = "Invalid Login!";
                return RedirectToAction("Login");
            }


            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Email, email) //whatever we set here, will be what is available through User.Identity.Name
            };

            //This line of code does the actual sign in
            HttpContext.SignInAsync(new ClaimsPrincipal(
                    new ClaimsIdentity(claims, "Cookies", ClaimTypes.Email, "roles"))
                ).Wait();

            return Redirect("/home/index");
        }

        public IActionResult Logout()
        {
            HttpContext.SignOutAsync().Wait();
            return Redirect("/");
        }
    }

}
