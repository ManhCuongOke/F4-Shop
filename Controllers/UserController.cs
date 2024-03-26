using System.Reflection.Metadata;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Project.Models;

public class UserController : Controller {
    private readonly DatabaseContext _context;
    private readonly IHttpContextAccessor _accessor;
    private readonly IUserResponsitory _userResponsitory;
    private readonly ICartReponsitory _cartResponsitory;
    public UserController(DatabaseContext context, IHttpContextAccessor accessor, IUserResponsitory userResponsitory, ICartReponsitory cartReponsitory)
    {
        _context = context;
        _accessor = accessor;
        _userResponsitory = userResponsitory;
        _cartResponsitory = cartReponsitory;
    }

    public IActionResult Login() {
        return View();
    }

    [HttpPost]
    public IActionResult Login(User user) {
        if (ModelState.IsValid) {

        }
        List<User> userLogin = _userResponsitory.login(user.sEmail, user.sPassword).ToList();
        string nameUser = userLogin[0].sName;
        _accessor?.HttpContext?.Session.SetString("UserName", nameUser);
        _accessor?.HttpContext?.Session.SetInt32("UserID", userLogin[0].PK_iUserID);

        // Lấy số lượng giỏ hàng
        var userID = _accessor?.HttpContext?.Session.GetInt32("UserID");
        // SqlParameter userIDParam = new SqlParameter("@PK_iUserID", userID);
        IEnumerable<CartDetail> carts = _cartResponsitory.getCartInfo(Convert.ToInt32(userID));
        int cartCount = carts.Count();
        _accessor?.HttpContext?.Session.SetInt32("CartCount", cartCount);

        // return Json(user);
        return RedirectToAction("Index", "Home");
    }

    public IActionResult Logout() {
        _accessor?.HttpContext?.Session.SetString("Username", "");
        _accessor?.HttpContext?.Session.SetInt32("UserID", 0);
        return RedirectToAction("Index", "Home");
    }

    public IActionResult Register() {
        return View();
    }

    [HttpPost]
    public IActionResult Register(User user) {
        // Phải đặt enctype="multipart/form-data" thì IFromFile mới có giá trị
        SqlParameter roleIdParam = new SqlParameter("@FK_iRoleID", 1);
        SqlParameter nameParam = new SqlParameter("@sName", user.sName);
        SqlParameter emailParam = new SqlParameter("@sEmail", user.sEmail);
        SqlParameter passwordParam = new SqlParameter("@sPassword", user.sPassword);
        _context.Database.ExecuteSqlRaw("EXEC sp_InsertUser @FK_iRoleID, @sName, @sEmail, @sPassword", roleIdParam, nameParam, emailParam, passwordParam);
        ViewData["msg"] = "Tạo tài khoản thành công!";
        return RedirectToAction("Register");
    }
}