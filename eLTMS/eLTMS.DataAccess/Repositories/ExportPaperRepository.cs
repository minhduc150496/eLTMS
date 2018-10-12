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
    public interface IExportPaperRepository : IRepository<ExportPaper>
    {
        ExportPaper GetSimpleById(string code);
        List<ExportPaper> GetAllExportPaper(string createDate);
        ExportPaper GetSimpleById(int id);
    }
    public class ExportPaperRepository : RepositoryBase<ExportPaper>, IExportPaperRepository
    {
        public ExportPaper GetSimpleById(string code)
        {
            return DbSet
                .Include( x => x.ExportPaperDetails)
                .SingleOrDefault(x => x.ExportPaperCode == code);
        }
        public List<ExportPaper> GetAllExportPaper(string createDate)
        {
            var result = DbSet.AsQueryable()
                .Where(x => x.CreateDate.ToString().Contains(createDate) && x.IsDeleted == false)
                .ToList();
            return result;
        }
        public ExportPaper GetSimpleById(int id)
        {
            var result = DbSet.AsQueryable()
                .Include(x=>x.ExportPaperDetails)
                .SingleOrDefault(x => x.ExportPaperId == id);
            return result;
        }
    }
}
