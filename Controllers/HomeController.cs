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
        private readonly ICartReponsitory _cartResponsitory;
        private readonly IUserResponsitory _userResponsitory;

        public HomeController(ILogger<HomeController> logger, DatabaseContext context, IHttpContextAccessor accessor, IHomeResponsitory homeResponsitory, ICartReponsitory cartReponsitory, IUserResponsitory userResponsitory)
        {
            _logger = logger;
            _context = context;
            _accessor = accessor;
            _homeResponsitory = homeResponsitory;
            _cartResponsitory = cartReponsitory;
            _userResponsitory = userResponsitory;
        }

        public IActionResult Index(int currentPage = 1)
        {
            // Lấy Cookies trên trình duyệt
            var userID = Request.Cookies["UserID"];
            if (userID != null) {
                _accessor?.HttpContext?.Session.SetInt32("UserID", Convert.ToInt32(userID));
            }
            IEnumerable<Product> products = _homeResponsitory.getProducts().ToList();
            int totalRecord = products.Count();
            int pageSize = 12;
            int totalPage = (int) Math.Ceiling(totalRecord / (double) pageSize);
            products = products.Skip((currentPage - 1) * pageSize).Take(pageSize);
            IEnumerable<Category> categories = _homeResponsitory.getCategories().ToList();
            IEnumerable<CartDetail> cartDetails = _cartResponsitory.getCartInfo(Convert.ToInt32(userID)).ToList();
            IEnumerable<CartDetail> carts = _cartResponsitory.getCartInfo(Convert.ToInt32(userID));
            if (userID != null) {
                List<User> users = _userResponsitory.checkUserLogin(Convert.ToInt32(userID)).ToList();
                _accessor?.HttpContext?.Session.SetString("UserName", users[0].sName);
            } else {
                _accessor?.HttpContext?.Session.SetString("UserName", "");
            }
            int cartCount = carts.Count();
            ProductViewModel model = new ProductViewModel {
                Products = products,
                Categories = categories,
                CartDetails = cartDetails,
                TotalPage = totalPage,
                PageSize = pageSize,
                CurrentPage = currentPage,
                UserID = Convert.ToInt32(userID),
                CartCount = cartCount
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult GetData(int currentPage = 1) {
            // Lấy giá trị Cookies đã lưu trên trình duyệt
            var userID = Request.Cookies["UserID"];
            IEnumerable<Product> products = _homeResponsitory.getProducts().ToList();
            int totalRecord = products.Count();
            int pageSize = 12;
            int totalPage = (int) Math.Ceiling(totalRecord / (double) pageSize);
            products = products.Skip((currentPage - 1) * pageSize).Take(pageSize);
            IEnumerable<Category> categories = _homeResponsitory.getCategories().ToList();
            IEnumerable<CartDetail> cartDetails = _cartResponsitory.getCartInfo(Convert.ToInt32(userID)).ToList();
            IEnumerable<CartDetail> carts = _cartResponsitory.getCartInfo(Convert.ToInt32(userID));
            int cartCount = carts.Count();
            ProductViewModel model = new ProductViewModel {
                Products = products,
                Categories = categories,
                CartDetails = cartDetails,
                TotalPage = totalPage,
                PageSize = pageSize,
                CurrentPage = currentPage,
                UserID = Convert.ToInt32(userID),
                CartCount = cartCount
            };
            return Ok(model);
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