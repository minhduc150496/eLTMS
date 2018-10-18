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
    public interface ISampleGettingRepository : IRepository<SampleGetting>
    {
        List<SampleGetting> GetAll();
    }
    public class SampleGettingRepository : RepositoryBase<SampleGetting>, ISampleGettingRepository
    {
        public List<SampleGetting> GetAll()
        {
            var results = DbSet.AsQueryable()
                .ToList();
            return results;
        }
    }
}
