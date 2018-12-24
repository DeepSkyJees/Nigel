namespace Nige.EntityFrameworkCore.UnitOfWork
{
    public interface IJoinEntity<TEntity>
    {
        TEntity Navigation { get; set; }
    }
}