using Microsoft.AspNetCore.Mvc;

namespace DemoMVC.Controllers
{
    public class HelloWorldController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public string Welcome()
        {
            return "This is the Welcome action method in the HelloWorld controller.";
        }
    }
}