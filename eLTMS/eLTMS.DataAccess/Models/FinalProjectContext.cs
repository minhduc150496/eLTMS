namespace eLTMS.DataAccess.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class FinalProjectContext : DbContext
    {
        public FinalProjectContext()
            : base("name=FinalProjectContext")
        {
        }
        public FinalProjectContext(String connectionString) : base(connectionString)
        {

        }
        public virtual DbSet<Employee> Employees { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
