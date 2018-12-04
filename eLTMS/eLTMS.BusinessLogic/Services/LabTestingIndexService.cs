﻿using eLTMS.DataAccess.Infrastructure;
using eLTMS.DataAccess.Models;
using eLTMS.DataAccess.Repositories;
using eLTMS.Models.Models.dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLTMS.BusinessLogic.Services
{
    public interface ILabTestingIndexService
    {
        List<LabTestingIndex> GetAllLabTestingIndex();
        List<LabTestingIndex> GetAllLabTestingIndexHaveLabtestingId(int labtestingId);
        bool AddLabTestingIndex(List<LabTestingIndex> labTestingIndex);
        ResponseObjectDto AddLabTestingIndexes(List<LabTestingIndex> labTestingIndexes); // DucBM
    }

    public class LabTestingIndexService : ILabTestingIndexService
    {
        private readonly IRepositoryHelper RepositoryHelper;
        private readonly IUnitOfWork UnitOfWork;

        public LabTestingIndexService(IRepositoryHelper repositoryHelper)
        {
            RepositoryHelper = repositoryHelper;
            UnitOfWork = RepositoryHelper.GetUnitOfWork();
        }
        public List<LabTestingIndex> GetAllLabTestingIndexHaveLabtestingId(int labtestingId)
        {
            var repo = this.RepositoryHelper.GetRepository<ILabTestingIndexRepository>(UnitOfWork);
            var labTesting = repo.GetAllLabTestingIndexById(labtestingId);
            return labTesting;
        }
        public bool AddLabTestingIndex(List<LabTestingIndex> labTestingIndex)
        {
            var repo = RepositoryHelper.GetRepository<ILabTestingIndexRepository>(UnitOfWork);
            try
            {
                foreach (var item in labTestingIndex)
                {
                    repo.Create(item);
                }
                var result = UnitOfWork.SaveChanges();
                if (result.Any())
                {
                    return false;
                }

            }
            catch (Exception ex) { return false; }
            return true;
        }

        public List<LabTestingIndex> GetAllLabTestingIndex()
        {
            var repo = this.RepositoryHelper.GetRepository<ILabTestingIndexRepository>(UnitOfWork);
            var labTesting = repo.GetAllLabTestingIndex();
            return labTesting;
        }

        // DucBM
        public ResponseObjectDto AddLabTestingIndexes(List<LabTestingIndex> labTestingIndexes)
        {
            var respObj = new ResponseObjectDto();
            respObj.Success = true;
            respObj.Message = "Thêm các chỉ số thành công.";
            respObj.Data = null;
            var repo = RepositoryHelper.GetRepository<ILabTestingIndexRepository>(UnitOfWork);
            try
            {
                foreach (var item in labTestingIndexes)
                {
                    repo.Create(item);
                }
                var result = UnitOfWork.SaveChanges();
                if (result.Any())
                {
                    respObj.Success = false;
                    respObj.Message = "Có lỗi xảy ra";
                    respObj.Data = result;
                }

            }
            catch (Exception ex) {
                respObj.Success = false;
                respObj.Message = "Có lỗi xảy ra";
                respObj.Data = ex;
            }
            return respObj;
        }
    }
}
