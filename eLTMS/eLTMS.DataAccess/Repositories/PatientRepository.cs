using eLTMS.DataAccess.Infrastructure;
using eLTMS.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLTMS.DataAccess.Repositories
{
    public interface IPatientRepository : IRepository<Patient>
    {
       // List<Supply> GetAllSupply(string suppliesCode);
        //Supply GetSimpleById(int id);
    }


  public  class PatientRepository : RepositoryBase<Patient>, IPatientRepository
    {
    }
}
