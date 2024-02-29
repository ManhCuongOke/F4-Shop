using Microsoft.AspNetCore.Mvc;
using Project.Models;

public class ProductController : Controller {
    private readonly DatabaseContext _context;
    public ProductController(DatabaseContext context)
    {
        _context = context;
    }
    public IActionResult Index() {
        return View();
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