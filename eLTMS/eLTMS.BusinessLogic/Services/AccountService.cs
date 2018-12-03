using AutoMapper;
using eLTMS.DataAccess.Infrastructure;
using eLTMS.DataAccess.Models;
using eLTMS.DataAccess.Repositories;
using eLTMS.Models.Models.dto;
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
        ResponseObjectDto CheckLoginPatient(string phoneNumber, string password);
        Account GetById(int accountId);
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
            Account account = repo.GetByPhoneNumberIncludeRoleEmployeePatient(phoneNumber);
            if (account!=null && account.Password.Trim().Equals(password.Trim()))
            {
                return account;
            }
            return null;
        }

        public ResponseObjectDto CheckLoginPatient(string phoneNumber, string password)
        {
            var repo = RepositoryHelper.GetRepository<IAccountRepository>(this.UnitOfWork);
            Account account = repo.GetAccountPatientForLoginMobile(phoneNumber, password);
            var accountDto = Mapper.Map<Account, AccountDto>(account);
            var respObj = new ResponseObjectDto();
            if (account != null)
            {
                respObj.Success = true;
                respObj.Message = "Đăng nhập thành công";
                respObj.Data = accountDto;
            } else
            {
                respObj.Success = true;
                respObj.Message = "Sai số điện thoại hoặc mật khẩu";
                respObj.Data = null;
            }
            return respObj;
        }

        public Account GetById(int accountId)
        {
            var repo = RepositoryHelper.GetRepository<IAccountRepository>(this.UnitOfWork);
            Account account = repo.GetById(accountId);
            return account;
        }
    }
}
