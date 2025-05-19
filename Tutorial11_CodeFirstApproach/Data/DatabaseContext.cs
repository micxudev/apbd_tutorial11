using Microsoft.EntityFrameworkCore;
using Tutorial5.Models;

namespace Tutorial5.Data;

public class DatabaseContext : DbContext
{
    public DbSet<Patient> Patients { get; set; }
    public DbSet<Doctor> Doctors { get; set; }
    public DbSet<Prescription> Prescriptions { get; set; }
    public DbSet<Medicament> Medicaments { get; set; }
    public DbSet<PrescriptionMedicament> PrescriptionMedicaments { get; set; }

    protected DatabaseContext()
    {
    }

    public DatabaseContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Patient>().HasData(new List<Patient>
        {
            new()
            {
                IdPatient = 1, FirstName = "JohnPatient", LastName = "DoePatient", BirthDate = new DateOnly(1990, 1, 1)
            },
            new()
            {
                IdPatient = 2, FirstName = "JanePatient", LastName = "DoePatient", BirthDate = new DateOnly(1990, 1, 1)
            }
        });

        modelBuilder.Entity<Doctor>().HasData(new List<Doctor>
        {
            new() { IdDoctor = 1, FirstName = "JohnDoctor", LastName = "DoeDoctor", Email = "JohnDoctor@email.com" },
            new() { IdDoctor = 2, FirstName = "JaneDoctor", LastName = "DoeDoctor", Email = "JaneDoctor@email.com" }
        });

        modelBuilder.Entity<Prescription>().HasData(new List<Prescription>
        {
            new()
            {
                IdPrescription = 1, Date = new DateOnly(2025, 05, 20), DueDate = new DateOnly(2025, 05, 20),
                IdPatient = 1, IdDoctor = 2
            },
            new()
            {
                IdPrescription = 2, Date = new DateOnly(2025, 05, 20), DueDate = new DateOnly(2025, 05, 20),
                IdPatient = 2, IdDoctor = 1
            }
        });

        modelBuilder.Entity<Medicament>().HasData(new List<Medicament>
        {
            new() { IdMedicament = 1, Name = "Med1", Description = "Desc1", Type = "Type1" },
            new() { IdMedicament = 2, Name = "Med2", Description = "Desc2", Type = "Type2" }
        });

        modelBuilder.Entity<PrescriptionMedicament>().HasData(new List<PrescriptionMedicament>
        {
            new() { IdPrescription = 1, IdMedicament = 1, Dose = 10, Details = "Details1" },
            new() { IdPrescription = 1, IdMedicament = 2, Dose = 20, Details = "Details2" },
            new() { IdPrescription = 2, IdMedicament = 1, Dose = 30, Details = "Details3" },
            new() { IdPrescription = 2, IdMedicament = 2, Dose = 40, Details = "Details4" }
        });
    }
}