using Microsoft.AspNetCore.Mvc;

namespace SharkFit.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}