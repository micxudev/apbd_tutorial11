using Tutorial5.Models.DTOs;

namespace Tutorial5.Services;

public interface IDbService
{
    Task CreatePrescriptionAsync(CreatePrescriptionRequestDto requestDto);

    Task<PatientDto> GetPatientDataByIdAsync(int id);
}