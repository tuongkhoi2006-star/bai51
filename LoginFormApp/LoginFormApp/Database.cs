using Microsoft.Data.SqlClient;

namespace LoginFormApp;

internal static class Database
{
    // SQL Server LocalDB connection. Change Server if your SQL Server instance differs.
    internal static string ConnectionString =
        @"Server=(localdb)\MSSQLLocalDB;Database=LoginDB;Integrated Security=True;TrustServerCertificate=True;";

    internal static SqlConnection GetConnection() => new(ConnectionString);
}
