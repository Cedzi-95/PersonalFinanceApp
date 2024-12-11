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

        var sql = @"SELECT * FROM users WHERE user_id = @id";
        using var cmd = new NpgsqlCommand(sql, this.connection);

        cmd.Parameters.AddWithValue("@id", LoggedInuser);

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
        var sql = @"SELECT * FROM users WHERE user_name = @username AND password = @password";
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

        LoggedInuser =user.UserId;

        return user;
    }

    public void Logout()
    {
        LoggedInuser = null;
    }

    public User RegisterUser(string username, string password)
    {
        throw new NotImplementedException();
    }
}