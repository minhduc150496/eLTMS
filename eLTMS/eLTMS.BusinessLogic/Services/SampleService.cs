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
    public interface ISampleService
    {
        List<Sample> GetAll();
        //bool Create(LabTest newTesting);
        //bool CreateMany(List<LabTest> newTestings);
        //bool Update(LabTest testing);
        SampleGettingCalendar GetAvailableSlots();
        bool Delete(int id);
        bool AddSample(Sample sample);
        Sample GetSampleById(int id);
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

        public SampleGettingCalendar GetAvailableSlots()
        {
            throw new NotImplementedException();
        }

        public List<Sample> GetAll()
        {
            var repo = this.RepositoryHelper.GetRepository<ISampleRepository>(UnitOfWork);
            var sample = repo.GetAllSamples();
            return sample;
        }

        public Sample GetSampleById(int id)
        {
            var repo = this.RepositoryHelper.GetRepository<ISampleRepository>(UnitOfWork);
            var sample = repo.GetSampleById(id);
            return sample;

        }
        public bool Delete(int id)
        {
            var repo = RepositoryHelper.GetRepository<ISampleRepository>(UnitOfWork);

            try
            {
                var sample = repo.GetById(id);
                sample.IsDeleted = true;
                repo.Update(sample);
                UnitOfWork.SaveChanges();
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
        public bool AddSample(Sample sample)
        {

            var repo = RepositoryHelper.GetRepository<ISampleRepository>(UnitOfWork);
            try
            {
                repo.Create(sample);
                UnitOfWork.SaveChanges();
            }
            catch (Exception) { return false; }
            return true;
        }
    }
}
