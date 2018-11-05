using eLTMS.DataAccess.Infrastructure;
using eLTMS.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLTMS.DataAccess.Repositories
{

    public interface ISlotRepository : IRepository<Slot>
    {
        List<Slot> GetAllSlot();
    }
    public class SlotRepository : RepositoryBase<Slot>, ISlotRepository
    {
        public List<Slot> GetAllSlot()
        {
            var result = DbSet.AsQueryable()
                .ToList();
            return result;
        }
        
    }
}
