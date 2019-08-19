using Microsoft.EntityFrameworkCore;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class EntityFrameworkIdentityServerBuilderExtensions
    {
        private const string DefaultSchema = "idsrv";
        private const string MigrationHistoryTable = "__EFMigrationsHistory";

        public static IIdentityServerBuilder AddEntityFrameworkStorage(this IIdentityServerBuilder builder, string connectionString)
        {
            builder.AddConfigurationStore(options =>
            {
                // use a different schema for the IdentityServer tables.
                options.DefaultSchema = DefaultSchema;
                options.ConfigureDbContext = GetDefaultDbBuilder(connectionString);
            });

            builder.AddOperationalStore(options =>
            {
                options.DefaultSchema = DefaultSchema;
                options.ConfigureDbContext = GetDefaultDbBuilder(connectionString);

                // this enables automatic token cleanup. this is optional.
                options.EnableTokenCleanup = true;
            });

            return builder;
        }

        /// <summary>
        /// Get a configured <see cref="DbContextOptionsBuilder" /> to the store the IdentityServer
        /// configuration data and operational data in the same database as Event Management uses.
        /// </summary>
        private static Action<DbContextOptionsBuilder> GetDefaultDbBuilder(string connectionString)
        {
            var migrationsAssembly =
                typeof(EntityFrameworkIdentityServerBuilderExtensions).Assembly.GetName().Name;

            return builder =>
                builder.UseSqlServer(connectionString,
                    sql => sql.MigrationsAssembly(migrationsAssembly)
                              .MigrationsHistoryTable(MigrationHistoryTable, DefaultSchema));
        }
    }
}
