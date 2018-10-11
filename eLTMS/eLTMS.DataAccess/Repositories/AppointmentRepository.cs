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
    }
    public class AppointmentRepository: RepositoryBase<Appointment>, IAppointmentRepository
    {
        public List<Appointment> GetNewAppByPatientId(int patientId)
        {
            var result = DbSet.AsQueryable()
                .Where(x => x.Status.Equals("NEW") && x.PatientId == patientId)
                .Include(x => x.Patient)
                .Include(x => x.SampleGettings)
                .ToList();
            return result;
        }
    }
}
