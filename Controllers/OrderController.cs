using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Project.Models;

public class OrderController : Controller {
    private readonly DatabaseContext _context;
    public OrderController(DatabaseContext context)
    {
        _context = context;
    }
    public IActionResult Index() {
        return View();
    }

    [HttpPost]
    public IActionResult Checkout() {
        // Fix cứng cũng phải khai báo SqlParameter
        SqlParameter userIDParam = new SqlParameter("@PK_iUserID", 1);
        var totalMoney = _context.Orders.FromSqlRaw("sp_TotalMoneyProductInCart @PK_iUserID", userIDParam);
        return Json(totalMoney); 
    }
}