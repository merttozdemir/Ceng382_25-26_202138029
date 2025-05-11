using Microsoft.EntityFrameworkCore;
using LabFormProject.Models;

namespace LabFormProject.Data
{
    public class SchoolDbContext : DbContext
    {
        public SchoolDbContext(DbContextOptions<SchoolDbContext> options) : base(options) { }

        public DbSet<ClassInformationModel> ClassInformationTable { get; set; }
    }
}
