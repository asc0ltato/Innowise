using Dapper;
using Task3.Database.Interfaces;
using Microsoft.Data.SqlClient;

namespace Task3.Database;

public class DatabaseInitializer
{
    private readonly string _connectionString;
    private readonly string _databaseName;

    public DatabaseInitializer(string connectionString, string databaseName = "Task3")
    {
        _connectionString = connectionString;
        _databaseName = databaseName;
    }

    public async Task InitializeAsync()
    {
        var masterConnection = GetMasterConnection();
        
        using var connection = new SqlConnection(masterConnection);
        await connection.OpenAsync();
        
        var dropDbSql = $"DROP DATABASE IF EXISTS [{_databaseName}]";
        await connection.ExecuteAsync(dropDbSql);
        
        var createDbSql = $"CREATE DATABASE [{_databaseName}]";
        await connection.ExecuteAsync(createDbSql);

        using var dbConnection = new SqlConnection(_connectionString);
        await dbConnection.OpenAsync();

        const string createTableSql = @"
            CREATE TABLE Tasks (
                Id INT PRIMARY KEY IDENTITY(1,1),
                Title NVARCHAR(255) NOT NULL,
                Description NVARCHAR(MAX),
                IsCompleted BIT DEFAULT 0,
                CreatedAt DATETIME DEFAULT GETDATE()
            )";

        await dbConnection.ExecuteAsync(createTableSql);
        Console.WriteLine("Database and table recreated successfully");
    }

    private string GetMasterConnection()
    {
        var builder = new SqlConnectionStringBuilder(_connectionString)
        {
            InitialCatalog = "master"
        };
        return builder.ToString();
    }
}