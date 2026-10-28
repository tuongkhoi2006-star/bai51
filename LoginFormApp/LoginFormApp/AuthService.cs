using System.Security.Cryptography;
using Microsoft.Data.SqlClient;

namespace LoginFormApp;

internal static class AuthService
{
    private const int SaltSize = 16;
    private const int HashSize = 32;
    private const int Iterations = 600_000;

    private static byte[] HashPassword(string password, byte[] salt) =>
        Rfc2898DeriveBytes.Pbkdf2(
            password, salt, Iterations, HashAlgorithmName.SHA256, HashSize);

    internal static bool Register(string username, string password)
    {
        byte[] salt = RandomNumberGenerator.GetBytes(SaltSize);
        byte[] hash = HashPassword(password, salt);

        using var conn = Database.GetConnection();
        conn.Open();

        const string sql = """
            INSERT INTO dbo.Users (Username, PasswordHash, PasswordSalt)
            VALUES (@Username, @Hash, @Salt);
            """;

        using var cmd = new SqlCommand(sql, conn);
        cmd.Parameters.Add("@Username", System.Data.SqlDbType.NVarChar, 50).Value = username;
        cmd.Parameters.Add("@Hash", System.Data.SqlDbType.NVarChar, 256).Value =
            Convert.ToBase64String(hash);
        cmd.Parameters.Add("@Salt", System.Data.SqlDbType.NVarChar, 256).Value =
            Convert.ToBase64String(salt);

        return cmd.ExecuteNonQuery() == 1;
    }

    internal static bool Login(string username, string password)
    {
        using var conn = Database.GetConnection();
        conn.Open();

        const string sql = """
            SELECT PasswordHash, PasswordSalt
            FROM dbo.Users
            WHERE Username = @Username;
            """;

        using var cmd = new SqlCommand(sql, conn);
        cmd.Parameters.Add("@Username", System.Data.SqlDbType.NVarChar, 50).Value = username;

        using var reader = cmd.ExecuteReader();
        if (!reader.Read())
            return false;

        byte[] expectedHash = Convert.FromBase64String(reader.GetString(0));
        byte[] salt = Convert.FromBase64String(reader.GetString(1));
        byte[] actualHash = HashPassword(password, salt);

        return CryptographicOperations.FixedTimeEquals(expectedHash, actualHash);
    }
}
