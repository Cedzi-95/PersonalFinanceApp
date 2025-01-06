using System.Security.Cryptography;
using System.Text;
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
        var sql = @"SELECT * FROM users WHERE username = @username";
        using var cmd = new NpgsqlCommand(sql, this.connection);
        cmd.Parameters.AddWithValue("@username", username);
        
        using var reader = cmd.ExecuteReader();
     while (reader.Read())
     {
       

         var user = new User 
        {
            UserId = reader.GetGuid(0),
            Name = reader.GetString(1),
            Password = reader.GetString(2)
        };

       

        string [] passwordSplit = user.Password.Split(":");
        string storedHash = passwordSplit[0];
        string salt = passwordSplit[1];

        byte[] fullBytes = Encoding.UTF8.GetBytes(password + salt);
        string computedHash;
        using(HashAlgorithm algorithm = SHA256.Create())
        {
            byte[] hash = algorithm.ComputeHash(fullBytes);
            computedHash = GetHexString(hash);
        }

        if (!storedHash.Equals(computedHash)) //compare just the hashes
        {
            continue;
        }

        LoggedInuser = user.UserId;
        


        return user;
     }

     return null;

       
    }

    public User Logout()
    {
         LoggedInuser = null;
         return null;
    }

    public User RegisterUser(string username, string password)
    {
        byte[] salt = RandomNumberGenerator.GetBytes(16);
        string saltstring = GetHexString(salt);

        byte[] fullBytes = Encoding.UTF8.GetBytes(password + saltstring);

        using (HashAlgorithm algorithm = SHA256.Create())
        {
            byte[] hash = algorithm.ComputeHash(fullBytes);
            password = GetHexString(hash);
        }
        password += ":" + saltstring;

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
       //debug by printing password
       Console.WriteLine($"About to store password: {password}");
       
       return user;
    }
    
    private static string GetHexString(byte[] array)
    {
        StringBuilder sb = new StringBuilder();
        foreach(byte b in array)
        {
            sb.Append(b.ToString("x2"));
        }
        return sb.ToString();
    }

    private void CreateAccountForUser(Guid userId)
    {
       


        var sql = @"INSERT INTO accounts(account_id, user_id) VALUES
        (@account_id, @user_id)";
        using var cmd = new NpgsqlCommand(sql, connection);
        cmd.Parameters.AddWithValue("@account_id", Guid.NewGuid());
        cmd.Parameters.AddWithValue("@user_id", userId);


        cmd.ExecuteNonQuery();
    }

    
}