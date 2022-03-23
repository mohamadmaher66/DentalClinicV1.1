using Microsoft.EntityFrameworkCore;
using System;

namespace Infrastructure
{
    public interface IUnitOfWork : IDisposable
    {
        DbContext DbContext { get; }
        void SaveChanges();
    }
}
