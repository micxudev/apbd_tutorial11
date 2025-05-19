namespace Tutorial5.Models.DTOs;

public class CreatePrescriptionRequestDto
{
    public PatientInputDto Patient { get; set; }

    public DoctorInputDto Doctor { get; set; }

    public DateOnly Date { get; set; }
    public DateOnly DueDate { get; set; }

    public List<MedicamentInputDto> Medicaments { get; set; } = [];
}

public class PatientInputDto
{
    public int IdPatient { get; set; }

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public DateOnly BirthDate { get; set; }
}

public class DoctorInputDto
{
    public int IdDoctor { get; set; }
}

public class MedicamentInputDto
{
    public int IdMedicament { get; set; }

    public int? Dose { get; set; }

    public string Details { get; set; } = string.Empty;
}