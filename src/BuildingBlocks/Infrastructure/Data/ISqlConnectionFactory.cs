using System.Data;

namespace Library.BuildingBlocks.Infrastructure.Data
{
    public interface ISqlConnectionFactory
    {
        IDbConnection GetOpenConnection();

        IDbConnection CreateNewConnection();

        string GetConnectionString();
    }
}
