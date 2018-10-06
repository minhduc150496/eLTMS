using eLTMS.DataAccess.Models;
using System;
using eLTMS.DataAccess.Infrastructure;
using eLTMS.DataAccess.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLTMS.BusinessLogic.Services
{
    
    public interface IPatientService
    {
        //List<Supply> GetAllSupplies(string suppliesCode);
        bool AddPatient(Patient patient);
        //bool Update(int id, string code, string name, int type, string unit, string note);
        //Supply GetSupplyById(int id);
        //bool Delete(int id);
    }
    public class PatientService : IPatientService
    {
        private readonly IRepositoryHelper RepositoryHelper;
        private readonly IUnitOfWork UnitOfWork;
        public PatientService(IRepositoryHelper repositoryHelper)
        {
            RepositoryHelper = repositoryHelper;
            UnitOfWork = RepositoryHelper.GetUnitOfWork();
        }
        
        public bool AddPatient(Patient patient)
        {
            var repo = RepositoryHelper.GetRepository<IPatientRepository>(UnitOfWork);
                 try
            {
                repo.Create(patient);
                UnitOfWork.SaveChanges();
            }
            catch (Exception) { return false; }
            return true;
        }

    }
}
