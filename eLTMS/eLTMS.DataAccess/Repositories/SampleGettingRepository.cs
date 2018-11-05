﻿using eLTMS.DataAccess.Infrastructure;
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
    public interface ISampleGettingRepository : IRepository<SampleGetting>
    {
        List<SampleGetting> GetAll();
        List<SampleGetting> GetAllIncludeApp();
        List<SampleGetting> GetAll2();
    }
    public class SampleGettingRepository : RepositoryBase<SampleGetting>, ISampleGettingRepository
    {
        public List<SampleGetting> GetAll()
        {
            var results = DbSet.AsQueryable()
                .ToList();
            return results;
        }
        public List<SampleGetting> GetAll2()
        {
            var results = DbSet.AsQueryable()
                .Include(p=>p.Slot)
                .ToList();
            return results;
        }
        public List<SampleGetting> GetAllIncludeApp()
        {
            var results = DbSet.AsQueryable()
                .Include(p => p.Appointment)
                .ToList();
            return results;
        }
    }
}
