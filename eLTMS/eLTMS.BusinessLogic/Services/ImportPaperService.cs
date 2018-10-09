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
    public interface IImportPaperService
    {
        bool AddImportPaper(ImportPaper importPaper);
        List<ImportPaper> GetAllImportPapers(string createDate);
        ImportPaper GetImportPaperById(int id);
        ImportPaper GetByimportPaperCode(string code);
    }
    public class ImportPaperService : IImportPaperService
    {
        private readonly IRepositoryHelper RepositoryHelper;
        private readonly IUnitOfWork UnitOfWork;
        public ImportPaperService(IRepositoryHelper repositoryHelper)
        {
            RepositoryHelper = repositoryHelper;
            UnitOfWork = RepositoryHelper.GetUnitOfWork();
        }

        public bool AddImportPaper(ImportPaper importPaper)
        {
            var importPaperRepo = RepositoryHelper.GetRepository<IImportPaperRepository>(UnitOfWork);
            var importPaperDetailRepo = RepositoryHelper.GetRepository<IImportPaperDetailRepository>(UnitOfWork);
            using (var transaction = UnitOfWork.BeginTransaction())
            {

                try
                {
                    importPaper.CreateDate = DateTime.Now;
                    importPaperRepo.Create(importPaper);
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

        public ImportPaper GetByimportPaperCode(string code)
        {
            var importPaperRepo = RepositoryHelper.GetRepository<IImportPaperRepository>(UnitOfWork);
            var data = importPaperRepo.GetSimpleById(code);
            return data;
        }
        public List<ImportPaper> GetAllImportPapers(string createDate)
        {
            var importPaperRepo = this.RepositoryHelper.GetRepository<IImportPaperRepository>(UnitOfWork);
            var importPapers = importPaperRepo.GetAllImportPaper(createDate);
            return importPapers;
        }
        public ImportPaper GetImportPaperById(int id)
        {
            var importPaperRepo = this.RepositoryHelper.GetRepository<IImportPaperRepository>(UnitOfWork);
            var importPapers = importPaperRepo.GetSimpleById(id);
            return importPapers;
        }
    }
}
