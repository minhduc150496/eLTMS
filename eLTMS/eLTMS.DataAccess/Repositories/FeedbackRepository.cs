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
        List<Feedback> GetAllFeed(String dateTime);
    }
    public class FeedbackRepository: RepositoryBase<Feedback>, IFeedbackRepository
    {
        public List<Feedback> GetAllFeed(string dateTime)
        {
            var result = DbSet.AsQueryable()
                    .Include(x => x.FeedbackId)
                    .Where(x => ((x.FeedbackId).ToString().Contains(dateTime) || (x.EmployeeId).ToString().Contains(dateTime) || (x.ReceivedDateTime).ToString().Contains(dateTime)) && x.IsDeleted == false)
                    .ToList();
            return result;
        }
    }
}
