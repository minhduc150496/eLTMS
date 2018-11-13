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
    }
    public class TableRepository : RepositoryBase<Table>, ITableRepository
    {
        public List<Table> GetAllTable()
        {
            var result = DbSet.AsQueryable()
                .ToList();
            return result;
        }

    }
}
