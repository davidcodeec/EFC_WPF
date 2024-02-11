using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Contexts;

public class EmployeeDbContext(DbContextOptions<EmployeeDbContext> options) : DbContext(options)
{
    public virtual DbSet<AddressEntity> Addresses { get; set; }
    public virtual DbSet<DepartmentEntity> Departments { get; set; }
    public virtual DbSet<EmployeeAddressEntity> EmployeeAddresses { get; set; }
    public virtual DbSet<EmployeeEntity> Employees { get; set; }
    public virtual DbSet<EmployeePhoneNumberEntity> EmployeePhoneNumbers { get; set; }
    public virtual DbSet<PositionEntity> Positions { get; set; }
    public virtual DbSet<SalaryEntity> Salaries { get; set; }
    public virtual DbSet<SkillEntity> Skills { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<EmployeePhoneNumberEntity>()
            .HasIndex(x => x.PhoneNumber)
            .IsUnique()
            .HasDatabaseName("IX_EmployeePhoneNumber");

        modelBuilder.Entity<EmployeeEntity>()
            .HasIndex(x => x.Email)
            .IsUnique()
            .HasDatabaseName("IX_EmployeeEmail");

        modelBuilder.Entity<EmployeeAddressEntity>()
            .HasIndex(x => new { x.EmployeeId, x.AddressId })
            .IsUnique()
            .HasDatabaseName("IX_EmployeeAddress");

        modelBuilder.Entity<DepartmentEntity>()
           .HasMany(d => d.Employees)
           .WithOne(e => e.Department)
           .HasForeignKey(e => e.DepartmentId);

      
        modelBuilder.Entity<PositionEntity>()
            .HasMany(p => p.Employees)
            .WithOne(e => e.Position)
            .HasForeignKey(e => e.PositionId);


        modelBuilder.Entity<EmployeeEntity>()
             .HasOne(e => e.Skill)
             .WithMany(s => s.Employees)
             .HasForeignKey(e => e.SkillId);


        modelBuilder.Entity<SalaryEntity>()
            .HasMany(s => s.Employees)
            .WithOne(e => e.Salary)
            .HasForeignKey(e => e.SalaryId);

        
        modelBuilder.Entity<EmployeeAddressEntity>()
            .HasOne(ea => ea.Employee)
            .WithMany(e => e.EmployeeAddresses)
            .HasForeignKey(ea => ea.EmployeeId);

        
        modelBuilder.Entity<EmployeePhoneNumberEntity>()
            .HasOne(pn => pn.Employee)
            .WithMany(e => e.EmployeePhoneNumbers)
            .HasForeignKey(pn => pn.EmployeeId);


    }
}
