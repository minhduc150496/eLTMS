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

    public interface ITestingRepository : IRepository<Testing>
    {
        List<Testing> GetAllTesting();
        Testing GetById(int id);
    }
    public class TestingRepository : RepositoryBase<Testing>, ITestingRepository
    {
        public List<Testing> GetAllTesting()
        {
            var result = DbSet.AsQueryable()
                .Include(x => x.Patient)
                .Include(x => x.LabTest)
                .ToList();
            return result;
        }
        public Testing GetById(int id)
        {
            var result = DbSet.AsQueryable()
                .Where(x => x.TestingId == id)
                .Include(x => x.Patient)
                .Include(x => x.LabTest)
                .ToList()
                .FirstOrDefault();
            return result;
        }
    }
}
