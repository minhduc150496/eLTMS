using eLTMS.DataAccess.Infrastructure;
using eLTMS.DataAccess.Models;
using eLTMS.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLTMS.BusinessLogic.Services
{
    public interface IAccountService
    {
        Account checkLogin(string phoneNumber, string password);
    }
    public class AccountService : IAccountService
    {
        private readonly IRepositoryHelper RepositoryHelper;
        private readonly IUnitOfWork UnitOfWork;
        public AccountService(IRepositoryHelper repositoryHelper)
        {
            RepositoryHelper = repositoryHelper;
            UnitOfWork = RepositoryHelper.GetUnitOfWork();
        }

        public Account checkLogin(string phoneNumber, string password)
        {
            var repo = RepositoryHelper.GetRepository<IAccountRepository>(this.UnitOfWork);
            Account account = repo.GetByPhoneNumber(phoneNumber);
            if (account!=null && account.Password.Trim().Equals(password.Trim()))
            {
                return account;
            }
            return null;
        }

    }
}
