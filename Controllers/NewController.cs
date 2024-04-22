using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.Models;

public class NewController : Controller
{
    private readonly DatabaseContext _context;
    public NewController(DatabaseContext context)
    {
        _context = context;
    }
    public IActionResult Index() {
        var categories = _context.Categories.FromSqlRaw("SELECT PK_iCategoryID, sCategoryName, sCategoryImage, sCategoryDescription FROM tbl_Categories");
        return View(categories);
    }
}