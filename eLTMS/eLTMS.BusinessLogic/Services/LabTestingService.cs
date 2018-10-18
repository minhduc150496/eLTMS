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
    public interface ILabTestingService
    {
        List<LabTesting> GetAll();
    }

    public class LabTestingService : ILabTestingService
    {
        private readonly IRepositoryHelper RepositoryHelper;
        private readonly IUnitOfWork UnitOfWork;

        public LabTestingService(IRepositoryHelper repositoryHelper)
        {
            RepositoryHelper = repositoryHelper;
            UnitOfWork = RepositoryHelper.GetUnitOfWork();
        }

        
        public List<LabTesting> GetAll()
        {
            var repo = this.RepositoryHelper.GetRepository<ILabTestingRepository>(UnitOfWork);
            var labTesting = repo.GetAllLabTesting();
            return labTesting;
        }
    }
}
