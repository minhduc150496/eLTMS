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
    public interface ILabTestService
    {
        List<LabTest> GetAll();
    }
    public class LabTestService : ILabTestService
    {
        private readonly IRepositoryHelper RepositoryHelper;
        private readonly IUnitOfWork UnitOfWork;
        public LabTestService(IRepositoryHelper repositoryHelper)
        {
            RepositoryHelper = repositoryHelper;
            UnitOfWork = RepositoryHelper.GetUnitOfWork();
        }

        public List<LabTest> GetAll()
        {
            var repo = RepositoryHelper.GetRepository<ILabTestRepository>(this.UnitOfWork);
            var results = repo.GetAll();
            return results;
        }
    }
}
