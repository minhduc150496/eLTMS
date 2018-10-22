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
        List<LabTest> GetAllLabTest(string code);
        LabTest GetSimpleById(int id);
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
        public List<LabTest> GetAllLabTest(string code)
        {
            var result = DbSet.AsQueryable()
                .Where(x => x.LabTestCode.Contains(code) && x.IsDeleted == false)
                .Include(x => x.Sample)
                .ToList();
            return result;
        }

        public LabTest GetSimpleById(int id)
        {
            var result = DbSet.AsQueryable()
                //.Include(x => x.SampleId)
                .SingleOrDefault(x => x.LabTestId == id);
            return result;
        }
    }
}
