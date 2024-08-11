using BlogApplicationSQL.Models;
using Database;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BlogApplicationSQL.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly Database.Database _database;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            _database = new Database.Database("Server=LAPTOP-LMK46U7C;Database=BlogsDB;Trusted_Connection=True;TrustServerCertificate=True");
        }

        public IActionResult Index()
        {
            List<Blog> blogs = _database.GetAllBlogs();
            return View(blogs);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Blog blog)
        {
            _database.CreateBlog(blog.Title, blog.Summary, blog.BlogText);

            return Redirect("Index");
        }

        public IActionResult Details(int id)
        {
            Blog blog = _database.GetBlogById(id);

            return View(blog);
        }

        public IActionResult Delete(int id)
        {
            Blog blog = _database.GetBlogToDelete(id);
            return View(blog);
        }

        [HttpPost]
        public IActionResult DeleteBlogById(int id)
        {
            _database.DeleteBlog(id);
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
