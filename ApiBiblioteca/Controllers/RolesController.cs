using Microsoft.AspNetCore.Mvc;

namespace ApiBiblioteca.Controllers
{
    public class RolesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
