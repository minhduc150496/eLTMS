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
        ResponseObjectDto Create(AppointmentDto appointment);
        Appointment GetSingleByCode(string code);
        Appointment GetSingleById(int appointmentId); // Author: DucBM
        Appointment GetResultDoneByAppointmentId(int appointmentId); // Author: DucBM
        List<AppointmentDto> GetAppointmentsByPatientId(int patientId); // Author: DucBM
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

      
        public Appointment GetSingleById(int appointmentId)
        {
            var appRepo = this.RepositoryHelper.GetRepository<IAppointmentRepository>(this.UnitOfWork);
            var result = appRepo.GetAppointmentByIdInclude(appointmentId);
            return result;
        }
        public Appointment GetSingleByCode(string code)
        {
            var appRepo = this.RepositoryHelper.GetRepository<IAppointmentRepository>(this.UnitOfWork);
            var result = appRepo.GetAppointmentByCode(code);
            return result;
        }
      
        public Appointment GetResultDoneByAppointmentId(int appointmentId)
        {
            var appRepo = this.RepositoryHelper.GetRepository<IAppointmentRepository>(this.UnitOfWork);
            var result = appRepo.GetResultDoneByAppointmentId(appointmentId);
            return result;
        }

      
        public ResponseObjectDto Create(AppointmentDto appointmentDto)
        {
            var appointmentRepo = this.RepositoryHelper.GetRepository<IAppointmentRepository>(this.UnitOfWork);
            var tableRepo = this.RepositoryHelper.GetRepository<ITableRepository>(this.UnitOfWork);
            var responseObject = new ResponseObjectDto();
            responseObject.Success = true;
            responseObject.Message = "Đặt lịch thành công";

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

            var sampleGettingDtos = appointmentDto.SampleGettingDtos;
            foreach (var sgDto in appointmentDto.SampleGettingDtos)
            {
                var sg = Mapper.Map<SampleGettingDto, SampleGetting>(sgDto);
                var avaiTable = tableRepo.GetFirstAvailableTable((int)sg.SlotId, (DateTime)sg.GettingDate);
                if (avaiTable==null)
                {
                    responseObject.Success = false;
                    responseObject.Message = "Có ca xét nghiệm đã hết chỗ";
                    break;
                } 
                sg.TableId = avaiTable.TableId; 
                sg.Status = "NEW";
                sg.LabTestings = new List<LabTesting>();
                foreach (var id in sgDto.LabTestIds)
                {
                    var labTesting = new LabTesting();
                    labTesting.LabTestId = id;
                    labTesting.SampleGettingId = sg.SampleGettingId;
                    labTesting.Status = "NEW";
                    sg.LabTestings.Add(labTesting);
                }
                if (responseObject.Success==false)
                {
                    break;
                }
                appointment.SampleGettings.Add(sg);
            }
            // Create
            appointmentRepo.Create(appointment);
            try
            {
                var result = this.UnitOfWork.SaveChanges();
                if (result.Any())
                {
                    responseObject.Success = false;
                    responseObject.Message = "Có lỗi xảy ra";
                }
            } catch(Exception ex)
            {
                responseObject.Success = false;
                responseObject.Message = "Có lỗi xảy ra";
            }
            return responseObject;
        }

      
        public List<AppointmentDto> GetAppointmentsByPatientId(int patientId)
        {
            var appRepo = this.RepositoryHelper.GetRepository<IAppointmentRepository>(this.UnitOfWork);
            var apps = appRepo.GetAppointmentsByPatientId(patientId);
            var appDtos = Mapper.Map<IEnumerable<Appointment>, IEnumerable<AppointmentDto>>(apps).ToList();
            return appDtos;
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
        public List<Appointment> GetResultByAppCode(string appCode)
        {
            var appRepo = this.RepositoryHelper.GetRepository<IAppointmentRepository>(this.UnitOfWork);
            var sampleRepo = this.RepositoryHelper.GetRepository<ISampleRepository>(this.UnitOfWork);
            var apps = appRepo.GetResultByAppCode(appCode);
            return apps;
        }

      
        public bool UpdateAppointment(int appointmentId, List<SampleGettingDto> sampleGettingDtos)
        {
            var appRepo = this.RepositoryHelper.GetRepository<IAppointmentRepository>(this.UnitOfWork);
            // get existing appointment by AppointmentCode

            var appointment = appRepo.GetAppointmentByIdInclude(appointmentId);
            // delete old records
            // TEMPORARY !! -> too waist memory !!
            foreach (var sg in appointment.SampleGettings)
            {
                sg.IsDeleted = true;
                foreach (var lt in sg.LabTestings)
                {
                    lt.IsDeleted = true;
                }
            }

            // modify SampleGettings property
            appointment.SampleGettings = new List<SampleGetting>();
            foreach (var sgDto in sampleGettingDtos)
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

            return true;
        }

        public bool Update(string code, string con)
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

      
        public bool DeleteAppointment(int appointmentId)
        {
            var appRepo = this.RepositoryHelper.GetRepository<IAppointmentRepository>(this.UnitOfWork);
            // get existing appointment by AppointmentCode
            var appointment = appRepo.GetAppointmentById(appointmentId);
            if (appointment == null)
            {
                return false;
            }
            // assign IsDeleted = true
            appointment.IsDeleted = true;
            // update entity
            appRepo.Update(appointment);
            // save to DB
            this.UnitOfWork.SaveChanges();
            return true;
        }

      
        public List<Token> GetAllTokens()
        {
            var repo = this.RepositoryHelper.GetRepository<ITokenRepository>(UnitOfWork);
            var tokens = repo.GetAll();
            return tokens;
        }
    }
}
