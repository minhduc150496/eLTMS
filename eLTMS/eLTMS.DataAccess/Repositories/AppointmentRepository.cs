using eLTMS.DataAccess.Infrastructure;
using eLTMS.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Entity;


namespace eLTMS.DataAccess.Repositories
{
    public interface IAppointmentRepository : IRepository<Appointment>
    {
        List<Appointment> GetAppointmentsByPatientId(int patientId); // DucBM
        List<Appointment> GetNewAppByPatientId(int patientId);
        List<Appointment> GetOldAppByPatientId(int patientId);
        List<Appointment> GetAppointmentByPhone(string phoneNumber);
        List<Appointment> GetResultByPatientId(int patientId);
        List<Appointment> GetAllApp();
        Appointment GetAppById(int appId);
        Appointment GetAppointmentByCode(string code);
        Appointment GetAppointmentById(int id);
        Appointment GetAppointmentByIdInclude(int id);
        List<Appointment> GetResultByAppCode(string appCode);
        int? CountByDate(string sDate);
    }
    public class AppointmentRepository : RepositoryBase<Appointment>, IAppointmentRepository
    {
        // DucBM
        public List<Appointment> GetAppointmentsByPatientId(int patientId)
        {
            var result = DbSet.AsQueryable()
                .Where(x => x.PatientId == patientId)
                .Include(x => x.SampleGettings.Select(y => y.LabTestings))
                .ToList();
            return result;
        }
        public List<Appointment> GetAllApp()
        {
            var result = DbSet.AsQueryable()
               .Include(x => x.Patient)
               .Include(x => x.SampleGettings.Select(y=>y.Sample))
               .ToList();
            return result;
        }
        public Appointment GetAppById(int appId)
        {
            var result = DbSet.AsQueryable()
               .Include(x => x.Patient)
               .Include(x => x.SampleGettings.Select(y => y.Sample))
               .FirstOrDefault(p => p.AppointmentId == appId);
            return result;
        }
        public List<Appointment> GetNewAppByPatientId(int patientId)
        {
            var appointment = DbSet.AsQueryable()
                .Where(x => x.IsDeleted == false && x.Status.ToUpper().Contains("NEW") && x.PatientId == patientId)
                .Include(x => x.Patient)
                .Include(x => x.SampleGettings.Select(y => y.LabTestings.Select(z => z.LabTest)))
                .Include(x => x.SampleGettings.Select(y => y.Sample))
                .OrderByDescending(x => x.AppointmentId)
                .FirstOrDefault();
            if (appointment==null)
            {
                return null;
            }
            var result = new List<Appointment>();
            result.Add(appointment);
            return result;
        }

        public List<Appointment> GetOldAppByPatientId(int patientId)
        {
            var result = DbSet.AsQueryable()
                .Where(x => x.IsDeleted == false && x.Status.ToUpper().Contains("DONE") && x.PatientId == patientId)
                .Include(x => x.Patient)
                .Include(x => x.SampleGettings.Select(y => y.Sample))
                .ToList();
            return result;
        }

        public List<Appointment> GetResultByPatientId(int patientId)
        {
            var result = DbSet.AsQueryable()
                .Where(x => x.PatientId == patientId)
                .Include(x => x.Patient)
                .Include(x => x.Employee)

                .Include(x => x.SampleGettings.Select(y => y.Sample))
                .Include(x => x.SampleGettings.Select(y => y.LabTestings.Select(z => z.LabTest)))

                .Include(x => x.SampleGettings.Select(y => y.LabTestings.Select(z => z.LabTestingIndexes)))
                .ToList();
            return result;
        }

        public Appointment GetAppointmentById(int id)
        {
            var result = DbSet.AsQueryable()
                .Where(x => x.IsDeleted != true && x.AppointmentId.Equals(id))
                .FirstOrDefault();
            return result;
        }

        public Appointment GetAppointmentByIdInclude(int id)
        {
            var result = DbSet.AsQueryable()
                .Where(x => x.IsDeleted != true && x.AppointmentId.Equals(id))

                .Include(x => x.SampleGettings.Select(y => y.LabTestings))

                .Include(x => x.SampleGettings)

                .FirstOrDefault();
            return result;
        }

        public Appointment GetAppointmentByCode(string code)
        {
            var result = DbSet.AsQueryable()
                .Where(x => x.IsDeleted != true && x.AppointmentCode.Equals(code))

                .Include(x => x.SampleGettings.Select(y => y.LabTestings))

                .Include(x => x.SampleGettings)

                .FirstOrDefault();
            return result;
        }

        public List<Appointment> GetResultByAppCode(string appCode)
        {
            var result = DbSet.AsQueryable()
                .Where(x => x.AppointmentCode == appCode)
                .Include(x => x.Patient)
                .Include(x => x.Employee)

                .Include(x => x.SampleGettings.Select(y => y.Sample))
                .Include(x => x.SampleGettings.Select(y => y.LabTestings.Select(z => z.LabTest)))

                .Include(x => x.SampleGettings.Select(y => y.LabTestings.Select(z => z.LabTestingIndexes)))
                .ToList();
            return result;
        }

        public List<Appointment> GetAppointmentByPhone(string phoneNumber)
        {
            var result = DbSet.AsQueryable()
                .Where(x => x.Patient.PhoneNumber.Trim().Equals(phoneNumber.Trim()))
                .Include(x => x.Patient)
                .ToList();
            return result;
        }

        public int? CountByDate(string sDate)
        {
            if (sDate == null)
            {
                return null;
            }
            sDate = sDate.Trim();
            //var dateLength = "yyyy-MM-dd".Length;
            var result = DbSet.AsQueryable().
                //Where(x => x.IsDeleted==false && x.AppointmentCode.Take(dateLength).Equals(sDate))
                Where(x => x.IsDeleted == false && x.AppointmentCode.Contains(sDate))
                .Count();
            return result;
        }

       
    }
}
