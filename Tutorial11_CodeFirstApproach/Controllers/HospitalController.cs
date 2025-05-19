using Microsoft.AspNetCore.Mvc;
using Tutorial5.Exceptions;
using Tutorial5.Models.DTOs;
using Tutorial5.Services;

namespace Tutorial5.Controllers;

[Route("api/[controller]")]
[ApiController]
public class HospitalController(IDbService dbService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreatePrescription([FromBody] CreatePrescriptionRequestDto requestDto)
    {
        // Validate input data
        if (requestDto.Medicaments.Count is 0 or > 10)
            return BadRequest("Medicaments number must be between 1 and 10");

        if (requestDto.Date > requestDto.DueDate)
            return BadRequest("Date must be before DueDate");

        // Try to insert the given prescription
        try
        {
            await dbService.CreatePrescriptionAsync(requestDto);
            return Created();
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message); // Doctor or Medication does not exist
        }
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetPatientData(int id)
    {
        try
        {
            var res = await dbService.GetPatientDataByIdAsync(id);
            return Ok(res);
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message); // Patient does not exist
        }
    }
}