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
        private readonly IHomeResponsitory _homeResponsitory;
        private readonly ICartReponsitoty _cartResponsitory;

        public HomeController(ILogger<HomeController> logger, DatabaseContext context, IHttpContextAccessor accessor, IHomeResponsitory homeResponsitory, ICartReponsitoty cartReponsitory)
        {
            _logger = logger;
            _context = context;
            _accessor = accessor;
            _homeResponsitory = homeResponsitory;
            _cartResponsitory = cartReponsitory;
        }

        public IActionResult Index(int currentPage = 1)
        {
            // Fix cứng dữ liệu
            // _accessor?.HttpContext?.Session.SetString("UserName", "Công Đặng");
            //_accessor?.HttpContext?.Session.SetInt32("UserID", 1);
            var userID = _accessor?.HttpContext?.Session.GetInt32("UserID");

            IEnumerable<Product> products = _homeResponsitory.getProducts().ToList();
            int totalRecord = products.Count();
            int pageSize = 6;
            int totalPage = (int) Math.Ceiling(totalRecord / (double) pageSize);
            products = products.Skip((currentPage - 1) * pageSize).Take(pageSize);
            IEnumerable<Category> categories = _homeResponsitory.getCategories().ToList();
            IEnumerable<CartDetail> cartDetails = _cartResponsitory.getCartInfo(Convert.ToInt32(userID)).ToList();
            ProductViewModel model = new ProductViewModel {
                Products = products,
                Categories = categories,
                CartDetails = cartDetails,
                TotalPage = totalPage,
                PageSize = pageSize,
                CurrentPage = currentPage
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult Search(string keyword = "") {
            IEnumerable<Category> products = _homeResponsitory.searchProductsByKeyword(keyword).ToList();
            return Ok(products);
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