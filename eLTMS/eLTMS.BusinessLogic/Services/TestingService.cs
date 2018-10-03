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

    public interface ITestingService
    {
        List<Testing> GetAll();
        Testing GetById(int id);
        bool Create(Testing newTesting);
        bool CreateMany(List<Testing> newTestings);
        bool Update(Testing testing);
        bool Delete(int id);
    }

    public class TestingService : ITestingService
    {
        private readonly IRepositoryHelper RepositoryHelper;
        private readonly IUnitOfWork UnitOfWork;
        public TestingService(IRepositoryHelper repositoryHelper)
        {
            RepositoryHelper = repositoryHelper;
            UnitOfWork = RepositoryHelper.GetUnitOfWork();
        }

        public List<Testing> GetAll()
        {
            var repo = this.RepositoryHelper.GetRepository<ITestingRepository>(this.UnitOfWork);
            var testings = repo.GetAllTesting();
            return testings;
        }

        public Testing GetById(int id)
        {
            var repo = this.RepositoryHelper.GetRepository<ITestingRepository>(this.UnitOfWork);
            var testing = repo.GetById(id);
            return testing;
        }

        public bool Create(Testing newTesting)
        {
            var repo = this.RepositoryHelper.GetRepository<ITestingRepository>(this.UnitOfWork);
            try
            {
                repo.Create(newTesting);
                this.UnitOfWork.SaveChanges();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public bool CreateMany(List<Testing> newTestings)
        {
            var repo = this.RepositoryHelper.GetRepository<ITestingRepository>(this.UnitOfWork);
            try
            {
                foreach(var item in newTestings)
                {
                    repo.Create(item);
                }
                this.UnitOfWork.SaveChanges();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public bool Update(Testing testing)
        {
            var repo = this.RepositoryHelper.GetRepository<ITestingRepository>(this.UnitOfWork);
            try
            {
                repo.Update(testing);
                this.UnitOfWork.SaveChanges();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public bool Delete(int id)
        {
            var repo = this.RepositoryHelper.GetRepository<ITestingRepository>(this.UnitOfWork);
            try
            {
                repo.Delete(id);
                this.UnitOfWork.SaveChanges();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }


    }
}
