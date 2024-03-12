using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Project.Models;

public class ProductResponsitory : IProductResponsitory {
    private readonly DatabaseContext _context;
    public ProductResponsitory(DatabaseContext context)
    {
        _context = context;
    }

    public IEnumerable<Product> getProductsByCategoryID(int categoryID)
    {
        SqlParameter categoryIDParam = new SqlParameter("@FK_iCategoryID", categoryID);
        return _context.Products.FromSqlRaw("EXEC sp_SelectProductsByCategoryID @FK_iCategoryID", categoryIDParam);
    }
}