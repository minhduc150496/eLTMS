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

        List<LabTesting> GetAllLabTestings();
        LabTesting GetLabTestingById(int id);
        List<LabTesting> GetLabTestingByListId(List<int> ids);

      

>>>>>>> d15abce0795bc092859b4a754703f9daa3c12730
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
=======

>>>>>>> d15abce0795bc092859b4a754703f9daa3c12730
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
<<<<<<< HEAD
        
=======

>>>>>>> d15abce0795bc092859b4a754703f9daa3c12730
            return result;
        }
    }
}
