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
    public interface IResultPaperRepository : IRepository<ResultPaper>
    {
        List<ResultPaper> GetResultPapersByPatientId(int patientId);
    }
    public class ResultPaperRepository : RepositoryBase<ResultPaper>, IResultPaperRepository
    {
        public List<ResultPaper> GetResultPapersByPatientId(int patientId)
        {
            //using (DbContext ctx = new DbContext())
            //{
            //    new Repository<ResultPaper>
            //}
            //var result = DbSet.AsQueryable()
            //    .ToList();
            //return result;
            return null;
        }
    }
}
