namespace PersonalFinanceApp;

using System;
using Npgsql;
class Program



{
    static void Main(string[] args)
    {
        string connectionString = "Host=localhost;Username=postgres;Password=password;Database=personal_finance";
        using var connection = new NpgsqlConnection(connectionString);
        connection.Open();
        
     var createTableSql = @"
        
        CREATE TABLE IF NOT EXISTS users (
        user_id UUID PRIMARY KEY,
        username TEXT UNIQUE,
        password TEXT
        );

        
        CREATE TABLE IF NOT EXISTS accounts (
        account_id UUID PRIMARY KEY,
        user_id UUID,
        
        
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
        IaccountManager accountManager = new PostgresAccount(userService, connection);
        ImenuService menuService = new SimpleMenuService();

        Menu initialMenu = new LoginMenu(userService, menuService, accountManager);
        menuService.SetMenu(initialMenu);

        while (true)
        {
            try
            {
                
                Console.Write($"{Colours.GREEN}\n>{Colours.NORMAL} ");
                string? input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input))
                    continue;

                if (input.ToLower() == "exit")
                    break;

               

                menuService.GetMenu().ExecuteCommand(input.ToLower());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
        }




     
    


