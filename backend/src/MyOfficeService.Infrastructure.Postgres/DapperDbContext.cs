using System.Data;
using System.Data.Common;
using Npgsql;

namespace MyOfficeService.Infrastructure.Postgres;

/// <summary>
/// DapperDbContext - создаёт подключение и кладёт в своё свойство,
/// для того, чтобы открыть подключения надо вызывать OpenConnectionAsync.
/// Реализован DisposeAsync для закрытия соединения.
/// </summary>
public class DapperDbContext : IAsyncDisposable
{
    public DbConnection Database { get; }

    public DapperDbContext(string connectionString)
    {
        Database = new NpgsqlConnection(connectionString);
    }

    public async Task OpenConnectionAsync(CancellationToken cancellationToken = default)
    {
        if (Database.State != ConnectionState.Open)
        {
            await Database.OpenAsync(cancellationToken);
        }
    }

    public async ValueTask DisposeAsync()
    {
        await Database.DisposeAsync();
    }
}