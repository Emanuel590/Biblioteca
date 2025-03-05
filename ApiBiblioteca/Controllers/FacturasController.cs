using Microsoft.AspNetCore.Mvc;

namespace ApiBiblioteca.Controllers
{
    public class FacturasController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
