using Microsoft.AspNetCore.Mvc;
using Project.Models;

public class ProductController : Controller {
    private readonly DatabaseContext _context;
    private readonly IProductResponsitory _productResponsitory;
    public ProductController(DatabaseContext context, IProductResponsitory productResponsitory)
    {
        _context = context;
        _productResponsitory = productResponsitory;
    }
    public IActionResult Index(int categoryID) {
        IEnumerable<Product> products = _productResponsitory.getProductsByCategoryID(categoryID).ToList();
        // Vì mình lấy layout của _Layout của kiểu là @model ProducdViewModel nó sẽ chung cho tất cả các trang, ta lấy riêng nó sẽ lỗi
        ProductViewModel model = new ProductViewModel {
            Products = products
        };
        return View(model);
    }

    [Route("/Product/Detail/{id?}")]
    public IActionResult Detail(int id)
    {
        IEnumerable<Product> product = _context.DisplayProductByID(id).ToList();
        ProductViewModel model = new ProductViewModel {
            Products = product
        };
        return View(model);
    }

    [Route("/Home/CartDetail")]
    public IActionResult CartDetail()
    {
        return View();
    }
}