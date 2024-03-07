using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.Models;
using System.Diagnostics;
using RouteAtrribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace Project.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DatabaseContext _context;
        private readonly IHttpContextAccessor _accessor;

        public HomeController(ILogger<HomeController> logger, DatabaseContext context, IHttpContextAccessor accessor)
        {
            _logger = logger;
            _context = context;
            _accessor = accessor;
        }

        public IActionResult Index(int currentPage = 1, int categoryId = 1)
        {
            // Fix cứng dữ liệu
            _accessor?.HttpContext?.Session.SetString("UserName", "Công Đặng");
            _accessor?.HttpContext?.Session.SetInt32("UserID", 1);

            IEnumerable<Product> products = _context.DisplayProducts(categoryId).ToList();
            int totalRecord = products.Count();
            int pageSize = 3;
            int totalPage = (int) Math.Ceiling(totalRecord / (double) pageSize);
            products = products.Skip((currentPage - 1) * pageSize).Take(pageSize);
            IEnumerable<Category> categories = _context.DisplayCategories().ToList();
            ProductViewModel model = new ProductViewModel {
                Products = products,
                Categories = categories,
                TotalPage = totalPage,
                PageSize = pageSize,
                CurrentPage = currentPage
            };
            return View(model);
        }

        public IActionResult Privacy()
        {
            _accessor?.HttpContext?.Session.SetString("StudentName", "Công");
            _accessor?.HttpContext?.Session.SetInt32("StudentID", 1);
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}