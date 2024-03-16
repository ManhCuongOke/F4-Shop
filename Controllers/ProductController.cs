using Microsoft.AspNetCore.Mvc;
using Project.Models;

public class ProductController : Controller {
    private readonly DatabaseContext _context;
    private readonly IProductResponsitory _productResponsitory;
    private readonly IHttpContextAccessor _accessor;
    private readonly ICartReponsitoty _cartResponsitory;
    public ProductController(DatabaseContext context, IProductResponsitory productResponsitory, ICartReponsitoty cartReponsitoty, IHttpContextAccessor accessor)
    {
        _context = context;
        _productResponsitory = productResponsitory;
        _cartResponsitory = cartReponsitoty;
        _accessor = accessor;
    }
    public IActionResult Index(int categoryID) {
        var userID = _accessor?.HttpContext?.Session.GetInt32("UserID");
        IEnumerable<Product> products = _productResponsitory.getProductsByCategoryID(categoryID).ToList();
        IEnumerable<CartDetail> cartDetails = _cartResponsitory.getCartInfo(Convert.ToInt32(userID)).ToList();
        // Vì mình lấy layout của _Layout của kiểu là @model ProducdViewModel nó sẽ chung cho tất cả các trang, ta lấy riêng nó sẽ lỗi
        ProductViewModel model = new ProductViewModel {
            Products = products,
            CartDetails = cartDetails
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

    [Route("/Home/CartDetail")]
    public IActionResult CartDetail()
    {
        return View();
    }
}