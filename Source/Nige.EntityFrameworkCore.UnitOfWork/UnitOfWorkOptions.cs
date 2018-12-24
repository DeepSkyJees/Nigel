namespace Nige.EntityFrameworkCore.UnitOfWork
{
    public class UnitOfWorkOptions
    {
        public DatabaseType DatabaseType { get; set; } = DatabaseType.SqlServer;
    }
}