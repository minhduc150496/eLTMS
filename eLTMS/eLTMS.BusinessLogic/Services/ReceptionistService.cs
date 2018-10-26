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
        public bool Add(AppointmentAddDto data)
        {

            var appRepo = RepositoryHelper.GetRepository<IAppointmentRepository>(UnitOfWork);
            var accRepo = RepositoryHelper.GetRepository<IAccountRepository>(UnitOfWork);
            var paRepo = RepositoryHelper.GetRepository<IPatientRepository>(UnitOfWork);
            var sgRepo = RepositoryHelper.GetRepository<ISampleGettingRepository>(UnitOfWork);
            try
            {
                accRepo.Create(new Account
                {
                    PhoneNumber = data.Phone,

                    //passWord default: qwe123
                    Password = "qwe123",
                    IsDeleted = false
                });
                UnitOfWork.SaveChanges();
                var accId = accRepo.GetByPhoneNumber(data.Phone).AccountId;
                paRepo.Create(new Patient
                {
                    AccountId= accId,
                    FullName = data.Name,
                    PhoneNumber = data.Phone,
                    IsDeleted = false
                });
                UnitOfWork.SaveChanges();
                var paId = paRepo.GetFirst(p=>p.PhoneNumber==data.Phone).PatientId;
                appRepo.Create(new Appointment
                {
                    AppointmentCode = data.AppCode,
                    PatientId=paId,
                    IsDeleted = false
                });
                UnitOfWork.SaveChanges();
                var appId = appRepo.GetAppointmentByCode(data.AppCode).AppointmentId;
                if (data.Mau == true)
                {
                    sgRepo.Create(new SampleGetting
                    {
                        AppointmentId = appId,
                        SampleId= 1,
                        IsDeleted = false
                    });
                }
                if (data.NuocTieu == true)
                {
                    sgRepo.Create(new SampleGetting
                    {
                        AppointmentId = appId,
                        SampleId = 2,
                        IsDeleted = false
                    });
                }
                if (data.TeBaoHoc == true)
                {
                    sgRepo.Create(new SampleGetting
                    {
                        AppointmentId = appId,
                        SampleId = 3,
                        IsDeleted = false
                    });
                }
                if (data.Phan == true)
                {
                    sgRepo.Create(new SampleGetting
                    {
                        AppointmentId = appId,
                        SampleId = 4,
                        IsDeleted = false
                    });
                }
                if (data.Dich == true)
                {
                    sgRepo.Create(new SampleGetting
                    {
                        AppointmentId = appId,
                        SampleId = 5,
                        IsDeleted = false
                    });
                }

                UnitOfWork.SaveChanges();
            }
            catch (Exception) { return false; }
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
    }
}
