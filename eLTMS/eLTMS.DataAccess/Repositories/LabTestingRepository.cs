using eLTMS.DataAccess.Infrastructure;
using eLTMS.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Entity;

namespace eLTMS.DataAccess.Repositories
{
    public interface ILabTestingRepository : IRepository<LabTesting>
    {
        List<Employee> GetAllEmployee(string fullName);
        Employee GetSimpleById(int id);
    }
    public class LabTestingRepository : RepositoryBase<LabTesting>, ILabTestingRepository
    {
        public List<Employee> GetAllEmployee(string fullName)
        {
            var result = DbSet.AsQueryable()
                .Include(x => x.Account)
                .Where(x => x.FullName.Contains(fullName) && x.IsDeleted == false)
                .ToList();
            return result;
        }
        public Employee GetSimpleById(int id)
        {
            var result = DbSet.AsQueryable()
                .Include(x=>x.Account)
                .SingleOrDefault(x => x.EmployeeId == id);
            return result;
        }
    }
}
