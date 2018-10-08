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
    public interface IImportPaperRepository : IRepository<ImportPaper>
    {
        ImportPaper GetSimpleById(string code);
        List<ImportPaper> GetAllImportPaper(string createDate);
        ImportPaper GetSimpleById(int id);
    }
    public class ImportPaperRepository : RepositoryBase<ImportPaper>, IImportPaperRepository
    {
        public ImportPaper GetSimpleById(string code)
        {
            return DbSet
                .Include( x => x.ImportPaperDetails)
                .SingleOrDefault(x => x.ImportPaperCode == code);
        }
        public List<ImportPaper> GetAllImportPaper(string createDate)
        {
            var result = DbSet.AsQueryable()
                .Where(x => x.CreateDate.ToString().Contains(createDate) && x.IsDeleted == false)
                .ToList();
            return result;
        }
        public ImportPaper GetSimpleById(int id)
        {
            var result = DbSet.AsQueryable()
                .Include(x=>x.ImportPaperDetails)
                .SingleOrDefault(x => x.ImportPaperId == id);
            return result;
        }
    }
}
