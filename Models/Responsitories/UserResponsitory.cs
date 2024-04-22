using System.Text;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Project.Models;
using System.Security.Cryptography;
using Azure;

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

    // Phương thức giải mã
    public string decrypt(string encrypted)
    {
        string hash = "cong@gmail.com";
        byte[] data = Convert.FromBase64String(encrypted);
        MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
        TripleDESCryptoServiceProvider tripDES = new TripleDESCryptoServiceProvider();
        tripDES.Key = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(hash));
        tripDES.Mode = CipherMode.ECB;
        ICryptoTransform transform = tripDES.CreateDecryptor();
        byte[] result = transform.TransformFinalBlock(data, 0, data.Length);
        return UTF8Encoding.UTF8.GetString(result);
    }

    // Phương thức mã hoá
    public string encrypt(string decryted)
    {
        string hash = "cong@gmail.com";
        byte[] data = UTF8Encoding.UTF8.GetBytes(decryted);
        MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
        TripleDESCryptoServiceProvider tripDES = new TripleDESCryptoServiceProvider();
        tripDES.Key = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(hash));
        tripDES.Mode = CipherMode.ECB;
        ICryptoTransform transform = tripDES.CreateEncryptor();
        byte[] result = transform.TransformFinalBlock(data, 0, data.Length);

        return Convert.ToBase64String(result);
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
        SqlParameter addressParam = new SqlParameter("@sAddress", user.sAddress);
        SqlParameter createTimeParam = new SqlParameter("@dCreateTime", DateTime.Now);
        SqlParameter passwordParam = new SqlParameter("@sPassword", user.sPassword);
        _context.Database.ExecuteSqlRaw("EXEC sp_InsertUser @FK_iRoleID, @sName, @sEmail, @sAddress, @dCreateTime, @sPassword", roleIdParam, nameParam, emailParam, addressParam, createTimeParam, passwordParam);
        return true;
    }
}