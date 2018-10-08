using eLTMS.DataAccess.Infrastructure;
using eLTMS.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLTMS.DataAccess.Repositories
{
    public interface ILabTestRepository: IRepository<LabTest>
    {
        List<LabTest> GetAll();
    }
    public class LabTestRepository : RepositoryBase<LabTest>, ILabTestRepository
    {
        public List<LabTest> GetAll()
        {
            var results = DbSet.AsQueryable()
                //.Include(x => x.LabTestSampleMappings)
                .ToList();
            return results;
        }
    }
}
