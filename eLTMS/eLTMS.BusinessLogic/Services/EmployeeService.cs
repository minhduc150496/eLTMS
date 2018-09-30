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
        List<Employee> GetAll();
        //List<Employee> GetByName(string name);
        //bool Insert(int id, string name, int age);
        //bool Delete(int id);
        //bool Update(int id, string name, int age);
    }
    public class EmployeeService : IEmployeeService
    {
        private readonly IRepositoryHelper RepositoryHelper;
        public EmployeeService(IRepositoryHelper repositoryHelper)
        {
            RepositoryHelper = repositoryHelper;
        }

        public List<Employee> GetAll()
        {
            var unitOfWork = RepositoryHelper.GetUnitOfWork();
            var repo = RepositoryHelper.GetRepository<IEmployeeRepository>(unitOfWork);
            var results = repo.GetAll().ToList();
            return results;
        }

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
