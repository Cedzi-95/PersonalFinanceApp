namespace PersonalFinanceApp;

using System;
using Npgsql;
class Program

// - Registrera användare genom namn och lösenord

// - Logga in genom namn och lösenord

// - Logga ut (och kunna byta användare genom att logga in igen)

// - All funktionalitet från tidigare uppgift, kopplat per-användare


{
    static void Main(string[] args)
    {
        string connectionString = "Host=localhost;Username=postgres;Password=password;Database=personal_finance";
        using var connection = new NpgsqlConnection(connectionString);
        connection.Open();
        
     var createTableSql = @"
        
        CREATE TABLE IF NOT EXISTS users (
        user_id UUID PRIMARY KEY,
        username TEXT,
        password TEXT
        );

        
        CREATE TABLE IF NOT EXISTS accounts (
        account_id UUID PRIMARY KEY,
        user_id UUID,
        balance DECIMAL (15,2),
        CONSTRAINT fk_user
        FOREIGN KEY (user_id) REFERENCES users (user_id) ON DELETE CASCADE
        );

                
        CREATE TABLE IF NOT EXISTS transactions (
        transaction_id uuid PRIMARY KEY,
        account_id UUID NOT NULL,
        type text,
        date TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
        amount DECIMAL (15,2),

        FOREIGN KEY (account_id) REFERENCES accounts (account_id) ON DELETE CASCADE
        );
        
        ";

        using var createTableCmd = new NpgsqlCommand(createTableSql, connection);
        createTableCmd.ExecuteNonQuery();


        IUserService userService = new PostgresUserService(connection);
        IaccountManager accountManager = new PostgresAccount(userService, connection, 0);
        ImenuService menuService = new SimpleMenuService();
        Menu initialMenu = new LoginMenu(userService, menuService, accountManager);

        menuService.SetMenu(initialMenu);

        while (true)
        {
            string? inputCommand = Console.ReadLine()!.ToLower();

            if (inputCommand != null)
            {
                menuService.GetMenu().ExecuteCommand(inputCommand);
            }
            else 
            {
                break;
            }
        }




    //  Menu.Execute();
     
    }

}
