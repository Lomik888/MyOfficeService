using DbUp;

namespace MyOfficeService.Infrastructure.Postgres.Migrations;

public class DatabaseInitializer
{
    private readonly string _connectionString;

    public DatabaseInitializer(string connectionString)
    {
        _connectionString = connectionString;
    }

    public void Initialize()
    {
        EnsureDatabase.For.PostgresqlDatabase(_connectionString);

        var upgrader = DeployChanges.To
            .PostgresqlDatabase(_connectionString)
            .WithScriptsEmbeddedInAssembly(typeof(DatabaseInitializer).Assembly)
            .LogToConsole()
            .Build();

        if (upgrader.IsUpgradeRequired())
        {
            var result = upgrader.PerformUpgrade();

            if (result.Successful == false)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(result.Error);
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Success!");
            }

            Console.ResetColor();
        }
    }
}