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
    }
}
