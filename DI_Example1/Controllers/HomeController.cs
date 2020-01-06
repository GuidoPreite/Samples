using DI_Example1.Interfaces;
using DI_Example1.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace DI_Example1.Controllers
{
    public class HomeController : Controller
    {
        private ITransientRandomNumberService _transient;
        private IScopedRandomNumberService _scoped;
        private ISingletonRandomNumberService _singleton;

        public HomeController(ITransientRandomNumberService transient,
                              IScopedRandomNumberService scoped,
                              ISingletonRandomNumberService singleton)
        {
            _transient = transient;
            _scoped = scoped;
            _singleton = singleton;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Route("transient")]
        public IActionResult Transient()
        {
            return View(_transient.GetNumber());
        }
        [Route("scoped")]
        public IActionResult Scoped()
        {
            return View(_scoped.GetNumber());
        }
        [Route("singleton")]
        public IActionResult Singleton()
        {
            return View(_singleton.GetNumber());
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
