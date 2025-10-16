using Dapper;
using Task3.Database.Interfaces;
using Task3.Models;
using Task3.Repositories.Interfaces;

namespace Task3.Repositories;

public class TaskRepository : ITaskRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public TaskRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<TaskItem?> GetByIdAsync(int id)
    {
        using var connection = _connectionFactory.CreateConnection();
        const string sql = "SELECT * FROM Tasks WHERE Id = @Id";
        return await connection.QueryFirstOrDefaultAsync<TaskItem>(sql, new { Id = id });
    }

    public async Task<IEnumerable<TaskItem>> GetAllAsync()
    {
        using var connection = _connectionFactory.CreateConnection();
        const string sql = "SELECT * FROM Tasks ORDER BY CreatedAt DESC";
        return await connection.QueryAsync<TaskItem>(sql);
    }

    public async Task<bool> AddAsync(TaskItem task)
    {
        using var connection = _connectionFactory.CreateConnection();
        const string sql = @"
            INSERT INTO Tasks (Title, Description, IsCompleted, CreatedAt)
            VALUES (@Title, @Description, @IsCompleted, @CreatedAt)";
        
        var resultRows = await connection.ExecuteAsync(sql, task);
        return resultRows > 0;
    }

    public async Task<bool> UpdateAsync(TaskItem task)
    {
        using var connection = _connectionFactory.CreateConnection();
        const string sql = @"
            UPDATE Tasks SET Title = @Title, Description = @Description, IsCompleted = @IsCompleted
            WHERE Id = @Id";
        var resultRows = await connection.ExecuteAsync(sql, task);
        return resultRows > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        using var connection = _connectionFactory.CreateConnection();
        const string sql = "DELETE FROM Tasks WHERE Id = @Id";
        var resultRows = await connection.ExecuteAsync(sql, new { Id = id });
        return resultRows > 0;
    }
}