
using Microsoft.EntityFrameworkCore;
using Project.Models;

public class HomeResponsitory : IHomeResponsitory
{
    private readonly DatabaseContext _context;
    public HomeResponsitory(DatabaseContext context)
    {
        _context = context;
    }
    public IEnumerable<Product> GetProducts() {
        return _context.Products.FromSqlRaw("EXEC sp_SelectProducts").ToList();
    }
    public IEnumerable<Category> GetCategories() { 
        throw new NotImplementedException();
    }
}