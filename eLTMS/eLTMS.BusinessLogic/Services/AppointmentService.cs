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
    public interface IAppointmentService
    {
        //bool Create(Appointment appointment);
        List<Appointment> GetNewApp(int patientId);
    }
    public class AppointmentService: IAppointmentService
    {
        private readonly IRepositoryHelper RepositoryHelper;
        private readonly IUnitOfWork UnitOfWork;
        public AppointmentService(IRepositoryHelper repositoryHelper)
        {
            RepositoryHelper = repositoryHelper;
            UnitOfWork = RepositoryHelper.GetUnitOfWork();
        }

        //public bool Create(Appointment appointment)
        //{
        //    var appointmentRepo = this.RepositoryHelper.GetRepository<IAppointmentRepository>(this.UnitOfWork);
        //    //var sampleGettingRepo = this.RepositoryHelper.GetRepository<ISampleGettingRepository>(this.UnitOfWork);
        //    //var labTestingRepo = this.RepositoryHelper.GetRepository<ILabTestingRepository>(this.UnitOfWork);
        //    try
        //    {
        //        //appointment.LabTestings = labTestings;
        //        //appointment.SampleGettings = sampleGettings;
        //        appointmentRepo.Create(appointment);
        //        var result = this.UnitOfWork.SaveChanges();
        //        if (result.Any())
        //        {
        //            return false;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return false;
        //    }
        //    return true;
        //}

        public List<Appointment> GetNewApp(int patientId)
        {
            var appRepo = this.RepositoryHelper.GetRepository<IAppointmentRepository>(this.UnitOfWork);
            var sampleRepo = this.RepositoryHelper.GetRepository<ISampleRepository>(this.UnitOfWork);
            var apps = appRepo.GetNewAppByPatientId(patientId);
            
            return apps;
        }
    }
}
