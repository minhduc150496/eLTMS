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
    public interface ITokenService
    {
        List<Token> GetAll();
    }
    public class TokenService : ITokenService
    {
        private readonly IRepositoryHelper RepositoryHelper;
        private readonly IUnitOfWork UnitOfWork;
        public TokenService(IRepositoryHelper repositoryHelper)
        {
            RepositoryHelper = repositoryHelper;
            UnitOfWork = RepositoryHelper.GetUnitOfWork();
        }
        public List<Token> GetAll()
        {
            var repo = this.RepositoryHelper.GetRepository<ITokenRepository>(UnitOfWork);
            var tokens = repo.GetAll();
            return tokens;
        }
    }
}
