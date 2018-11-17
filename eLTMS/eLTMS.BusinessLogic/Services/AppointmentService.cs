using AutoMapper;
using eLTMS.DataAccess.Infrastructure;
using eLTMS.DataAccess.Models;
using eLTMS.DataAccess.Repositories;
using eLTMS.Models.Models.dto;
using eLTMS.Models.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using eLTMS.Models.;

namespace eLTMS.BusinessLogic.Services
{
    public interface IAppointmentService
    {
        bool Create(AppointmentDto appointment);
        Appointment GetSingleById(int appointmentId); // DucBM
        List<AppointmentDto> GetAppointmentsByPatientId(int patientId); // DucBM
        List<Appointment> GetNewApp(int patientId);
        List<Appointment> GetOldApp(int patientId);
        List<Appointment> GetResult(int patientId);
        List<Appointment> GetResultDone(int patientId);
        List<Appointment> GetAppByPhone(string phone);
        List<Appointment> GetResultByAppCode(string appCode);
        bool UpdateAppointment(int appointmentId, List<SampleGettingDto> sgDtos);
        bool Update(string code, string con);
        bool DeleteAppointment(int appointmentId);
        List<Token> GetAllTokens();
    }
    public class AppointmentService : IAppointmentService
    {
        private readonly IRepositoryHelper RepositoryHelper;
        private readonly IUnitOfWork UnitOfWork;
        public AppointmentService(IRepositoryHelper repositoryHelper)
        {
            RepositoryHelper = repositoryHelper;
            UnitOfWork = RepositoryHelper.GetUnitOfWork();
        }

        public List<Appointment> GetAppByPhone(string phone)
        {
            var appRepo = this.RepositoryHelper.GetRepository<IAppointmentRepository>(this.UnitOfWork);
            var sampleRepo = this.RepositoryHelper.GetRepository<ISampleRepository>(this.UnitOfWork);
            var apps = appRepo.GetAppointmentByPhone(phone);
            return apps;
        }

        // DucBM
        public Appointment GetSingleById(int appointmentId)
        {
            var appRepo = this.RepositoryHelper.GetRepository<IAppointmentRepository>(this.UnitOfWork);
            var result = appRepo.GetAppointmentByIdInclude(appointmentId);
            return result;
        }

