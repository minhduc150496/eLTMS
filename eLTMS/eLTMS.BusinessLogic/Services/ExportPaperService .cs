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
    public interface IExportPaperService
    {
        bool AddExportPaper(ExportPaper exportPaper);
        List<ExportPaper> GetAllExportPapers(string createDate);
        ExportPaper GetExportPaperById(int id);
        ExportPaper GetByexportPaperCode(string code);
    }
    public class ExportPaperService : IExportPaperService
    {
        private readonly IRepositoryHelper RepositoryHelper;
        private readonly IUnitOfWork UnitOfWork;
        public ExportPaperService(IRepositoryHelper repositoryHelper)
        {
            RepositoryHelper = repositoryHelper;
            UnitOfWork = RepositoryHelper.GetUnitOfWork();
        }

        public bool AddExportPaper(ExportPaper exportPaper)
        {
            var exportPaperRepo = RepositoryHelper.GetRepository<IExportPaperRepository>(UnitOfWork);
            var exportPaperDetailRepo = RepositoryHelper.GetRepository<IExportPaperDetailRepository>(UnitOfWork);
            using (var transaction = UnitOfWork.BeginTransaction())
            {

                try
                {
                    exportPaper.CreateDate = DateTime.Now;
                    exportPaperRepo.Create(exportPaper);
                    var dbValresults =  UnitOfWork.SaveChanges();
                    if (dbValresults.Any())
                    {
                        transaction.Rollback();
                        return false;
                    }
                    dbValresults =  UnitOfWork.SaveChanges();
                    if (dbValresults.Any())
                    {
                        transaction.Rollback();
                        return false;
                    }

                    transaction.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return true;
                }
            }
        }

        public ExportPaper GetByexportPaperCode(string code)
        {
            var exportPaperRepo = RepositoryHelper.GetRepository<IExportPaperRepository>(UnitOfWork);
            var data = exportPaperRepo.GetSimpleById(code);
            return data;
        }
        public List<ExportPaper> GetAllExportPapers(string createDate)
        {
            var exportPaperRepo = this.RepositoryHelper.GetRepository<IExportPaperRepository>(UnitOfWork);
            var exportPapers = exportPaperRepo.GetAllExportPaper(createDate);
            return exportPapers;
        }
        public ExportPaper GetExportPaperById(int id)
        {
            var exportPaperRepo = this.RepositoryHelper.GetRepository<IExportPaperRepository>(UnitOfWork);
            var exportPapers = exportPaperRepo.GetSimpleById(id);
            return exportPapers;
        }
    }
}
