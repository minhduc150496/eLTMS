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
    public interface ILabTestingIndexService
    {
        List<LabTestingIndex> GetAllLabTestingIndex();
        List<LabTestingIndex> GetAllLabTestingIndexHaveLabtestingId(int labtestingId);
        bool AddLabTestingIndex(List<LabTestingIndex> labTestingIndex);
    }

    public class LabTestingIndexService : ILabTestingIndexService
    {
        private readonly IRepositoryHelper RepositoryHelper;
        private readonly IUnitOfWork UnitOfWork;

        public LabTestingIndexService(IRepositoryHelper repositoryHelper)
        {
            RepositoryHelper = repositoryHelper;
            UnitOfWork = RepositoryHelper.GetUnitOfWork();
        }
        public List<LabTestingIndex> GetAllLabTestingIndexHaveLabtestingId(int labtestingId)
        {
            var repo = this.RepositoryHelper.GetRepository<ILabTestingIndexRepository>(UnitOfWork);
            var labTesting = repo.GetAllLabTestingIndexById(labtestingId);
            return labTesting;
        }
        public bool AddLabTestingIndex(List<LabTestingIndex> labTestingIndex)
        {

            var repo = RepositoryHelper.GetRepository<ILabTestingIndexRepository>(UnitOfWork);
            try
            {
                foreach (var item in labTestingIndex)
                {
                    repo.Create(item);
                }
                var result = UnitOfWork.SaveChanges();
                if (result.Any())
                {
                    return false;
                }

            }
            catch (Exception ex) { return false; }
            return true;
        }

        public List<LabTestingIndex> GetAllLabTestingIndex()
        {
            var repo = this.RepositoryHelper.GetRepository<ILabTestingIndexRepository>(UnitOfWork);
            var labTesting = repo.GetAllLabTestingIndex();
            return labTesting;
        }
       
    }
}
