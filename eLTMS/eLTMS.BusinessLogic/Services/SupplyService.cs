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
    public interface ISupplyService
    {
        List<Supply> GetAllSupplies();
        bool AddSupply(Supply supply);
    }

    public class SupplyService : ISupplyService
    {
        private readonly IRepositoryHelper RepositoryHelper;
        private readonly IUnitOfWork UnitOfWork;
        public SupplyService(IRepositoryHelper repositoryHelper)
        {
            RepositoryHelper = repositoryHelper;
            UnitOfWork = RepositoryHelper.GetUnitOfWork();
        }

        public List<Supply> GetAllSupplies()
        {
            var supplyRepo = this.RepositoryHelper.GetRepository<ISupplyRepository>(UnitOfWork);
            var supplies =  supplyRepo.GetAllSupply();
            return supplies;
        }
        public bool AddSupply(Supply supply)
        {

            var repo = RepositoryHelper.GetRepository<ISupplyRepository>(UnitOfWork);
            //repo.Create(supply);
            //var saveResult =  UnitOfWork.SaveChanges();
            try
            {
                repo.Create(supply);
                UnitOfWork.SaveChanges();
            }
            catch (Exception) { return false; }
            return true;
        }
    }
}
