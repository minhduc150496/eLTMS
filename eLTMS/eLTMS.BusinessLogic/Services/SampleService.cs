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
    public interface ISampleService
    {
        List<Sample> GetAll();
    }
    public class SampleService: ISampleService
    {
        private readonly IRepositoryHelper RepositoryHelper;
        private readonly IUnitOfWork UnitOfWork;
        public SampleService(IRepositoryHelper repositoryHelper)
        {
            RepositoryHelper = repositoryHelper;
            UnitOfWork = RepositoryHelper.GetUnitOfWork();
        }

        public List<Sample> GetAll()
        {
            var repo = this.RepositoryHelper.GetRepository<ISampleRepository>(UnitOfWork);
            var sample = repo.GetAllSamples();
            return sample;
        }
        
    }
}
