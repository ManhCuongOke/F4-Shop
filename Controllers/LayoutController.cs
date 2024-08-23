using Microsoft.AspNetCore.Mvc;

namespace Project.Controllers
{
    public class LayoutController : Controller
    {
        public IActionResult Layout()
        {
            return View();
        }
    }
}
