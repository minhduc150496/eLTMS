using eLTMS.DataAccess.Infrastructure;
using eLTMS.DataAccess.Models;
using eLTMS.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLTMS.BusinessLogic.Services
{
    public interface IEmployeeService
    {
        List<Employee> GetAllEmployees(string fullName);
        //List<Employee> GetAll();
        //List<Employee> GetByName(string name);
        //bool Insert(int id, string name, int age);
        //bool Delete(int id);
        //bool Update(int id, string name, int age);
       // bool Update(int id, string status, string fullname, string gender, DateTime? dateOfBirth, string phone, string address, DateTime? dateStart, string Role);
        bool Update(Employee employeedto);
        bool AddEmployee(Employee employee);
        Employee getEmployeeById(int id);
    }
    public class EmployeeService : IEmployeeService
    {
        private readonly IRepositoryHelper RepositoryHelper;
        private readonly IUnitOfWork UnitOfWork;
        public EmployeeService(IRepositoryHelper repositoryHelper)
        {
            RepositoryHelper = repositoryHelper;
            UnitOfWork = RepositoryHelper.GetUnitOfWork();
        }

        public List<Employee> GetAllEmployees(string fullName)
        {
            var employeeRepo = this.RepositoryHelper.GetRepository<IEmployeeRepository>(UnitOfWork);
            var employee = employeeRepo.GetAllEmployee(fullName);
            return employee;
        }
        public bool Update(Employee employeedto)
        {
           // int id, string status, string fullname, string gender, DateTime? dateOfBirth, string phone, string address, DateTime? dateStart, string Role
            var repo = RepositoryHelper.GetRepository<IEmployeeRepository>(UnitOfWork);
            
            try
            {           
                var employee = repo.GetSimpleById(employeedto.EmployeeId);              
                var account = employee.Account;
                employee.Status = employeedto.Status;
                employee.FullName = employeedto.FullName;
                employee.Gender = employeedto.Gender;
                employee.DateOfBirth = employeedto.DateOfBirth;
                employee.PhoneNumber = employeedto.PhoneNumber;
                employee.HomeAddress = employeedto.HomeAddress;
                employee.StartDate = employeedto.StartDate;
                account.Role = employeedto.Account.Role;
                employee.AccountId = employee.AccountId;
                repo.Update(employee);
                var result = UnitOfWork.SaveChanges();
                if (result.Any()) return false;
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
        public Employee getEmployeeById(int id)
        {
            var employeeRepo = this.RepositoryHelper.GetRepository<IEmployeeRepository>(UnitOfWork);
            var employees = employeeRepo.GetSimpleById(id);
            return employees;
        }
        public bool AddEmployee(Employee employee)
        {
            var repo = RepositoryHelper.GetRepository<IEmployeeRepository>(UnitOfWork);
            try
            {
                repo.Create(employee);
                UnitOfWork.SaveChanges();
            }
            catch (Exception ex) { return false; }
            return true;
        }
        //public List<Employee> GetAll()
        //{
        //  var unitOfWork = RepositoryHelper.GetUnitOfWork();
        //  var repo = RepositoryHelper.GetRepository<IEmployeeRepository>(unitOfWork);
        //   var results = repo.GetAll().ToList();
        //    return results;
        //}

        //public List<Employee> GetByName(string name)
        //{
        //    var unitOfWork = RepositoryHelper.GetUnitOfWork();
        //    var repo = RepositoryHelper.GetRepository<IEmployeeRepository>(unitOfWork);
        //    var results = repo.GetAll().Where(x => x.Name.ToLower().Contains(name.ToLower())).ToList();
        //    return results;
        //}

        //public bool Insert(int id, string name, int age)
        //{
        //    var unitOfWork = RepositoryHelper.GetUnitOfWork();
        //    var repo = RepositoryHelper.GetRepository<IEmployeeRepository>(unitOfWork);

        //    try
        //    {
        //        repo.Create(new Employee(id, name, age));
        //        unitOfWork.SaveChanges();
        //    } catch (Exception)
        //    {
        //        return false;
        //    }

        //    return true;
        //}

        //public bool Delete(int id)
        //{
        //    var unitOfWork = RepositoryHelper.GetUnitOfWork();
        //    var repo = RepositoryHelper.GetRepository<IEmployeeRepository>(unitOfWork);

        //    try
        //    {
        //        var employee = repo.GetById(id);
        //        repo.Delete(employee);
        //        unitOfWork.SaveChanges();
        //    }
        //    catch (Exception)
        //    {
        //        return false;
        //    }

        //    return true;
        //}

        //public bool Update(int id, string name, int age)
        //{
        //    var unitOfWork = RepositoryHelper.GetUnitOfWork();
        //    var repo = RepositoryHelper.GetRepository<IEmployeeRepository>(unitOfWork);

        //    try
        //    {
        //        var employee = repo.GetById(id);
        //        employee.Age = age;
        //        employee.Name = name;
        //        repo.Update(employee);
        //        unitOfWork.SaveChanges();
        //    }
        //    catch (Exception)
        //    {
        //        return false;
        //    }

        //    return true;
        //}
    }
}
