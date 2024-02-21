using AutoMapper;
using HCSYS.Core.Exceptions;
using HCSYS.Core.Models;
using HCSYS.Core.Services.Contracts;
using HCSYS.Persistence;
using HCSYS.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace HCSYS.Core.Services;

public class PatientsService : IPatientsService
{
    private readonly DataContext _dataContext;
    private readonly IMapper _mapper;

    public PatientsService(DataContext dataContext, IMapper mapper)
    {
        _dataContext = dataContext ?? throw new ArgumentNullException(nameof(dataContext));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public Task<PatientDto> CreateAsync(CreatePatientRequest request)
    {
        // TODO: Validate request

        Patient patientToAdd = _mapper.Map<Patient>(request);

        _dataContext.Patients.Add(patientToAdd);
        _dataContext.SaveChanges();

        PatientDto patientDto = _mapper.Map<PatientDto>(patientToAdd);

        return Task.FromResult(patientDto);
    }

    public Task<PatientDto> GetByIdAsync(Guid patientId)
    {
        Patient? patient = _dataContext.Patients
            .Include(x => x.Name)
            .FirstOrDefault(x => x.Id == patientId);
        if (patient is null)
        {
            throw new EntityNotFoundException(nameof(patient), patientId);
        }

        PatientDto patientDto = _mapper.Map<PatientDto>(patient);

        return Task.FromResult(patientDto);
    }

    public Task DeleteAsync(Guid patientId)
    {
        Patient? patientToDelete = _dataContext.Patients.FirstOrDefault(x => x.Id == patientId);
        if (patientToDelete is null)
        {
            throw new EntityNotFoundException(nameof(patientToDelete), patientId);
        }

        _dataContext.Patients.Remove(patientToDelete);
        _dataContext.SaveChanges();

        return Task.CompletedTask;
    }

    public Task UpdateAsync(UpdatePatientRequest request)
    {
        // TODO: Validate request

        Patient? patientToUpdate = _dataContext.Patients
            .Include(x => x.Name)
            .FirstOrDefault(x => x.Id == request.PatientToUpdateId);
        if (patientToUpdate is null)
        {
            throw new EntityNotFoundException(nameof(patientToUpdate), request.PatientToUpdateId);
        }

        _mapper.Map(request, patientToUpdate);

        _dataContext.Patients.Update(patientToUpdate);
        _dataContext.SaveChanges();

        return Task.CompletedTask;
    }
}
