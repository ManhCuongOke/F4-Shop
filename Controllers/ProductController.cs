using Microsoft.AspNetCore.Mvc;
using Project.Models;

public class ProductController : Controller {
    private readonly DatabaseContext _context;
    private readonly IProductResponsitory _productResponsitory;
    private readonly IHttpContextAccessor _accessor;
    private readonly ICartReponsitory _cartResponsitory;
    private readonly IHomeResponsitory _homeresponsitory;
    public ProductController(DatabaseContext context, IProductResponsitory productResponsitory, ICartReponsitory cartReponsitoty, IHttpContextAccessor accessor, IHomeResponsitory homeResponsitory)
    {
        _context = context;
        _productResponsitory = productResponsitory;
        _cartResponsitory = cartReponsitoty;
        _accessor = accessor;
        _homeresponsitory = homeResponsitory;
    }
    public IActionResult Index(int categoryID) {
        var userID = _accessor?.HttpContext?.Session.GetInt32("UserID");
        IEnumerable<Product> products = _productResponsitory.getProductsByCategoryID(categoryID).ToList();
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

    [Route("/Product/Detail/{id?}")]
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

    public IActionResult Sort(int categoryID, string sortType = "") {
        IEnumerable<Product> products;
        var userID = _accessor?.HttpContext?.Session.GetInt32("UserID");
        if (sortType == "asc") {
            products = _productResponsitory.getProductsByCategoryIDAndSortIncre(categoryID); // Gọi đúng phương thức sắp xếp nhé
        } else {
            products = _productResponsitory.getProductsByCategoryIDAndSortIncre(categoryID); // Gọi đúng phương thức sắp xếp nhé
        }
        IEnumerable<CartDetail> cartDetails = _cartResponsitory.getCartInfo(Convert.ToInt32(userID));
        IEnumerable<Category> categories = _homeresponsitory.getCategories();
        ProductViewModel model = new ProductViewModel {
            Products = products,
            CartDetails = cartDetails,
            Categories = categories,
            CurrentCategoryID = categoryID
        };
        return View("Index", model);
    }

    [Route("/Home/CartDetail")]
    public IActionResult CartDetail()
    {
        return View();
    }
}