using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Threading.Tasks;

namespace eLTMS.DataAccess.Infrastructure
{
    public interface IUnitOfWork : IDisposable
    {
        DbContext Context { get; set; }
        string ConnectionString { get; set; }
        ICollection<ValidationResult> SaveChanges();
        Task<ICollection<ValidationResult>> SaveChangesAsync();
        IDbContextTransactionProxy BeginTransaction();
    }
}