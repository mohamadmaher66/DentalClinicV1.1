using DBContext;

namespace Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        public DentalClinicDBContext DbContext { get; }
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
