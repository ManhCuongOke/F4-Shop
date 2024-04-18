using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Project.Models;

public class OrderController : Controller {
    private readonly DatabaseContext _context;
    private readonly IHttpContextAccessor _accessor;
    public OrderController(DatabaseContext context, IHttpContextAccessor accessor)
    {
        _context = context;
        _accessor = accessor;
    }
    public IActionResult Index() {
        return View();
    }

    [HttpPost]
    public IActionResult Checkout() {
        // Fix cứng cũng phải khai báo SqlParameter
        var sessionUserID = _accessor?.HttpContext?.Session.GetInt32("UserID");
        SqlParameter userIDParam = new SqlParameter("@PK_iUserID", Convert.ToInt32(sessionUserID));
        var totalMoney = _context.Orders.FromSqlRaw("sp_TotalMoneyProductInCart @PK_iUserID", userIDParam);
        return Json(totalMoney); 
    }
}