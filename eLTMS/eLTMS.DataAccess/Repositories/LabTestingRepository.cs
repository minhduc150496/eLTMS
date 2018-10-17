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
    public interface ILabTestingRepository : IRepository<LabTesting>
    {
        List<LabTesting> GetAllLabTesting();
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
    }
}
