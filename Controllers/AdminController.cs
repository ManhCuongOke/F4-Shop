using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.Models;

public class AdminController : Controller {
    private readonly DatabaseContext _context;
    public AdminController(DatabaseContext context)
    {
        _context = context;
    }

    [Route("/admin")]
    public IActionResult Index() {
        return View();
    }

    [HttpPost]
    public IActionResult Index(Category category) {
        if (!ModelState.IsValid) {
            return View(category);
        }
        TempData["msg"] = "Thêm thể loại thành công!";
        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult GetData() {
        var categories = _context.Categories.FromSqlRaw("SELECT PK_iCategoryID, sCategoryName, sCategoryImage, sCategoryDescription FROM tbl_Categories");
        return Ok(categories);
    }
}