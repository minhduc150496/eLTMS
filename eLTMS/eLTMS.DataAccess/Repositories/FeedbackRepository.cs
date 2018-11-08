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
   public interface IFeedbackRepository: IRepository<Feedback>
    {
        List<Feedback> GetAllFeed(string dateTime);
        Feedback GetSimpleById(int id);
    }
    public class FeedbackRepository: RepositoryBase<Feedback>, IFeedbackRepository
    {
        public List<Feedback> GetAllFeed(string dateTime)
        {
            var result = DbSet.AsQueryable()
                .Include(x=>x.Employee)
                .Include(x=>x.Patient)
                    .Where(x => ((x.Content).ToString().Contains(dateTime) || (x.FeedbackId).ToString().Contains(dateTime) || (x.Patient.FullName).ToString().Contains(dateTime) || (x.Employee.FullName).ToString().Contains(dateTime) || (x.Status).ToString().Contains(dateTime) || x.ReceivedDateTime.ToString().Contains(dateTime)) && x.IsDeleted == false)
                    .ToList();
            return result;
        }
        public Feedback GetSimpleById(int id)
        {
            var result = DbSet.AsQueryable()
                .Include(x => x.Employee)
                .Include(x => x.Patient)
                .SingleOrDefault(x => x.FeedbackId == id);
            return result;
        }
    }
}
