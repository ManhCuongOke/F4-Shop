
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Project.Models;
public class CartResponsitory : ICartReponsitoty
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
}