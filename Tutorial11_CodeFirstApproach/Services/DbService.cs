using Microsoft.EntityFrameworkCore;
using Tutorial5.Data;
using Tutorial5.Exceptions;
using Tutorial5.Models;
using Tutorial5.Models.DTOs;

namespace Tutorial5.Services;

public class DbService(DatabaseContext context) : IDbService
{
    public async Task CreatePrescriptionAsync(CreatePrescriptionRequestDto requestDto)
    {
        await using var transaction = await context.Database.BeginTransactionAsync();

        try
        {
            // 1. if a patient does not exist, insert it
            var patient = await context.Patients.FirstOrDefaultAsync(p => p.IdPatient == requestDto.Patient.IdPatient);
            if (patient == null)
            {
                patient = new Patient
                {
                    FirstName = requestDto.Patient.FirstName,
                    LastName = requestDto.Patient.LastName,
                    BirthDate = requestDto.Patient.BirthDate
                };
                context.Patients.Add(patient);
                await context.SaveChangesAsync();
            }

            // 2. if a doctor does not exist, throw an exception and rollback
            var doctor = await context.Doctors.FirstOrDefaultAsync(d => d.IdDoctor == requestDto.Doctor.IdDoctor);
            if (doctor == null)
                throw new NotFoundException($"Doctor with id {requestDto.Doctor.IdDoctor} not found");

            // 3. if at least one medicament does not exist, throw an exception and rollback
            var medicaments = new List<Medicament>();
            foreach (var medDto in requestDto.Medicaments)
            {
                var medicament =
                    await context.Medicaments.FirstOrDefaultAsync(m => m.IdMedicament == medDto.IdMedicament);
                if (medicament == null)
                    throw new NotFoundException($"Medicament with id {medDto.IdMedicament} not found");
                medicaments.Add(medicament);
            }

            // 4. insert a prescription
            var prescription = new Prescription
            {
                Date = requestDto.Date,
                DueDate = requestDto.DueDate,
                IdPatient = patient.IdPatient,
                IdDoctor = doctor.IdDoctor
            };
            context.Prescriptions.Add(prescription);
            await context.SaveChangesAsync();

            // 5. for each medicament in the request, insert prescription-medicament with the given details
            foreach (var (medicament, medDto) in medicaments.Zip(requestDto.Medicaments))
                context.PrescriptionMedicaments.Add(new PrescriptionMedicament
                {
                    IdMedicament = medicament.IdMedicament,
                    IdPrescription = prescription.IdPrescription,
                    Dose = medDto.Dose,
                    Details = medDto.Details
                });

            await context.SaveChangesAsync();

            await transaction.CommitAsync();
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<PatientDto> GetPatientDataByIdAsync(int id)
    {
        var patientData = await context.Patients.Include(patient => patient.Prescriptions)
            .ThenInclude(prescription => prescription.Doctor).Include(patient => patient.Prescriptions)
            .ThenInclude(prescription => prescription.PrescriptionMedicaments)
            .ThenInclude(prescriptionMedicament => prescriptionMedicament.Medicament)
            .FirstOrDefaultAsync(p => p.IdPatient == id);

        if (patientData == null)
            throw new NotFoundException($"Patient with id {id} not found");

        var patientDto = new PatientDto
        {
            IdPatient = patientData.IdPatient,
            FirstName = patientData.FirstName,
            LastName = patientData.LastName,
            BirthDate = patientData.BirthDate,
            Prescriptions = patientData.Prescriptions.Select(pr => new PrescriptionDto
            {
                IdPrescription = pr.IdPrescription,
                Date = pr.Date,
                DueDate = pr.DueDate,
                Doctor = new DoctorDto
                {
                    IdDoctor = pr.Doctor.IdDoctor,
                    FirstName = pr.Doctor.FirstName,
                    LastName = pr.Doctor.LastName
                },
                Medicaments = pr.PrescriptionMedicaments.Select(pm => new MedicamentDto
                {
                    IdMedicament = pm.IdMedicament,
                    Name = pm.Medicament.Name,
                    Dose = pm.Dose,
                    Details = pm.Details
                }).ToList()
            }).ToList()
        };

        return patientDto;
    }
}