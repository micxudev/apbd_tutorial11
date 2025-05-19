namespace Tutorial5.Models.DTOs;

public class PatientDto
{
    public int IdPatient { get; set; }

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public DateOnly BirthDate { get; set; }

    public List<PrescriptionDto> Prescriptions { get; set; } = [];
}

public class PrescriptionDto
{
    public int IdPrescription { get; set; }

    public DateOnly Date { get; set; }

    public DateOnly DueDate { get; set; }

    public DoctorDto Doctor { get; set; }

    public List<MedicamentDto> Medicaments { get; set; } = [];
}

public class DoctorDto
{
    public int IdDoctor { get; set; }

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;
}

public class MedicamentDto
{
    public int IdMedicament { get; set; }

    public string Name { get; set; } = string.Empty;

    public int? Dose { get; set; }

    public string Details { get; set; } = string.Empty;
}