using Project.Models;

public interface IUserResponsitory
{
    IEnumerable<User> login(string email, string password);
    bool register(RegistrastionModel user);
    IEnumerable<User> checkUserLogin(int userID);
    string encrypt(string decryted);
    string decrypt(string encrypted);
}