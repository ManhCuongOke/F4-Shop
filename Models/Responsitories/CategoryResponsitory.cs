using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Project.Models;

public class CategoryResponsitory : ICategoryResponsitory
{
    private readonly DatabaseContext _context;
    public CategoryResponsitory(DatabaseContext context)
    {
        _context = context;
    }
    public bool inserCategory(Category category)
    {
        SqlParameter categoryNameParam = new SqlParameter("@sCategoryName", category.sCategoryName);
        SqlParameter categoryDescParam = new SqlParameter("@sCategoryDescription", category.sCategoryDescription);
        _context.Database.ExecuteSqlRaw("sp_InsertCategory @sCategoryName, @sCategoryDescription", categoryNameParam, categoryDescParam);
        return true;
    }
}