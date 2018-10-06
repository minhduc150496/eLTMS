using eLTMS.DataAccess.Infrastructure;
using eLTMS.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLTMS.DataAccess.Repositories
{
    public interface IImportPaperRepository : IRepository<ImportPaper>
    {

    }
    public class ImportPaperRepository : RepositoryBase<ImportPaper>, IImportPaperRepository
    {
    }
}
