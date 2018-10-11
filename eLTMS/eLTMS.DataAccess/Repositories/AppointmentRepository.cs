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
<<<<<<< HEAD
                .Where(x => x.Status.Equals("NEW") && x.PatientId == patientId)
=======
                .Where(x => x.Status.Contains("NEW") && x.PatientId == patientId)
>>>>>>> 00aed11b368192dcf20360e42ff6fbd83f0b573e
                .Include(x => x.Patient)
                .Include(x => x.SampleGettings.Select( y => y.Sample))
                .ToList();
            return result;
        }
    }
}
