using eLTMS.DataAccess.Infrastructure;
using eLTMS.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLTMS.DataAccess.Repositories
{
    public interface IEmployeeRepository : IRepository<Employee>
    {
        List<Employee> GetAllEmployee(string fullName);
        
    }
    public class EmployeeRepository : RepositoryBase<Employee>, IEmployeeRepository
    {
        public List<Employee> GetAllEmployee(string fullName)
        {
            var result = DbSet.AsQueryable()
                .Where(x => x.FullName.Contains(fullName) && x.IsDeleted == false)
                .ToList();
            return result;
        }
        public Employee GetSimpleById(int id)
        {
            var result = DbSet.AsQueryable().SingleOrDefault(x => x.EmployeeId == id);
            return result;
        }
    }
}
