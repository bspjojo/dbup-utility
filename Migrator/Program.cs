using System;
using System.IO;
using System.Reflection;
using DbUp;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Binder;

namespace Migrator
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello.");

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            IConfigurationRoot configuration = builder.Build();

            var connectionString = configuration.GetConnectionString("ConnectionString");

            var migrationSection = configuration.GetSection("Migration");
            var folder = migrationSection.GetValue<string>("FolderPath");
            var ensureDatabaseExists = migrationSection.GetValue<bool>("EnsureDatabaseExists");

            Console.WriteLine($"Folder: {folder}");

            DatabaseSetup(connectionString, folder, ensureDatabaseExists);
        }

        private static void DatabaseSetup(string connectionString, string path, bool ensureDatabaseExists)
        {
            Console.WriteLine("Beginning migration.");

            if (ensureDatabaseExists)
            {
                EnsureDatabase.For.SqlDatabase(connectionString);
            }

            var upgrader = DeployChanges.To.SqlDatabase(connectionString)
                .WithTransactionPerScript()
                .WithScriptsFromFileSystem(path)
                .LogToConsole()
                .Build();

            var result = upgrader.PerformUpgrade();

            if (!result.Successful)
            {
                Console.WriteLine("Migration failed.");

                throw result.Error;
            }
            Console.WriteLine("Migration success.");
        }
    }
}
