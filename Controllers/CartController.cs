using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Project.Models;

public class CartController : Controller {
    private readonly IHttpContextAccessor _accessor;
    private readonly DatabaseContext _context;
    public CartController(IHttpContextAccessor accessor, DatabaseContext context)
    {
        _accessor = accessor;
        _context = context;
    }

    public IActionResult Index() {
        // Fix cứng dữ liệu
        _accessor?.HttpContext?.Session.SetInt32("UserID", 1);

        var userID = _accessor?.HttpContext?.Session.GetInt32("UserID");
        SqlParameter userIDParam = new SqlParameter("@PK_iUserID", userID);
        IEnumerable<CartDetail> carts = _context.CartDetails.FromSqlRaw("sp_GetInfoCart @PK_iUserID", userIDParam);
        int cartCount = carts.Count();
        _accessor?.HttpContext?.Session.SetInt32("CartCount", cartCount);
        
        return View(carts);
    }

    [Route("/Cart/AddToCart/{productID?}")]
    [HttpGet("/Cart/AddToCart/{productID?}")]
    public IActionResult AddToCart(int productID)
    {
        var sessionUserID = _accessor?.HttpContext?.Session.GetInt32("UserID");
        SqlParameter userIDParam = new SqlParameter("@PK_iUserID", sessionUserID);
        var userID = _context.Users.FromSqlRaw("EXEC sp_CheckUserLogin @PK_iUserID", userIDParam);
        if (userID == null)
        {
            string msg = "Bạn phải đăng nhập mới được thêm vào giỏ hàng!";
            return Json(new { msg });
        }
        else
        {
            // Thêm mã giỏ hàng
            SqlParameter updateTimeParam = new SqlParameter("@dUpdateTime", DateTime.Now);
            _context.Database.ExecuteSqlRaw("sp_InsertCart @dUpdateTime", updateTimeParam);
            List<Cart> cart = _context.Carts.FromSqlRaw("").ToList();
            return Json(new { productID });
        }
    }

    // Hàm lấy số lượng sản phẩm trong giỏ hàng
    [HttpPost]
    public IActionResult Quantity(int productID, int quantity, double unitPrice) {
        double money = quantity * unitPrice;
        SqlParameter userIDParam = new SqlParameter("@PK_iUserID", _accessor?.HttpContext?.Session.GetInt32("UserID"));
        SqlParameter productIDParam = new SqlParameter("@PK_iProductID", productID);
        SqlParameter quantityParam = new SqlParameter("@iQuantity", quantity);
        SqlParameter moneyParam = new SqlParameter("@dMoney", money);
        _context.Database.ExecuteSqlRaw("sp_UpdateProductQuantity @PK_iUserID, @PK_iProductID, @iQuantity, @dMoney", userIDParam, productIDParam, quantityParam, moneyParam);
        return Json(new {money});
    }

    public IActionResult DeleteProduct(int productID) {
        SqlParameter userIDParam = new SqlParameter("@PK_iUserID", _accessor?.HttpContext?.Session.GetInt32("UserID"));
        SqlParameter productIDParam = new SqlParameter("@PK_iProductID", productID);
        _context.Database.ExecuteSqlRaw("sp_DeleteProductInCart @PK_iUserID, @PK_iProductID", userIDParam, productIDParam);
        string msg = "Sản phẩm đã được xoá khỏi giỏ hàng!";
        return Ok(new {msg});
    }
}