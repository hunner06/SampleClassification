using Microsoft.EntityFrameworkCore;
using SampleClassification.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleClassification.Data
{
   public class ClassificationDataContext : DbContext
    {
        public static string _connectionString = ".;Database=MLClassification;Trusted_Connection=True;MultipleActiveResultSets=true";

        public DbSet<ModelInput> ModelInput { get; set; }

        public ClassificationDataContext()
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlServer($"Server={_connectionString}");
    }
}
