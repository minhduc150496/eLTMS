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
    public interface IFeedback
    {

    }
    public class FeedbackService: IFeedback
    {
        private readonly IRepositoryHelper RepositoryHelper;
        private readonly IUnitOfWork UnitOfWork;
        public FeedbackService(IRepositoryHelper repositoryHelper)
        {
            RepositoryHelper = repositoryHelper;
            UnitOfWork = RepositoryHelper.GetUnitOfWork(); 
        }
        public List<Feedback> GetAllFeed(String dateTime)
        {
            var feedRepo = this.RepositoryHelper.GetRepository<IFeedbackRepository>(UnitOfWork);
            var feedback = feedRepo.GetAllFeed(dateTime);
            return feedback;
        }
    }
}
