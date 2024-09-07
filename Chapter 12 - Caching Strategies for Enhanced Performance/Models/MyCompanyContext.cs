using Microsoft.EntityFrameworkCore;
namespace Chapter_9___Entity_Framework_Core_and_Dapper.Models
{
    public partial class MyCompanyContext : DbContext
    {
        public MyCompanyContext()
        {
        }
        public MyCompanyContext(DbContextOptions<MyCompanyContext> options)
            : base(options)
        {
        }
        public virtual DbSet<Employee> Employees { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263. 
            => optionsBuilder.UseSqlServer("YOURCONNECTIONSTRING");
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>(entity =>
            {
                entity.HasNoKey();
                entity.Property(e => e.Address).IsUnicode(false);
                entity.Property(e => e.City)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.Country)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.Name).IsUnicode(false);
                entity.Property(e => e.Phone)
                    .HasMaxLength(200)
                    .IsUnicode(false);
                entity.Property(e => e.Region)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.Salary).HasColumnType("decimal(10, 2)");
            });
            OnModelCreatingPartial(modelBuilder);
        }
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
