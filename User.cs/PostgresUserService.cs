using Npgsql;
public class PostgresUserService : IUserService
{
    private NpgsqlConnection connection;

    private Guid? LoggedInuser = null;
    

    public PostgresUserService(NpgsqlConnection connection) 
    {
        this.connection = connection;
    }

    public User? GetLoggedInUser()
    {
        if (LoggedInuser == null)
        {
            return null;
        }

        var sql = @"SELECT * FROM users WHERE user_id = @userId";
        using var cmd = new NpgsqlCommand(sql, this.connection);

        cmd.Parameters.AddWithValue("@userId", LoggedInuser);

        using var reader = cmd.ExecuteReader();

        if (!reader.Read())
        {
            return null;
        }
        var user = new User 
        {
            UserId = reader.GetGuid(0),
            Name = reader.GetString(1),
            Password = reader.GetString(2)
        };
        return user;
    }

    public User? login(string username, string password)
    {
        var sql = @"SELECT * FROM users WHERE username = @username AND password = @password";
        using var cmd = new NpgsqlCommand(sql, this.connection);
        cmd.Parameters.AddWithValue("@username", username);
        cmd.Parameters.AddWithValue("@password", password);

        using var reader = cmd.ExecuteReader();
        if(!reader.Read())
        {
            return null;
        }

        var user = new User 
        {
            UserId = reader.GetGuid(0),
            Name = reader.GetString(1),
            Password = reader.GetString(2)
        };

        LoggedInuser = user.UserId;

        return user;
    }

    public User Logout()
    {
         LoggedInuser = null;
         return null;
    }

    public User RegisterUser(string username, string password)
    {
       var user = new User
       {
        UserId = Guid.NewGuid(),
        Name = username,
        Password = password
       };

       var sql = @"INSERT INTO users(user_id, username, password) VALUES (
       @user_id,
       @username,
       @password);";

       using var cmd = new NpgsqlCommand (sql, this.connection);
       cmd.Parameters.AddWithValue("@user_id", user.UserId);
       cmd.Parameters.AddWithValue("@username", user.Name);
       cmd.Parameters.AddWithValue("@password", user.Password);

       cmd.ExecuteNonQuery();
       CreateAccountForUser(user.UserId);
       
       return user;
    }

    private void CreateAccountForUser(Guid userId)
    {
       


        var sql = @"INSERT INTO accounts(account_id, user_id, balance) VALUES
        (@account_id, @user_id, 0)";
        using var cmd = new NpgsqlCommand(sql, connection);
        cmd.Parameters.AddWithValue("@account_id", Guid.NewGuid());
        cmd.Parameters.AddWithValue("@user_id", userId);
        cmd.Parameters.AddWithValue("@balance", 0);


        cmd.ExecuteNonQuery();
    }

    void IUserService.Logout()
    {
        throw new NotImplementedException();
    }
}