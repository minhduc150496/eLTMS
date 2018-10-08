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
    public interface ISampleRepository : IRepository<Sample>
    {
        List<Sample> GetAllSamples();
    }
    public class SampleRepository : RepositoryBase<Sample>, ISampleRepository
    {
        public List<Sample> GetAllSamples()
        {
            var result = DbSet.AsQueryable()
                .Include (x  => x.LabTests)
                .ToList();
            return result;
        }
    }
}
