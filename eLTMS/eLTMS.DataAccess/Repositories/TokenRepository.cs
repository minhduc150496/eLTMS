using eLTMS.DataAccess.Infrastructure;
using eLTMS.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Entity;

namespace eLTMS.DataAccess.Repositories
{

    public interface ITokenRepository : IRepository<Token>
    {
        List<Token> GetAll();
    }
    public class TokenRepository : RepositoryBase<Token>, ITokenRepository
    {
        public List<Token> GetAll()
        {
            List<Token> result = DbSet.AsQueryable().Where(x=>x.IsDeleted==false).ToList();
            return result;
        }
    }
}