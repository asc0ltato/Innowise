using System.Data;

namespace Task3.Database.Interfaces;

public interface IDbConnectionFactory
{
    IDbConnection CreateConnection();
}