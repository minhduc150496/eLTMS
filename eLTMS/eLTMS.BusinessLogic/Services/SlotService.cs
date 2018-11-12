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
        bool CreateNewSlotsForAMonth(int year, int month);
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

        /*
         * Create new Slots for a month
         * Author: DucBM
         */
        public bool CreateNewSlotsForAMonth(int year, int month)
        {
            var sgRepo = this.RepositoryHelper.GetRepository<ISampleGroupRepository>(UnitOfWork);
            var slotRepo = this.RepositoryHelper.GetRepository<ISlotRepository>(UnitOfWork);
            var sampleGroups = sgRepo.GetAll().Where(x => x.IsDeleted == false);
            // for days in month
            var nDays = DateTime.DaysInMonth(year, month);
            for (var day = 6; day <= 15; day++)
            {
                var date = new DateTime(year, month, day);
                // for sample groups
                foreach (var sampleGroup in sampleGroups)
                {
                    var duration = sampleGroup.GettingDuration;
                    // for slot time from OpenTime to CloseTime
                    if (sampleGroup.OpenTime >= sampleGroup.CloseTime)
                    {
                        continue;
                    }
                    for (var startTime = sampleGroup.OpenTime;
                        startTime + duration <= sampleGroup.CloseTime;
                        startTime += 2 * duration)
                    {
                        var slot = new Slot();
                        slot.SampleGroupId = sampleGroup.SampleGroupId;
                        slot.Quantity = sampleGroup.NumberOfSlots;
                        slot.StartTime = startTime;
                        slot.FinishTime = startTime + duration;
                        slot.Date = date;
                        slotRepo.Create(slot);
                    }
                }
            }
            UnitOfWork.SaveChanges();
            return true;
        }

    }
}
