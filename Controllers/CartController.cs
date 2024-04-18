using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using Project.Models;

public class CartController : Controller {
    private readonly IHttpContextAccessor _accessor;
    private readonly DatabaseContext _context;
    private readonly ICartReponsitory _cartResponsitory;
    private readonly IUserResponsitory _userResponsitory;
    public CartController(IHttpContextAccessor accessor, DatabaseContext context, ICartReponsitory cartReponsitoty, IUserResponsitory userResponsitory)
    {
        _accessor = accessor;
        _context = context;
        _cartResponsitory = cartReponsitoty;
        _userResponsitory = userResponsitory;
    }

    [Route("cart")]
    public IActionResult Index() {
        var userID = _accessor?.HttpContext?.Session.GetInt32("UserID");  
        IEnumerable<CartDetail> carts = _cartResponsitory.getCartInfo(Convert.ToInt32(userID)); 
        // Lấy số lượng giỏ hàng
        int cartCount = carts.Count();
        ProductViewModel model = new ProductViewModel {
            CartDetails = carts,
            CartCount = cartCount
        };
        return View(model); 
    }

    [HttpPost]
    public IActionResult GetCartInfo() {
        var userID = _accessor?.HttpContext?.Session.GetInt32("UserID");  
        IEnumerable<CartDetail> carts = _cartResponsitory.getCartInfo(Convert.ToInt32(userID));  
        int cartCount = carts.Count();
        ProductViewModel model = new ProductViewModel {
            CartDetails = carts,
            CartCount = cartCount
        };
        return Json(model);  
    }

    [HttpPost]
    public IActionResult AddToCart(int productID, double unitPrice, int quantity)
    {
        var sessionUserID = _accessor?.HttpContext?.Session.GetInt32("UserID");
        if (sessionUserID == null) {
            sessionUserID = 0;
        } 
        SqlParameter userIDParam = new SqlParameter("@PK_iUserID", sessionUserID);
        List<User> user = _userResponsitory.checkUserLogin(Convert.ToInt32(sessionUserID)).ToList();

        List<CartDetail> checkProduct = _cartResponsitory.checkProduct(Convert.ToInt32(sessionUserID), productID).ToList();

        if (user.Count() == 0)
        {
            string msg = "Bạn phải đăng nhập mới được thêm vào giỏ hàng!";
            return Json(new { msg });
        } else if (checkProduct.Count() != 0) // Kiểm tra sản phẩm bị trùng trong giỏ hàng
        {
            string msg = "Sản phẩm này đã có trong giỏ hàng";
            return Json(new {msg});
        }
        else
        {
            // https://www.c-sharpcorner.com/blogs/date-and-time-format-in-c-sharp-programming1
            // Thêm mã giỏ hàng
            SqlParameter updateTimeParam = new SqlParameter("@dUpdateTime", DateTime.Now.ToString("dd/MM/yyyy"));
            List<Cart> cart = _cartResponsitory.checkCartIDExist().ToList();

            int cartID;
            if (cart.Count() != 0) {
                cartID = cart[0].PK_iCartID;
                var updateTime = cart[0].dUpdateTime;
            } else {
                _context.Database.ExecuteSqlRaw("SET DATEFORMAT dmy EXEC sp_InsertCart @dUpdateTime", updateTimeParam);
                List<Cart> newCart = _context.Carts.FromSqlRaw("SET DATEFORMAT dmy EXEC sp_GetCartIDByTime @dUpdateTime", updateTimeParam).ToList();
                cartID = newCart[0].PK_iCartID;
            }
            // Thêm vào chi tiết giỏ hàng
            _cartResponsitory.insertCartDetail(Convert.ToInt32(sessionUserID), productID, cartID, quantity, unitPrice);
            IEnumerable<CartDetail> carts = _cartResponsitory.getCartInfo(Convert.ToInt32(sessionUserID));
            IEnumerable<CartDetail> cartDetails = _cartResponsitory.getCartInfo(Convert.ToInt32(sessionUserID)).ToList();
            int cartCount = carts.Count();
            ProductViewModel model = new ProductViewModel {
                CartCount = cartCount,
                CartDetails = cartDetails
            };
            string msg = "Thêm vào giỏ hàng thành công!";
            return Json(new { msg, model });
        }
    }

    // Hàm lấy số lượng sản phẩm trong giỏ hàng
    [HttpPost]
    public IActionResult Quantity(int productID, int quantity, double unitPrice) {
        double money = quantity * unitPrice;
        _cartResponsitory.changeQuantity(Convert.ToInt32(_accessor?.HttpContext?.Session.GetInt32("UserID")), productID, quantity, money);
        return Json(new {money});
    }

    [HttpPost]
    public IActionResult DeleteProduct(int productID) {
        _cartResponsitory.deleteProductInCart(productID, Convert.ToInt32(_accessor?.HttpContext?.Session.GetInt32("UserID")));
        string msg = "Sản phẩm đã được xoá khỏi giỏ hàng!";
        IEnumerable<CartDetail> cartDetails = _cartResponsitory.getCartInfo(Convert.ToInt32(_accessor?.HttpContext?.Session.GetInt32("UserID")));
        int cartCount = cartDetails.Count();
        CartViewModel model = new CartViewModel {
            CartCount = cartCount,
            Message = msg
        };
        return Ok(model);
    }
}