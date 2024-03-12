using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Project.Models;

public class CartController : Controller {
    private readonly IHttpContextAccessor _accessor;
    private readonly DatabaseContext _context;
    private readonly ICartReponsitoty _cartResponsitory;
    public CartController(IHttpContextAccessor accessor, DatabaseContext context, ICartReponsitoty cartReponsitoty)
    {
        _accessor = accessor;
        _context = context;
        _cartResponsitory = cartReponsitoty;
    }

    public IActionResult Index() {
        // Fix cứng dữ liệu
        _accessor?.HttpContext?.Session.SetInt32("UserID", 1);

        // Them comment
        var userID = _accessor?.HttpContext?.Session.GetInt32("UserID");
        IEnumerable<CartDetail> carts = _cartResponsitory.getCartInfo(Convert.ToInt32(userID)).ToList();
        int cartCount = carts.Count();
        _accessor?.HttpContext?.Session.SetInt32("CartCount", cartCount);
        
        return View(carts);
    }

    [HttpPost]
    public IActionResult GetCartInfo() {
        var userID = _accessor?.HttpContext?.Session.GetInt32("UserID");
        SqlParameter userIDParam = new SqlParameter("@PK_iUserID", userID);
        IEnumerable<CartDetail> carts = _context.CartDetails.FromSqlRaw("sp_GetInfoCart @PK_iUserID", userIDParam);
        CartViewModel model = new CartViewModel {
            Carts = carts,
            CartCount = carts.Count()
        };
        return Json(model);
    }

    [Route("/Cart/AddToCart/{productID?}/{unitPrice?}/{quantity?}")]
    [HttpGet("/Cart/AddToCart/{productID?}/{unitPrice?}/{quantity?}")]
    public IActionResult AddToCart(int productID, double unitPrice, int quantity)
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
            // https://www.c-sharpcorner.com/blogs/date-and-time-format-in-c-sharp-programming1
            // Thêm mã giỏ hàng
            SqlParameter updateTimeParam = new SqlParameter("@dUpdateTime", DateTime.Now.ToString("dd/MM/yyyy"));
            List<Cart> cart = _context.Carts.FromSqlRaw("SET DATEFORMAT dmy EXEC sp_GetCartIDByTime @dUpdateTime", updateTimeParam).ToList();
            int cartID;
            if (cart != null) {
                cartID = cart[0].PK_iCartID;
                var updateTime = cart[0].dUpdateTime;
            } else {
                _context.Database.ExecuteSqlRaw("SET DATEFORMAT dmy EXEC sp_InsertCart @dUpdateTime", updateTimeParam);
                List<Cart> newCart = _context.Carts.FromSqlRaw("SET DATEFORMAT dmy EXEC sp_GetCartIDByTime @dUpdateTime", updateTimeParam).ToList();
                cartID = newCart[0].PK_iCartID;
            }
            // Thêm vào chi tiết giỏ hàng
            SqlParameter productIDParam = new SqlParameter("@PK_iProductID", productID);
            SqlParameter cartIDParam = new SqlParameter("@PK_iCartID", cartID);
            SqlParameter quantityParam = new SqlParameter("@iQuantity", quantity);
            SqlParameter unitPriceParam = new SqlParameter("@dUnitPrice", unitPrice);
            SqlParameter discountParam = new SqlParameter("@dDiscount", 1);
            SqlParameter moneyParam = new SqlParameter("@dMonney", unitPrice * quantity);
            _context.Database.ExecuteSqlRaw("sp_InsertProductIntoCartDetail @PK_iUserID, @PK_iProductID, @PK_iCartID, @iQuantity, @dUnitPrice, @dDiscount, @dMonney", userIDParam, productIDParam, cartIDParam, quantityParam, unitPriceParam, discountParam, moneyParam);
            string msg = "Thêm vào giỏ hàng thành công!";
            return Json(new {msg});
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