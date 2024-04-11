using Project.Models;

public interface IUserResponsitory
{
    IEnumerable<User> login(string email, string password);
    bool register(UserViewModel user);
    IEnumerable<User> checkUserLogin(int userID);
    
}