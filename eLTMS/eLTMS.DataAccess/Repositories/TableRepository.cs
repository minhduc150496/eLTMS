using eLTMS.DataAccess.Infrastructure;
using eLTMS.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLTMS.DataAccess.Repositories
{
    public interface ITableRepository : IRepository<Table>
    {
        List<Table> GetAllTable();
        int GetTableCountBySampleGroupId(int sampleGroupId);
    }
    public class TableRepository : RepositoryBase<Table>, ITableRepository
    {
        public List<Table> GetAllTable()
        {
            var result = DbSet.AsQueryable()
                .ToList();
            return result;
        }

        public int GetTableCountBySampleGroupId(int sampleGroupId)
        {
            var result = DbSet.AsQueryable()
                .Where(x => x.SampleGroupId == sampleGroupId)
                .Count();
            return result;
        }
    }
}
