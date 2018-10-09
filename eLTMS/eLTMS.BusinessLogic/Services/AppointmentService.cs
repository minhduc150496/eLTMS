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
    public interface IAppointmentService
    {
        bool Create(Appointment appointment);
    }
    public class AppointmentService: IAppointmentService
    {
        private readonly IRepositoryHelper RepositoryHelper;
        private readonly IUnitOfWork UnitOfWork;
        public AppointmentService(IRepositoryHelper repositoryHelper)
        {
            RepositoryHelper = repositoryHelper;
            UnitOfWork = RepositoryHelper.GetUnitOfWork();
        }

        public bool Create(Appointment appointment)
        {
            var appointmentRepo = this.RepositoryHelper.GetRepository<IAppointmentRepository>(this.UnitOfWork);
            try
            {
                appointmentRepo.Create(appointment);
                var result = this.UnitOfWork.SaveChanges();
                if (result.Any())
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
    }
}
