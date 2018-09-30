namespace eLTMS.DataAccess
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using eLTMS.DataAccess.Models;
    using System.Reflection;
    using System.Data.Entity.ModelConfiguration;

    public partial class FinalProjectContext : DbContext
    {
        public FinalProjectContext()
            : base("name=FinalProjectContext")
        {
        }
        public FinalProjectContext(String connectionString) : base(connectionString)
        {

        }
        public System.Data.Entity.DbSet<Account> Accounts { get; set; } // Account
        public System.Data.Entity.DbSet<Employee> Employees { get; set; } // Employee
        public System.Data.Entity.DbSet<ExportPaper> ExportPapers { get; set; } // ExportPaper
        public System.Data.Entity.DbSet<ExportProposurePaper> ExportProposurePapers { get; set; } // ExportProposurePaper
        public System.Data.Entity.DbSet<ExportProposurePaperDetail> ExportProposurePaperDetails { get; set; } // ExportProposurePaperDetail
        public System.Data.Entity.DbSet<Faculty> Faculties { get; set; } // Faculty
        public System.Data.Entity.DbSet<Feedback> Feedbacks { get; set; } // Feedback
        public System.Data.Entity.DbSet<Hospital> Hospitals { get; set; } // Hospital
        public System.Data.Entity.DbSet<HospitalFacultyMapping> HospitalFacultyMappings { get; set; } // HospitalFacultyMapping
        public System.Data.Entity.DbSet<HospitalSuggesting> HospitalSuggestings { get; set; } // HospitalSuggesting
        public System.Data.Entity.DbSet<ImportPaper> ImportPapers { get; set; } // ImportPaper
        public System.Data.Entity.DbSet<ImportPaperDetail> ImportPaperDetails { get; set; } // ImportPaperDetail
        public System.Data.Entity.DbSet<LabTest> LabTests { get; set; } // LabTest
        public System.Data.Entity.DbSet<LabTestSampleMapping> LabTestSampleMappings { get; set; } // LabTestSampleMapping
        public System.Data.Entity.DbSet<Patient> Patients { get; set; } // Patient
        public System.Data.Entity.DbSet<ResultIndex> ResultIndexes { get; set; } // ResultIndex
        public System.Data.Entity.DbSet<ResultPaper> ResultPapers { get; set; } // ResultPaper
        public System.Data.Entity.DbSet<Sample> Samples { get; set; } // Sample
        public System.Data.Entity.DbSet<Supply> Supplies { get; set; } // Supply
        public System.Data.Entity.DbSet<SupplyType> SupplyTypes { get; set; } // SupplyType
        public System.Data.Entity.DbSet<Testing> Testings { get; set; } // Testing
        public System.Data.Entity.DbSet<TestProfile> TestProfiles { get; set; } // TestProfile
        public System.Data.Entity.DbSet<TestProfileLabTestMapping> TestProfileLabTestMappings { get; set; } // TestProfileLabTestMapping

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            var typesToRegister = Assembly.GetExecutingAssembly().GetTypes()
         .Where(type => !string.IsNullOrEmpty(type.Namespace))
         .Where(type => type.BaseType != null && type.BaseType.IsGenericType
              && type.BaseType.GetGenericTypeDefinition() == typeof(EntityTypeConfiguration<>));
            foreach (var type in typesToRegister)
            {
                dynamic configurationInstance = Activator.CreateInstance(type);
                modelBuilder.Configurations.Add(configurationInstance);
            }
            base.OnModelCreating(modelBuilder);
        }
    }
    }

