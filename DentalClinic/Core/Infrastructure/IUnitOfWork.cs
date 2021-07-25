using DBContext;
using System;

namespace Infrastructure
{
    internal interface IUnitOfWork : IDisposable
    {
        DentalClinicDBContext DbContext { get; }
        void SaveChanges();
    }
}