        // Author: DucBM
        public bool Create(AppointmentDto appointmentDto)
        {
            var appointmentRepo = this.RepositoryHelper.GetRepository<IAppointmentRepository>(this.UnitOfWork);
            try
            {
                var appointment = new Appointment();
                // Convert AppointmentDto to Appointment
                var now = DateTime.Now;
                var sDate = now.ToString("yyyy-MM-dd");
                var count = appointmentRepo.CountByDate(sDate);
                var code = sDate + "-" + count;
                appointment.AppointmentCode = code;
                appointment.Status = "NEW";
                appointment.PatientId = appointmentDto.PatientId;

                appointment.SampleGettings = new List<SampleGetting>();

                foreach(var sgDto in appointmentDto.SampleGettingDtos)
                {
                    var sg = Mapper.Map<SampleGettingDto, SampleGetting>(sgDto);
                    sg.TableId = null;
                    sg.LabTestings = new List<LabTesting>();
                    foreach (var id in sgDto.LabTestIds)
                    {
                        var labTesting = new LabTesting();
                        labTesting.LabTestId = id;
                        labTesting.SampleGettingId = sg.SampleGettingId;
                        labTesting.Status = "NEW";
                        sg.LabTestings.Add(labTesting);
                    }
                    appointment.SampleGettings.Add(sg);
                }
                // Create
                appointmentRepo.Create(appointment);
                var result = this.UnitOfWork.SaveChanges();
                if (result.Any())
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        // DucBM
        public List<AppointmentDto> GetAppointmentsByPatientId(int patientId)
        {
            var appRepo = this.RepositoryHelper.GetRepository<IAppointmentRepository>(this.UnitOfWork);
            var apps = appRepo.GetAppointmentsByPatientId(patientId);
            var appDtos = Mapper.Map<IEnumerable<Appointment>, IEnumerable<AppointmentDto>>(apps).ToList();
            return appDtos;
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

        public List<Appointment> GetResult(int patientId)
        {
            var appRepo = this.RepositoryHelper.GetRepository<IAppointmentRepository>(this.UnitOfWork);
            var sampleRepo = this.RepositoryHelper.GetRepository<ISampleRepository>(this.UnitOfWork);
            var apps = appRepo.GetResultByPatientId(patientId);
            return apps;
        }
        public List<Appointment> GetResultDone(int patientId)
        {
            var appRepo = this.RepositoryHelper.GetRepository<IAppointmentRepository>(this.UnitOfWork);
            var sampleRepo = this.RepositoryHelper.GetRepository<ISampleRepository>(this.UnitOfWork);
            var apps = appRepo.GetResultDoneByPatientId(patientId);
            return apps;
        }
        public  List<Appointment> GetResultByAppCode (string appCode)
        {
            var appRepo = this.RepositoryHelper.GetRepository<IAppointmentRepository>(this.UnitOfWork);
            var sampleRepo = this.RepositoryHelper.GetRepository<ISampleRepository>(this.UnitOfWork);
            var apps = appRepo.GetResultByAppCode(appCode);
            return apps;
        }

        // DucBM
        public bool UpdateAppointment(int appointmentId, List<SampleGettingDto> sampleGettingDtos)
        {
            try
            {
                var appRepo = this.RepositoryHelper.GetRepository<IAppointmentRepository>(this.UnitOfWork);
                // get existing appointment by AppointmentCode

                var appointment = appRepo.GetAppointmentByIdInclude(appointmentId);
                // delete old records
                // TEMPORARY !! -> too waist memory !!
                foreach (var sg in appointment.SampleGettings)
                {
                    sg.IsDeleted = true;
                    foreach(var lt in sg.LabTestings)
                    {
                        lt.IsDeleted = true;
                    }
                }

                // modify SampleGettings property
                appointment.SampleGettings = new List<SampleGetting>();
                foreach(var sgDto in sampleGettingDtos)
                {
                    var sg = Mapper.Map<SampleGettingDto, SampleGetting>(sgDto);
                    sg.SampleGettingCode = ""; // Need a Formula for this Code !!
                    sg.LabTestings = new List<LabTesting>();
                    sg.Status = "NEW";
                    sg.TableId = 1;
                    sg.IsDeleted = false;
                    foreach (var id in sgDto.LabTestIds)
                    {
                        var lt = new LabTesting();
                        lt.LabTestId = id;
                        sg.LabTestings.Add(lt);
                    }
                    appointment.SampleGettings.Add(sg);
                }
                
                // update entity
                appRepo.Update(appointment);
                // save to DB
                this.UnitOfWork.SaveChanges();

            } catch(Exception ex)
            {
                return false; 
            }
            return true;
        }

        public bool Update(string code,string con)
        {
            try
            {
                var appRepo = this.RepositoryHelper.GetRepository<IAppointmentRepository>(this.UnitOfWork);
                // get existing appointment by AppointmentCode
                var appointment = appRepo.GetAppointmentByCode(code);
                // modify SampleGettings property               
                appointment.Conclusion = con;
                appointment.Status = "DOCTORDONE";
                // update entity
                appRepo.Update(appointment);
                // save to DB
                this.UnitOfWork.SaveChanges();
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        // DucBM
        public bool DeleteAppointment(int appointmentId)
        {
            try
            {
                var appRepo = this.RepositoryHelper.GetRepository<IAppointmentRepository>(this.UnitOfWork);
                // get existing appointment by AppointmentCode
                var appointment = appRepo.GetAppointmentById(appointmentId);
                if (appointment==null)
                {
                    return false;
                }
                // assign IsDeleted = true
                appointment.IsDeleted = true;
                // update entity
                appRepo.Update(appointment);
                // save to DB
                this.UnitOfWork.SaveChanges();
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        // DucBM
        public List<Token> GetAllTokens()
        {
            var repo = this.RepositoryHelper.GetRepository<ITokenRepository>(UnitOfWork);
            var tokens = repo.GetAll();
            return tokens;
        }
    }
}
