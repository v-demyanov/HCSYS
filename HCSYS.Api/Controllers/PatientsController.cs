using HCSYS.Api.Models;
using HCSYS.Core.Models;
using HCSYS.Core.Services.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace HCSYS.Api.Controllers;

/// <summary>
/// Manages patients.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class PatientsController : ControllerBase
{
    private readonly IPatientsService _patientsService;

    /// <summary>
    /// Initializes a new instance of the <see cref="PatientsController"/> class.
    /// </summary>
    /// <param name="patientsService">The <see cref="IPatientsService"/> instance.</param>
    public PatientsController(IPatientsService patientsService)
    {
        _patientsService = patientsService ?? throw new ArgumentNullException(nameof(patientsService));
    }

    /// <summary>
    /// Creates a new patient.
    /// </summary>
    /// <param name="request">Data to create a new patient.</param>
    /// <returns>Data transfer object of created patient entity or expected errors.</returns>
    [HttpPost]
    [ProducesResponseType(typeof(PatientDto), StatusCodes.Status201Created)]
    public async Task<ActionResult<PatientDto>> Create([FromBody] CreatePatientRequest request)
    {
        PatientDto createdPatient = await _patientsService.CreateAsync(request);

        return StatusCode(StatusCodes.Status201Created, createdPatient);
    }

    /// <summary>
    /// Gets an existing patient.
    /// </summary>
    /// <param name="patientId">Patient's identifier.</param>
    /// <returns>Data transfer object of existing patient entity or expected errors.</returns>
    [HttpGet("{patientId}")]
    [ProducesResponseType(typeof(PatientDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PatientDto>> GetById(Guid patientId)
    {
        PatientDto patient = await _patientsService.GetByIdAsync(patientId);

        return StatusCode(StatusCodes.Status200OK, patient);
    }

    /// <summary>
    /// Deletes an existing patient.
    /// </summary>
    /// <param name="patientId">Patient's identifier.</param>
    /// <returns>Empty result or expected errors.</returns>
    [HttpDelete("{patientId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Delete(Guid patientId)
    {
        await _patientsService.DeleteAsync(patientId);

        return StatusCode(StatusCodes.Status204NoContent);
    }

    /// <summary>
    /// Updates an existing patient.
    /// </summary>
    /// <param name="request">Data to update an existing patient.</param>
    /// <returns>Empty result or expected errors.</returns>
    [HttpPut]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Update([FromBody] UpdatePatientRequest request)
    {
        await _patientsService.UpdateAsync(request);

        return StatusCode(StatusCodes.Status204NoContent);
    }

    /// <summary>
    /// Searches patients.
    /// </summary>
    /// <param name="request">Data to search for patients.</param>
    /// <returns>Data transfer objects of searched patient entities or expected errors.</returns>
    [HttpPost("search")]
    [ProducesResponseType(typeof(IEnumerable<PatientDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult<IEnumerable<PatientDto>>> Search([FromBody] SearchPatientsRequest request)
    {
        IEnumerable<PatientDto> patients = await _patientsService.SearchAsync(request);

        if (patients.Count() is 0)
        {
            return StatusCode(StatusCodes.Status204NoContent);
        }

        return StatusCode(StatusCodes.Status200OK, patients);
    }
}
