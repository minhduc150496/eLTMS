using eLTMS.DataAccess.Infrastructure;
using eLTMS.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLTMS.DataAccess.Repositories
{
    public interface ILabTestingIndexRepository : IRepository<LabTestingIndexRepository>
    {
        List<LabTestingIndexRepository> GetAll();
    }

    public class LabTestingIndexRepository : RepositoryBase<LabTestingIndexRepository>, ILabTestingIndexRepository
    {
        public List<LabTestingIndexRepository> GetAll()
        {
            var results = DbSet.AsQueryable()
                //.Include(x => x.LabTestSampleMappings)
                .ToList();
            return results;
        }
    }
}

