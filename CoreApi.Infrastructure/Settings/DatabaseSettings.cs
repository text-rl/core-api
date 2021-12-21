using System.Data;

namespace CoreApi.Infrastructure.Settings
{
    public enum DbType
    {
        SqlServer,
        MySql
    }

    public class DatabaseSettings
    {
        public DbType Type { get; set; }
        public string? ConnectionString { get; set; }
    }
}