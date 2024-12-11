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
        user_name TEXT,
        password TEXT
        );

        
        CREATE TABLE accounts (
        account_id UUID PRIMARY KEY,
        user_id UUID,
        balance DECIMAL (15,2),
        CONSTRAINT fk_user
        FOREIGN KEY (user_id) REFERENCES users (user_id) ON DELETE CASCADE
        );

                
        CREATE TABLE IF NOT EXISTS transactions (
        transaction_id uuid PRIMARY KEY,
        account_id UUID NOT NULL,
        transaction_date TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
        amount DECIMAL (15,2),
        CONSTRAINT fk_account
        FOREIGN KEY (account_id) REFERENCES accounts (account_id) ON DELETE CASCADE
        );
        
        ";

        using var createTableCmd = new NpgsqlCommand(createTableSql, connection);
        createTableCmd.ExecuteNonQuery();




     Menu.Execute();
     
    }

}
