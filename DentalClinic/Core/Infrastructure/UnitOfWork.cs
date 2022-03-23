using AppDBContext;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        public DbContext DbContext { get; }
        public UnitOfWork(DentalClinicDBContext dbContext)
        {
            DbContext = dbContext;
        }
        public void SaveChanges()
        {
            DbContext.SaveChanges();
        }
        public void Dispose()
        {
            DbContext.Dispose();
        }
    }
}
