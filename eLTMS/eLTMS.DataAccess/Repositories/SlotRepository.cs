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

    public interface ISlotRepository : IRepository<Slot>
    {
        List<Slot> GetByDateAndSampleGroupId(DateTime date, int sampleId);
        List<Slot> GetAvailableSlots();
        List<Slot> GetAllSlot();
    }
    public class SlotRepository : RepositoryBase<Slot>, ISlotRepository
    {
        public List<Slot> GetByDateAndSampleGroupId(DateTime date, int sampleGroupId)
        {
            /*var result = DbSet.AsQueryable()
                .Where(x => x.SampleGroup.SampleGroupId == sampleGroupId && x.Date == date)
                .ToList();
            return result;*/
            return null;
        }
        public List<Slot> GetAvailableSlots() 
        {
            /* var now = DateTime.Now;
             var today = DateTime.Now.Date;
             int secondNow = now.Hour * 60 * 60 + now.Minute * 60 + now.Second;

             var tmp = DbSet.AsQueryable()
                 //.Where(x => x.Date > today || (x.Date == today && x.StartTime > secondNow))
                 .Include(x => x.SampleGettings)
                 .ToList();
             var result = new List<Slot>();
             foreach (var item in tmp)
             {
                 var qty = item.Quantity;
                 var booked = item.SampleGettings.Count();
                 var available = qty - booked;
                 if (available > 0)
                 {
                     result.Add(item);
                 }
             } kkhoan ô
             return result;*/ 
            return null;
        }
        public List<Slot> GetAllSlot()
        {
            var result = DbSet.AsQueryable()
                .ToList();
            return result;
        }
    }
}