using eLTMS.DataAccess.Infrastructure;
using eLTMS.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Entity;

namespace eLTMS.DataAccess.Repositories
{
    public interface IAppointmentRepository : IRepository<Appointment>
    {
    }
    public class AppointmentRepository: RepositoryBase<Appointment>, IAppointmentRepository
    {

    }
}
