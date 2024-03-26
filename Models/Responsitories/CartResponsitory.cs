
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Project.Models;
public class CartResponsitory : ICartReponsitory
{
    private readonly DatabaseContext _context;
    public CartResponsitory(DatabaseContext context)
    {
        _context = context;
    }

    public IEnumerable<CartDetail> getCartInfo(int userID) {
        SqlParameter userIDParam = new SqlParameter("PK_iUserID", userID);
        return _context.CartDetails.FromSqlRaw("EXEC sp_GetInfoCart @PK_iUserID", userIDParam);
    }

    public IEnumerable<Cart> checkCartIDExist()
    {
        SqlParameter updateTimeParam = new SqlParameter("@dUpdateTime", DateTime.Now.ToString("dd/MM/yyyy"));
        return _context.Carts.FromSqlRaw("SET DATEFORMAT dmy EXEC sp_GetCartIDByTime @dUpdateTime", updateTimeParam);
    }

    public bool insertCart()
    {
        SqlParameter updateTimeParam = new SqlParameter("@dUpdateTime", DateTime.Now.ToString("dd/MM/yyyy"));   
        _context.Database.ExecuteSqlRaw("SET DATEFORMAT dmy EXEC sp_InsertCart @dUpdateTime", updateTimeParam);
        return true;
    }

    public bool insertCartDetail(int userID, int productID, int cartID, int quantity, double unitPrice) {
        SqlParameter userIDParam = new SqlParameter("@PK_iUserID", userID);
        SqlParameter productIDParam = new SqlParameter("@PK_iProductID", productID);
        SqlParameter cartIDParam = new SqlParameter("@PK_iCartID", cartID);
        SqlParameter quantityParam = new SqlParameter("@iQuantity", quantity);
        SqlParameter unitPriceParam = new SqlParameter("@dUnitPrice", unitPrice);
        SqlParameter discountParam = new SqlParameter("@dDiscount", 1);
        SqlParameter moneyParam = new SqlParameter("@dMonney", unitPrice * quantity);
        _context.Database.ExecuteSqlRaw("sp_InsertProductIntoCartDetail @PK_iUserID, @PK_iProductID, @PK_iCartID, @iQuantity, @dUnitPrice, @dDiscount, @dMonney", userIDParam, productIDParam, cartIDParam, quantityParam, unitPriceParam, discountParam, moneyParam);
        return true;
    }

    public IEnumerable<CartDetail> checkProduct(int userID, int productID)
    {
        SqlParameter userIDPram = new SqlParameter("@PK_iUserID", userID);
        SqlParameter productIDPram = new SqlParameter("@PK_iProductID", productID);
        return _context.CartDetails.FromSqlRaw("sp_CheckProductInCartDetail @PK_iUserID, @PK_iProductID", userIDPram, productIDPram);
    }
}