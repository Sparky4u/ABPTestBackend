using ABPTest.Models;
using Microsoft.EntityFrameworkCore;

namespace ABPTest.Data
{
    public class ExperimentContext : DbContext
    {
        public ExperimentContext(DbContextOptions<ExperimentContext> options) : base(options)
        {

        }
      
        public DbSet<ExperimentResult> ExperimentResults { get; set; }
    }
}
