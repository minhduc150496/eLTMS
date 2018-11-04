﻿using eLTMS.DataAccess.Infrastructure;
using eLTMS.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace eLTMS.DataAccess.Repositories
{
    public interface ILabTestingIndexRepository : IRepository<LabTestingIndex>
    {
        List<LabTestingIndex> GetAllLabTestingIndex();
        LabTestingIndex GetLabTestingIndexById(int id);
        List<LabTestingIndex> GetAllLabTestingIndexById(int id);
    }
    public class LabTestingIndexRepository : RepositoryBase<LabTestingIndex>, ILabTestingIndexRepository
    {
        public List<LabTestingIndex> GetAllLabTestingIndex()
        {
            var result = DbSet.AsQueryable()
                .Include(x => x.LabTesting)
                .ToList();
            return result;
        }
      

        public LabTestingIndex GetLabTestingIndexById(int id)
        {
            var result = DbSet.Where(s => s.LabTestingId == id).ToList().FirstOrDefault();
            return result;
        }
        public List<LabTestingIndex> GetAllLabTestingIndexById(int id)
        {

            var result = DbSet.AsQueryable()
             .Where(x => x.LabTestingId==id)
            
             .ToList();
            return result;

        }

    }
}
