using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Project.Models;

public class UserResponsitory : IUserResponsitory
{
    private readonly DatabaseContext _context;
    public UserResponsitory(DatabaseContext context)
    {
        _context = context;
    }

    public IEnumerable<User> login(string email, string password)
    {
        SqlParameter emailParam = new SqlParameter("@sEmail", email);
        SqlParameter passwordParam = new SqlParameter("@sPassword", password);
        return _context.Users.FromSqlRaw("EXEC sp_LoginEmailAndPassword @sEmail, @sPassword", emailParam, passwordParam);
    }

    public bool register()
    {
        throw new NotImplementedException();
    }
}