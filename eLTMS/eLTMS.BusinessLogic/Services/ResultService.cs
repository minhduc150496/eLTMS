using eLTMS.DataAccess.Infrastructure;
using eLTMS.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLTMS.BusinessLogic.Services
{
    public interface IResultService
    {
        List<ResultPaper> GetMyNewResult();
        List<ResultPaper> GetResultsByPatientId(int id);
    }
    public class ResultService : IResultService
    {
        private readonly IRepositoryHelper RepositoryHelper;
        private readonly IUnitOfWork UnitOfWork;
        public ResultService(IRepositoryHelper repositoryHelper)
        {
            this.RepositoryHelper = repositoryHelper;
            this.UnitOfWork = RepositoryHelper.GetUnitOfWork();
        }

        public List<ResultPaper> GetMyNewResult()
        {
            throw new NotImplementedException();
        }

        public List<ResultPaper> GetResultsByPatientId(int id)
        {
            //var repo = this.RepositoryHelper.GetRepository<IResultPaperRepository>
            return null;
        }
    }
}
