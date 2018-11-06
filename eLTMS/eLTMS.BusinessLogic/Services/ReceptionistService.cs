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
    public interface IReceptionistService
    {
        List<Appointment> GetNewApp(int patientId);
        List<Appointment> GetOldApp(int patientId);
        bool Add(AppointmentAddDto data);
        List<Appointment> GetAllAppointment();
        List<Appointment> GetAppBySample(int sampleId);
        List<Appointment> OrderBySlot(List<Appointment> apps);
    }
    class ReceptionistService : IReceptionistService
    {
        private readonly IRepositoryHelper RepositoryHelper;
        private readonly IUnitOfWork UnitOfWork;
        public ReceptionistService(IRepositoryHelper repositoryHelper)
        {
            RepositoryHelper = repositoryHelper;
            UnitOfWork = RepositoryHelper.GetUnitOfWork();
        }
        public string CreateAppReturnCode(Appointment appointment)
        {
            var appointmentRepo = this.RepositoryHelper.GetRepository<IAppointmentRepository>(this.UnitOfWork);
            try
            {
                // Convert AppointmentDto to Appointment
                var now = DateTime.Now;
                var sDate = now.ToString("yyyy-MM-dd");
                var count = appointmentRepo.CountByDate(sDate);
                var code = sDate + "-" + count;
                appointment.AppointmentCode = code;
                appointment.Status = "NEW";
                // Create
                appointmentRepo.Create(appointment);
                var result = this.UnitOfWork.SaveChanges();
                if (result.Any())
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
            return appointment.AppointmentCode;
        }

        //public DateTime 
        public Slot getFirstEmptySlot (int sampleId)
        {
            var paRepo= RepositoryHelper.GetRepository<IPatientRepository>(UnitOfWork);
            var appRepo = RepositoryHelper.GetRepository<IAppointmentRepository>(UnitOfWork);
            var sgRepo = RepositoryHelper.GetRepository<ISampleGettingRepository>(UnitOfWork);

            var pas = paRepo.GetAll().Where(p => p.IsOnline == false);
            var apps = new List<Appointment>();
            foreach (var pa in pas)
            {
                var appps=(appRepo.GetAll().Where(p => p.PatientId==pa.PatientId));
                foreach(var appp in appps)
                {
                    apps.Add(appp);
                }
            }
            List<Slot> slots = new List<Slot>();
            foreach(var app in apps)
            {
                var sgs = sgRepo.GetAll2().ToList().Where(p => p.AppointmentId == app.AppointmentId && p.SampleId == sampleId);
                if (sgs != null) foreach (var sg in sgs) {
                        if(sg.Slot!=null)
                        slots.Add(sg.Slot);
                    }
            }
            slots.OrderByDescending(p => p.StartTime);
            if (slots.Count <= 0) return null;
            else return slots.FirstOrDefault();
        }
        
        public bool Add(AppointmentAddDto data)
        {
            var appRepo = RepositoryHelper.GetRepository<IAppointmentRepository>(UnitOfWork);
            var accRepo = RepositoryHelper.GetRepository<IAccountRepository>(UnitOfWork);
            var paRepo = RepositoryHelper.GetRepository<IPatientRepository>(UnitOfWork);
            var sgRepo = RepositoryHelper.GetRepository<ISampleGettingRepository>(UnitOfWork);
            var slotRepo = RepositoryHelper.GetRepository<ISlotRepository>(UnitOfWork);
            try
            {
                //tao account
                accRepo.Create(new Account
                {
                    PhoneNumber = data.Phone,

                    //passWord default: qwe123
                    Password = "qwe123",
                    IsDeleted = false
                });
                UnitOfWork.SaveChanges();
                //tao benh nhan
                var accId = accRepo.GetByPhoneNumber(data.Phone).AccountId;
                paRepo.Create(new Patient
                {
                    AccountId= accId,
                    FullName = data.Name,
                    PhoneNumber = data.Phone,
                    IsDeleted = false
                });
                UnitOfWork.SaveChanges();
                //tao cuoc hen
                var paId = paRepo.GetFirst(p=>p.PhoneNumber==data.Phone).PatientId;
                var appCode= CreateAppReturnCode(new Appointment
                {
                    PatientId = paId,
                    IsDeleted = false
                });
                UnitOfWork.SaveChanges();
                //xep lich hen cho tung loai xet nghiem
                var appId = appRepo.GetFirst(p=>p.AppointmentCode==appCode).AppointmentId;
                if (data.Mau == true)
                {
                    //tim slot trong
                    var firstSlot = getFirstEmptySlot(1);
                    var allSlot = slotRepo.GetAll().ToList();
                    var emtySlot = allSlot.FirstOrDefault();
                    if (firstSlot != null) emtySlot = firstSlot;
                    //tao lich hen loai xet nghiem mau 
                    sgRepo.Create(new SampleGetting
                    {
                        AppointmentId = appId,
                        SampleId = 1,
                        SlotId = emtySlot.SlotId,
                        IsDeleted = false
                    });
                }
                if (data.NuocTieu == true)
                {
                    var firstSlot = getFirstEmptySlot(2);
                    sgRepo.Create(new SampleGetting
                    {
                        AppointmentId = appId,
                        SampleId = 2,
                        IsDeleted = false
                    });
                }
                if (data.TeBaoHoc == true)
                {
                    var firstSlot = getFirstEmptySlot(3);
                    sgRepo.Create(new SampleGetting
                    {
                        AppointmentId = appId,
                        SampleId = 3,
                        IsDeleted = false
                    });
                }
                if (data.Phan == true)
                {
                    var firstSlot = getFirstEmptySlot(4);
                    sgRepo.Create(new SampleGetting
                    {
                        AppointmentId = appId,
                        SampleId = 4,
                        IsDeleted = false
                    });
                }
                if (data.Dich == true)
                {
                    var firstSlot = getFirstEmptySlot(5);
                    sgRepo.Create(new SampleGetting
                    {
                        AppointmentId = appId,
                        SampleId = 5,
                        IsDeleted = false
                    });
                }

                UnitOfWork.SaveChanges();
            }
            catch (Exception e) { return false; }
            return true;
        }
        public List<Appointment> GetNewApp(int patientId)
        {
            var appRepo = this.RepositoryHelper.GetRepository<IAppointmentRepository>(this.UnitOfWork);
            var apps = appRepo.GetNewAppByPatientId(patientId);
            return apps;
        }

        public List<Appointment> GetOldApp(int patientId)
        {
            var appRepo = this.RepositoryHelper.GetRepository<IAppointmentRepository>(this.UnitOfWork);
            var sampleRepo = this.RepositoryHelper.GetRepository<ISampleRepository>(this.UnitOfWork);
            var apps = appRepo.GetOldAppByPatientId(patientId);
            return apps;
        }

        public List<Appointment> GetAllAppointment()
        {
            var appRepo = this.RepositoryHelper.GetRepository<IAppointmentRepository>(this.UnitOfWork);
            var apps = appRepo.GetAllApp();
            //var sampleGettingRepo = this.RepositoryHelper.GetRepository<ISampleGettingRepository>(this.UnitOfWork);
            //foreach(var app in apps)
            //{
            //    app.SampleGettings= sampleGettingRepo.GetAll().Where(p=>p.Appointment.ap)
            //}
            return apps;
        }

        public List<Appointment> GetAppBySample(int sampleId)
        {
            var appRepo = this.RepositoryHelper.GetRepository<IAppointmentRepository>(this.UnitOfWork);
            var sgRepo = this.RepositoryHelper.GetRepository<ISampleGettingRepository>(this.UnitOfWork);
            var apps = sgRepo.GetAllIncludeApp().Where(x => x.SampleId == sampleId).Select(p=>p.Appointment).ToList();
            var result = new List<Appointment>();
            foreach(var app in apps)
            {
                var appResult = appRepo.GetAppById(app.AppointmentId);
                if (appResult != null) result.Add(appResult);
                foreach(var sg in app.SampleGettings.ToList())
                {
                    if (sg.SampleId != sampleId) app.SampleGettings.Remove(sg);
                }
            }
            return result;
        }
        public List<Appointment> OrderBySlot(List<Appointment> apps)
        {
            //foreach(var app in apps)
            //{
            //    app.
            //}
            return apps;
        }
       
    }
}
