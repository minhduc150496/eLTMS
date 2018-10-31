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
<<<<<<< HEAD
        List<LabTesting> GetAllLabTestings();
        LabTesting GetLabTestingById(int id);
        List<LabTesting> GetLabTestingByListId(List<int> ids);
=======
        List<LabTesting> GetByPatientId(int patientId);
>>>>>>> 74cc0cdad23dc50a11c25ae504a8a36a1e7922c5
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
<<<<<<< HEAD
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

        public LabTesting GetLabTestingById(int id)
        {
            var result = DbSet.Where(s => s.LabTestingId == id).ToList().FirstOrDefault();
            return result;
        }

        public List<LabTesting> GetLabTestingByListId(List<int> ids)
        {
            var result = DbSet.Where(s => ids.Contains(s.LabTestingId)).ToList();
=======
        public List<LabTesting> GetByPatientId(int patientId)
        {
            var result = DbSet.AsQueryable()
                .Include(x => x.LabTest)
                .ToList();
>>>>>>> 74cc0cdad23dc50a11c25ae504a8a36a1e7922c5
            return result;
        }
    }
}
