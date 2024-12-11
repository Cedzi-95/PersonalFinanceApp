using Npgsql;

public class postgresAccount : IaccountManager
{
    private NpgsqlConnection connection;
}