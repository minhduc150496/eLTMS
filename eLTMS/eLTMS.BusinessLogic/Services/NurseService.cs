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
    public interface INurseService
    {
        bool ChangeIsGot(int sampleGettingId);
        List<SampleGettingForNurseBySampleDto> GetAllBySample(DateTime date, int sampleId);
    }
    class NurseService : INurseService
    {
        private readonly IRepositoryHelper RepositoryHelper;
        private readonly IUnitOfWork UnitOfWork;
        public NurseService(IRepositoryHelper repositoryHelper)
        {
            RepositoryHelper = repositoryHelper;
            UnitOfWork = RepositoryHelper.GetUnitOfWork();
        }

        public bool ChangeIsGot(int sampleGettingId)
        {
            try
            {
                var sgRepo = RepositoryHelper.GetRepository<ISampleGettingRepository>(UnitOfWork);
                var labRepo = RepositoryHelper.GetRepository<ILabTestingRepository>(UnitOfWork);
                var sampleGetting = sgRepo.GetFirst(p => p.SampleGettingId == sampleGettingId);

                sampleGetting.IsGot = true;
                sampleGetting.Status = "NURSEDONE";
                sgRepo.Update(sampleGetting);

                var labs = labRepo.GetAll().Where(x => x.SampleGettingId == sampleGettingId);
                foreach(var lab in labs)
                {
                    lab.Status = "WAITING";
                    labRepo.Update(lab);
                }

                UnitOfWork.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }

        }

        public List<SampleGettingForNurseBySampleDto> GetAllBySample(DateTime date, int sampleId)
        {
            var appRepo = RepositoryHelper.GetRepository<IAppointmentRepository>(UnitOfWork);
            var paRepo = RepositoryHelper.GetRepository<IPatientRepository>(UnitOfWork);
            var sgRepo = RepositoryHelper.GetRepository<ISampleGettingRepository>(UnitOfWork);
            var slotRepo = RepositoryHelper.GetRepository<ISlotRepository>(UnitOfWork);
            var spRepo = this.RepositoryHelper.GetRepository<ISampleRepository>(this.UnitOfWork);
            var tableRepo = this.RepositoryHelper.GetRepository<ITableRepository>(this.UnitOfWork);

            var apps = appRepo.GetAll().Where(p => p.IsDeleted != true);
            var pas = paRepo.GetAll().Where(p => p.IsDeleted != true);
            var sgs = sgRepo.GetAll().Where(p => p.SampleId == sampleId && p.IsDeleted != true && p.GettingDate == date && p.IsPaid == true);
            var sps = spRepo.GetAll().Where(p => p.IsDeleted != true);
            var slots = slotRepo.GetAll();

            //app + patient (1)
            var appPas = apps.Join(pas, p => p.PatientId, c => c.PatientId, (p, c) => new
            {
                app = p,
                pa = c
            });

            //sample + sampleGetting (2)
            var spSgs = sgs.Join(sps, p => p.SampleId, c => c.SampleId, (p, c) => new
            {
                sg = p,
                sp = c
            });

            //(2) + slot
            var spSgSlots = spSgs.Join(slots, p => p.sg.SlotId, c => c.SlotId, (p, c) => new
            {
                spSg = p,
                slot = c
            });

            var result = spSgSlots.Join(appPas, p => p.spSg.sg.AppointmentId,
                c => c.app.AppointmentId, (p, c) => new SampleGettingForNurseBySampleDto
                {
                    StartTime = TimeSpan.FromSeconds(p.slot.StartTime.Value).ToString(@"hh\:mm"),
                    SampleGettingId = p.spSg.sg.SampleGettingId,
                    SampleName = p.spSg.sp.SampleName,
                    PatientName = c.pa.FullName,
                    Date = p.spSg.sg.GettingDate.Value.ToShortDateString(),
                    IsGot = p.spSg.sg.IsGot

                }).ToList();
            return result;
        }

        
    }
}
