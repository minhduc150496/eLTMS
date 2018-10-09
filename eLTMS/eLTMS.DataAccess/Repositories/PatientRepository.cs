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
    public interface IPatientRepository : IRepository<Patient>
    {
        List<Patient> GetAllPatient(string phoneNumber);
        Patient GetSimpleById(int id);
    }


  public  class PatientRepository : RepositoryBase<Patient>, IPatientRepository
    {
        public List<Patient> GetAllPatient(string phoneNumber)
        {
            var result = DbSet.AsQueryable()
                .Where(x => x.PhoneNumber.Contains(phoneNumber) && x.IsDeleted == false)
                .ToList();
            return result;
        }
        public Patient GetSimpleById(int id)
        {
            var result = DbSet.AsQueryable()
                .SingleOrDefault(x => x.PatientId == id);
            return result;
        }
    }
}
