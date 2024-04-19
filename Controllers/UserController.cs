using System.Reflection.Metadata;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Project.Models;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;


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
        string password = "12345678";
        string encrypted = _userResponsitory.encrypt(password);
        string decryted = _userResponsitory.decrypt(encrypted);
        System.Console.WriteLine("Mat khau ma hoa: " + encrypted);
        System.Console.WriteLine("Mat khau giai ma: " + decryted);
        return View();
    }

    [HttpPost]
    public IActionResult Login(LoginModel user) {
        if (!ModelState.IsValid) {
            return View(user);
        }
        List<User> userLogin = _userResponsitory.login(user.sEmail, user.sPassword).ToList();
        string nameUser = userLogin[0].sName;
        int value = userLogin[0].PK_iUserID;
        // Tạo Cookies
        CookieOptions options = new CookieOptions {
            Expires = DateTime.Now.AddDays(1)
        };
        Response.Cookies.Append("UserID", value.ToString(), options);
        _accessor?.HttpContext?.Session.SetString("UserName", nameUser);
        //_accessor?.HttpContext?.Session.SetInt32("UserID", userLogin[0].PK_iUserID);

        // Lấy số lượng giỏ hàng
        // var userID = _accessor?.HttpContext?.Session.GetInt32("UserID");
        // var userID = Request.Cookies["UserID"];
        // IEnumerable<CartDetail> carts = _cartResponsitory.getCartInfo(Convert.ToInt32(userID));
        // int cartCount = carts.Count();
        // _accessor?.HttpContext?.Session.SetInt32("CartCount", cartCount);

        // return Json(user);
        return RedirectToAction("Index", "Home");
    }

    [Route("/user/forgot")]
    public IActionResult Forgot() {
        return View();
    }

    [Route("/user/forgot")]
    [HttpPost]
    public IActionResult Forgot(string email) {
        TempData["result"] = "Mật khẩu của bạn là: 12345678";
        return RedirectToAction("Forgot");
    }

    public IActionResult Profile() {
        var userID = _accessor?.HttpContext?.Session.GetInt32("UserID");
        IEnumerable<CartDetail> cartDetails = _cartResponsitory.getCartInfo(Convert.ToInt32(userID)).ToList();
        ProductViewModel model = new ProductViewModel
        {
            CartDetails = cartDetails,
            UserID = Convert.ToInt32(userID)
        };
        return View(model);
    }

    public IActionResult Logout() {
        _accessor?.HttpContext?.Session.SetString("Username", "");
        _accessor?.HttpContext?.Session.SetInt32("UserID", 0);
        return RedirectToAction("Index", "Home");
    }

    public IActionResult Register() {
        return View();
    }

    /// <summary>
    /// Tương tự ViewData và ViewBag, TempData cũng dùng để truyền dữ liệu ra view. 
    /// Tuy nhiên sẽ hơi khác một chút, đó là TempData sẽ tồn tại cho đến khi nó được đọc. 
    /// Tức là ViewBag và ViewData chỉ hiển thị được dữ liệu ngay tại trang người dùng truy cập, 
    /// còn TempData có thể lưu lại và hiển thị ở một trang sau đó và nó chỉ biến mất khi người dùng đã "đọc" nó.
    /// Nguồn: https://techmaster.vn/posts/34556/cach-su-dung-tempdata-trong-aspnet-core-mvc
    /// </summary>

    [HttpPost]
    public IActionResult Register(RegistrastionModel user) {
        System.Console.WriteLine("Password Confirm: " + user.sPasswordConfirm);
        if (!ModelState.IsValid) {
            return View(user);
        }
        _userResponsitory.register(user);
        TempData["msg"] = "Đăng ký tài khoản thành công!";
        return RedirectToAction("Register");
    }
}