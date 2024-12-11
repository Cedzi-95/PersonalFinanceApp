public interface IUserService
{
    User RegisterUser(string username, string password);
    User? login(string username, string password);
    void Logout();
    User? GetLoggedInUser();
}