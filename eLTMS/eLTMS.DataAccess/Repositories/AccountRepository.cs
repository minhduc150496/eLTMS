using eLTMS.DataAccess.Infrastructure;
using eLTMS.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLTMS.DataAccess.Repositories
{
    public interface IAccountRepository : IRepository<Account>
    {
        Account GetByPhoneNumber(string phoneNumber);
        Account GetByPhoneNumberIncludeRoleEmployeePatient(string phoneNumber);
    }
    public class AccountRepository : RepositoryBase<Account>, IAccountRepository
    {
        public Account GetByPhoneNumber(string phoneNumber)
        {
            var account = DbSet.AsQueryable()
                .Where(x => x.PhoneNumber.Trim().Equals(phoneNumber.Trim()))
                .ToList()
                .FirstOrDefault();
            return account;
        }

        public Account GetByPhoneNumberIncludeRoleEmployeePatient(string phoneNumber)
        {
            var account = DbSet.AsQueryable()
                .Where(x => x.PhoneNumber.Trim().Equals(phoneNumber.Trim()))
                .Include(x => x.Role_RoleId)
                .Include(x => x.Employees)
                .Include(x => x.PatientAccounts.Select(y => y.Patient))
                .ToList()
                .FirstOrDefault();
            return account;
        }
    }
}
