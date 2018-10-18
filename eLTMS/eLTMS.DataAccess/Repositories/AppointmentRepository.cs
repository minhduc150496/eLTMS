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
        List<Appointment> GetNewAppByPatientId(int patientId);
        List<Appointment> GetOldAppByPatientId(int patientId);
        List<Appointment> GetResultByPatientId(int patientId);
        List<Appointment> GetResultByAppCode(string appCode);
        List<Appointment> GetAppointmentByPhoneNDate(string phoneNumber);
        int? CountByDate(string sDate);
    }
    public class AppointmentRepository : RepositoryBase<Appointment>, IAppointmentRepository
    {
        public List<Appointment> GetNewAppByPatientId(int patientId)
        {
            var result = DbSet.AsQueryable()
                .Where(x => x.Status.Contains("NEW") && x.PatientId == patientId)
                .Include(x => x.Patient)
                .Include(x => x.SampleGettings.Select(y => y.Sample))
                .ToList();
            return result;
        }

        public List<Appointment> GetOldAppByPatientId(int patientId)
        {
            var result = DbSet.AsQueryable()
                .Where(x => x.Status.Contains("OLD") && x.PatientId == patientId)
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
                .Include(x => x.LabTestings.Select(y => y.LabTest))
                .Include(x => x.LabTestings.Select(y => y.LabTestingIndexes))
                .Include(x => x.LabTestings)
                .ToList();
            return result;
        }

        public List<Appointment> GetResultByAppCode(string appCode)
        {
            var result = DbSet.AsQueryable()
                .Where(x => x.AppointmentCode == appCode)
                .Include(x => x.Patient)
                .Include(x => x.Employee)
                .Include(x => x.SampleGettings.Select(y => y.Sample))
                .Include(x => x.LabTestings.Select(y => y.LabTest))
                .Include(x => x.LabTestings.Select(y => y.LabTestingIndexes))
                .Include(x => x.LabTestings)
                .ToList();
            return result;
        }

        public List<Appointment> GetAppointmentByPhoneNDate(string phoneNumber)
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
            var result = DbSet.AsQueryable().
                Where(x => x.AppointmentCode
                    .Take("yyyy-MM-dd".Length)
                    .ToString()
                    .Equals(sDate))
                .Count();
            return result;
        }

    }
}
