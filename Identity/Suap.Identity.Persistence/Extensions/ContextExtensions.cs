using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql;

namespace Suap.Identity.Persistence.Extensions
{
    public static class ContextExtensions
    {
        public static void ConfigDatabase(this DbContextOptionsBuilder builder, string connectionString, string? assembly = null)
        {
            //#if DEBUG
            //            var connectionBuilder = new NpgsqlConnectionStringBuilder(connectionString);
            //            connectionBuilder.Database = connectionBuilder.Database + "_" + Environment.UserName;
            //            connectionString = connectionBuilder.ToString();
            //#endif

            builder
                .UseNpgsql(connectionString, a =>
                {
                    string? scheme = connectionString.GetScheme();
                    if (scheme != null)
                        a.MigrationsHistoryTable(HistoryRepository.DefaultTableName, scheme);
                    if (assembly != null)
                        a.MigrationsAssembly(assembly);
                })
                .UseSnakeCaseNamingConvention();

        }

        public static string? GetScheme(this string connectionString)
        {
            return connectionString.Split(';')
                .Where(x => x.StartsWith("Search Path", StringComparison.OrdinalIgnoreCase))
                .Select(s => s.Split('=').Last())
                .FirstOrDefault();
        }
    }
}