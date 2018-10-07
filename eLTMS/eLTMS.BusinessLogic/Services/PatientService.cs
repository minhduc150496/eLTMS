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
        List<Patient> GetAllPatients(string phoneNumber);
        bool AddPatient(Patient patient);
        bool Update(int id, string code, string name, int type, string unit, string note);
        Patient GetPatientById(int id);
        //bool Delete(int id);
       // bool UpdatePatient(Patient dto);
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

        public List<Patient> GetAllPatients(string phoneNumber)
        {
            var patientRepo = this.RepositoryHelper.GetRepository<IPatientRepository>(UnitOfWork);
            var patients = patientRepo.GetAllPatient(phoneNumber);
            return patients;
        }
        public Patient GetPatientById(int id)
        {
            var patientRepo = this.RepositoryHelper.GetRepository<IPatientRepository>(UnitOfWork);
            var patients = patientRepo.GetSimpleById(id);
            return patients;
        }
        public bool Update(int id, string code, string name, int type, string unit, string note)
        {
            var repo = RepositoryHelper.GetRepository<ISupplyRepository>(UnitOfWork);

            try
            {
                var supply = repo.GetById(id);
                supply.SuppliesCode = code;
                supply.SuppliesName = name;
                supply.SuppliesTypeId = type;
                supply.Unit = unit;
                supply.Note = note;
                repo.Update(supply);
                UnitOfWork.SaveChanges();
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
        // public bool UpdatePatient(Patient dto)
        //{
        //    var patientRepo = this.RepositoryHelper.GetRepository<IPatientRepository>(UnitOfWork);
        //    var patient = patientRepo.GetById(dto.PatientId);
        //    patient.PhoneNumber = dto.PhoneNumber;
        //    [
        //    if (dto.AccountId == 0 || dto.AccountId is null)
        //    {
        //        patient.AccountId = null;
        //    }
        // }
    }
}
