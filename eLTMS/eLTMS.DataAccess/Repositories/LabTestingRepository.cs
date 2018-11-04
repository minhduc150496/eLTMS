using eLTMS.DataAccess.Infrastructure;
using eLTMS.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace eLTMS.DataAccess.Repositories
{
    public interface ILabTestingRepository : IRepository<LabTesting>
    {
        List<LabTesting> GetAllLabTesting();

        List<LabTesting> GetAllLabTestingResult();
        List<LabTesting> GetAllLabTestings();
        LabTesting GetLabTestingById(int id);
        List<LabTesting> GetLabTestingByListId(List<int> ids);
        List<LabTesting> GetAllLabTestingHaveAppointmentCode(String code);


      



    }
    public class LabTestingRepository : RepositoryBase<LabTesting>, ILabTestingRepository
    {
        public List<LabTesting> GetAllLabTesting()
        {
            var result = DbSet.AsQueryable()
                .Include(x => x.LabTest)
                .Include(x => x.LabTestingIndexes)
                .ToList();
            return result;
        }

        public List<LabTesting> GetAllLabTestings()
        {

            var result = DbSet.AsQueryable()
             .Where(x => x.Status.Contains("Waiting"))
             .Include(x => x.LabTest)
             .Include(x => x.SampleGetting.Appointment)
             .Include(x => x.SampleGetting.Sample)
             .ToList();
            return result;

        }
        public List<LabTesting> GetAllLabTestingHaveAppointmentCode(String code)
        {

            var result = DbSet.AsQueryable()
             .Where(x => x.SampleGetting.Appointment.AppointmentCode.Contains(code))
             .Include(x => x.LabTest)
             .Include(x => x.SampleGetting.Appointment)
             .Include(x => x.SampleGetting.Sample)
             .ToList();
            return result;

        }
        public List<LabTesting> GetAllLabTestingResult()
        {

            var result = DbSet.AsQueryable()
             .Where(x => x.Status.Contains("LabtestDone"))
             .Include(x => x.LabTest)
             .Include(x => x.SampleGetting.Appointment)
             .Include(x => x.SampleGetting.Sample)
             .ToList();
            return result;

        }
        public LabTesting GetLabTestingById(int id)
        {
            var result = DbSet.Where(s => s.LabTestingId == id).ToList().FirstOrDefault();
            return result;
        }

        public List<LabTesting> GetLabTestingByListId(List<int> ids)
        {
            var result = DbSet.Where(s => ids.Contains(s.LabTestingId)).ToList();

            return result;
        }
    }
}
