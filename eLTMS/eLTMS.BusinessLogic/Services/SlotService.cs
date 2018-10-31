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
    public interface ISlotService
    {
        List<Slot> GetByDateAndSampleId(DateTime date, int sampleId);
        List<Slot> GetAvailableSlots();
    }
    public class SlotService : ISlotService
    {
        private readonly IRepositoryHelper RepositoryHelper;
        private readonly IUnitOfWork UnitOfWork;
        public SlotService(IRepositoryHelper repositoryHelper)
        {
            RepositoryHelper = repositoryHelper;
            UnitOfWork = RepositoryHelper.GetUnitOfWork();
        }

        public List<Slot> GetByDateAndSampleId(DateTime date, int sampleId)
        {
            var sampleRepo = this.RepositoryHelper.GetRepository<ISampleRepository>(UnitOfWork);
            var sample = sampleRepo.GetById(sampleId);

            if (sample.SampleGroupId == null)
            {
                return null;
            }

            var sampleGroupId = (int)sample.SampleGroupId;
            var slotRepo = this.RepositoryHelper.GetRepository<ISlotRepository>(UnitOfWork);
            var slots = slotRepo.GetByDateAndSampleGroupId(date, sampleGroupId);

            return slots;
        }

        public List<Slot> GetAvailableSlots()
        {
            var slotRepo = this.RepositoryHelper.GetRepository<ISlotRepository>(UnitOfWork);
            var slots = slotRepo.GetAvailableSlots();
            return slots;
        }

    }
}
