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

    }
    public class EmployeeRepository : RepositoryBase<Employee>, IEmployeeRepository
    {
        
        
    }
}
