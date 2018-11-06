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


    public class PatientRepository : RepositoryBase<Patient>, IPatientRepository
    {
        public List<Patient> GetAllPatient(string phoneNumber)
        {
            var result = DbSet.AsQueryable()
<<<<<<< HEAD
                .Include(x=>x.Account)
                .Where(x => x.PhoneNumber.Contains(phoneNumber) && x.IsDeleted == false)
=======
                .Where(x => x.PhoneNumber.Contains(phoneNumber) || x.FullName.Contains(phoneNumber) || x.PatientCode.Contains(phoneNumber) || x.HomeAddress.Contains(phoneNumber)  && x.IsDeleted == false)
>>>>>>> 3383418b6c70ca3ad57a481e079f2f199634d463
                .ToList();
            return result;
        }
        public Patient GetSimpleById(int id)
        {
            var result = DbSet.AsQueryable()
                .Include(x => x.Account)
                .SingleOrDefault(x => x.PatientId == id);
            return result;
        }
    }
}
