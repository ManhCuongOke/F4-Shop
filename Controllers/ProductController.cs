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
        return View(products);
    }

    [Route("/Product/Detail/{id?}")]
    public IActionResult Detail(int id)
    {
        var product = _context.DisplayProductByID(id).ToList();
        return View(product);
    }

    [Route("/Home/CartDetail")]
    public IActionResult CartDetail()
    {
        return View();
    }
}