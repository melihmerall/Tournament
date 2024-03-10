using Microsoft.EntityFrameworkCore;
using Tournament.Data.Entities;

namespace Tournament.Data.Context
{
    public class ApplicationContext: DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
        }

        // DbSet'leri ekleyin, varsa sınıf isimlerini değiştirin
        public DbSet<TeamApplicationForm> TeamApplicationForms { get; set; }
        public DbSet<TeamMember> TeamMembers { get; set; }
        public DbSet<FileDocument> FileDocuments { get; set; }

    }
}
