using Forum_rufine_et_paul.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Forum_rufine_et_paul.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        

        public HomeController(ILogger<HomeController> logger, Forum23105Context forumContext)
        {
            _logger = logger;
            
        }

        public IActionResult Index()
        {
            return RedirectToAction("Index","Categories");


           // return View();
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