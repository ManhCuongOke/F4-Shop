using Microsoft.AspNetCore.Mvc;
using Project.Models;

[Route("/product")]
public class ProductController : Controller {
    private readonly DatabaseContext _context;
    private readonly IProductResponsitory _productResponsitory;
    private readonly IHttpContextAccessor _accessor;
    private readonly ICartReponsitory _cartResponsitory;
    private readonly IHomeResponsitory _homeresponsitory;
    private readonly IUserResponsitory _userResponsitory;
    public ProductController(DatabaseContext context, IProductResponsitory productResponsitory, ICartReponsitory cartReponsitoty, IHttpContextAccessor accessor, IHomeResponsitory homeResponsitory, IUserResponsitory userResponsitory)
    {
        _context = context;
        _productResponsitory = productResponsitory;
        _cartResponsitory = cartReponsitoty;
        _accessor = accessor;
        _homeresponsitory = homeResponsitory;
        _userResponsitory = userResponsitory;
    }

    [Route("index/{categoryID}")]
    public IActionResult Index(int categoryID) {
        IEnumerable<Product> products;
        var userID = _accessor?.HttpContext?.Session.GetInt32("UserID");
        List<User> users = _userResponsitory.checkUserLogin(Convert.ToInt32(userID)).ToList();
        if (users[0].FK_iRoleID == 2) {
            products = _productResponsitory.getProductsByCategoryIDIfRoleAdmin(categoryID).ToList();
        } else {
            products = _productResponsitory.getProductsByCategoryID(categoryID).ToList();
        }
        IEnumerable<CartDetail> cartDetails = _cartResponsitory.getCartInfo(Convert.ToInt32(userID)).ToList();
        IEnumerable<Category> categories = _homeresponsitory.getCategories().ToList();
        // Vì mình lấy layout của _Layout của kiểu là @model ProducdViewModel nó sẽ chung cho tất cả các trang, ta lấy riêng nó sẽ lỗi
        ProductViewModel model = new ProductViewModel {
            Products = products,
            Categories = categories,
            CartDetails = cartDetails,
            CurrentCategoryID = categoryID
        };
        return View(model);
    }

    /// <summary>
    /// Nguồn: https://xuanthulab.net/asp-net-core-mvc-chi-tiet-ve-route-trong-asp-net-mvc.html
    /// </summary>
    [Route("detail/{id?}")]
    public IActionResult Detail(int id)
    {
        var userID = _accessor?.HttpContext?.Session.GetInt32("UserID");
        IEnumerable<Product> product = _productResponsitory.getProductByID(id);
        IEnumerable<CartDetail> cartDetails = _cartResponsitory.getCartInfo(Convert.ToInt32(userID)).ToList();
        ProductViewModel model = new ProductViewModel {
            Products = product,
            CartDetails = cartDetails
        };
        return View(model);
    }

    [Route("sort/{categoryID?}/{sortType?}")]
    public IActionResult Sort(int categoryID, string sortType = "") {
        IEnumerable<Product> products;
        var userID = _accessor?.HttpContext?.Session.GetInt32("UserID");
        if (sortType == "asc") {
            products = _productResponsitory.getProductsByCategoryIDAndSortIncre(categoryID); // Gọi đúng phương thức sắp xếp tăng dần nhé
        } else {
            products = _productResponsitory.getProductsByCategoryIDAndSortReduce(categoryID); // Gọi đúng phương thức sắp xếp giảm dần nhé
        }
        IEnumerable<CartDetail> cartDetails = _cartResponsitory.getCartInfo(Convert.ToInt32(userID));
        IEnumerable<Category> categories = _homeresponsitory.getCategories();
        ProductViewModel model = new ProductViewModel {
            Products = products,
            CartDetails = cartDetails,
            Categories = categories,
            CurrentCategoryID = categoryID
        };
        //return Json(model);
        return View("Index", model);
    }

    [Route("/Home/CartDetail")]
    public IActionResult CartDetail()
    {
        return View();
    }
}