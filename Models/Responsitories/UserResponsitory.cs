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

    public IEnumerable<User> checkUserLogin(int userID)
    {
        SqlParameter userIDParam = new SqlParameter("@PK_iUserID", userID);
        return _context.Users.FromSqlRaw("EXEC sp_CheckUserLogin @PK_iUserID", userIDParam).ToList();
    }

    public IEnumerable<User> login(string email, string password)
    {
        SqlParameter emailParam = new SqlParameter("@sEmail", email);
        SqlParameter passwordParam = new SqlParameter("@sPassword", password);
        return _context.Users.FromSqlRaw("EXEC sp_LoginEmailAndPassword @sEmail, @sPassword", emailParam, passwordParam);
    }

    public bool register(RegistrastionModel user)
    {
        // Phải đặt enctype="multipart/form-data" thì IFromFile mới có giá trị
        SqlParameter roleIdParam = new SqlParameter("@FK_iRoleID", 1);
        SqlParameter nameParam = new SqlParameter("@sName", user.sName);
        SqlParameter emailParam = new SqlParameter("@sEmail", user.sEmail);
        SqlParameter passwordParam = new SqlParameter("@sPassword", user.sPassword);
        _context.Database.ExecuteSqlRaw("EXEC sp_InsertUser @FK_iRoleID, @sName, @sEmail, @sPassword", roleIdParam, nameParam, emailParam, passwordParam);
        return true;
    }
}