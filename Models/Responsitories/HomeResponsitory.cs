
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Project.Models;

public class HomeResponsitory : IHomeResponsitory
{
    private readonly DatabaseContext _context;
    public HomeResponsitory(DatabaseContext context)
    {
        _context = context;
    }
    public IEnumerable<Product> getProducts() {
        return _context.Products.FromSqlRaw("EXEC sp_SelectProducts").ToList();
    }
    public IEnumerable<Category> getCategories() { 
        return _context.Categories.FromSqlRaw("EXEC sp_SelectCategories").ToList();
    }

    public IEnumerable<Product> displayProductsPagination(int pageSize, int pageNumber)
    {
        SqlParameter pageSizeParam = new SqlParameter("@PageSize", pageSize);
        SqlParameter pageNumberParam = new SqlParameter("@PageNumber", pageNumber);
        return _context.Products.FromSqlRaw("EXEC sp_PaginationProducts @PageSize, @PageNumber", pageSizeParam, pageNumberParam);
    }

    public IEnumerable<Category> searchProductsByKeyword(string keyword) {
        SqlParameter keywordParam = new SqlParameter("@sKeyword", keyword);
        return _context.Categories.FromSqlRaw("EXEC sp_SearchCategoryByKeyword @sKeyword", keywordParam);
    }
}