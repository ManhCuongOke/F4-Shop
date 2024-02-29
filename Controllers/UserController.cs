using System.Reflection.Metadata;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Project.Models;

public class UserController : Controller {
    private readonly DatabaseContext _context;
    private readonly IHttpContextAccessor _accessor;
    public UserController(DatabaseContext context, IHttpContextAccessor accessor)
    {
        _context = context;
        _accessor = accessor;
    }

    [HttpPost]
    public IActionResult Login(string email = "", string password = "") {
        SqlParameter emailParam = new SqlParameter("@sEmail", email);
        SqlParameter passwordParam = new SqlParameter("@sPassword", password);
        List<User> user = _context.Users.FromSqlRaw("EXEC sp_LoginEmailAndPassword @sEmail, @sPassword", emailParam, passwordParam).ToList();
        var nameUser = user[0].sName;
        _accessor?.HttpContext?.Session.SetString("UserName", nameUser);
        _accessor?.HttpContext?.Session.SetInt32("UserID", user[0].PK_iUserID);

        // Lấy số lượng giỏ hàng
        var userID = _accessor?.HttpContext?.Session.GetInt32("UserID");
        SqlParameter userIDParam = new SqlParameter("@PK_iUserID", userID);
        IEnumerable<Cart> carts = _context.Carts.FromSqlRaw("sp_GetInfoCart @PK_iUserID", userIDParam);
        int cartCount = carts.Count();
        _accessor?.HttpContext?.Session.SetInt32("CartCount", cartCount);

        // return Json(user);
        return RedirectToAction("Index", "Home");
    }

    public IActionResult Register() {
        return View();
    }

    [HttpPost]
    public IActionResult Register(User user) {
        // Phải đặt enctype="multipart/form-data" thì IFromFile mới có giá trị
        // var name = user.sName;
        SqlParameter roleIdParam = new SqlParameter("@FK_iRoleID", 1);
        SqlParameter nameParam = new SqlParameter("@sName", user.sName);
        SqlParameter emailParam = new SqlParameter("@sEmail", user.sEmail);
        SqlParameter passwordParam = new SqlParameter("@sPassword", user.sPassword);
        // _context.Users.FromSqlRaw("EXEC sp_InsertUser @FK_iRoleID, @sName, @sEmail, @sPassword", 1, user.sName, user.sEmail, user.sPassword);
        _context.Database.ExecuteSqlRaw("EXEC sp_InsertUser @FK_iRoleID, @sName, @sEmail, @sPassword", roleIdParam, nameParam, emailParam, passwordParam);
        // _context.Database.ExecuteSqlRaw("INSERT INTO tbl_Users (FK_iRoleID, sName, sEmail, sPassword) VALUES (1, N'{0}', '{1}', '{2}')", 1, user.sName, user.sEmail, user.sPassword);
        ViewData["msg"] = "Tạo tài khoản thành công!";
        return RedirectToAction("Register");
    }
}