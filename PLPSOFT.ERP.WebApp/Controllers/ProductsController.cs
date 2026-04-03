using Microsoft.AspNetCore.Mvc;

namespace PLPSOFT.ERP.WebApp.Controllers
{
    public class ProductsController : Controller
    {
        public IActionResult Index()
        {
            return View(); // Trả về giao diện Index.cshtml
        }
    }
}