using AutoMapper;
using AutoMapper.QueryableExtensions;
using HCSYS.Core.Constants;
using HCSYS.Core.Exceptions;
using HCSYS.Core.Models;
using HCSYS.Core.Services.Contracts;
using HCSYS.Persistence;
using HCSYS.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

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

    public Task<IQueryable<PatientDto>> SearchAsync(SearchPatientsRequest request)
    {
        var birthDateFilters = new Dictionary<string, HashSet<string>>();

        try
        {
            birthDateFilters = ParseBirthDateFilters(request.BirthDateFilters);
    }
        catch (Exception exception) 
        when (exception is InvalidOperationException || exception is ArgumentOutOfRangeException)
        {
            throw new UnprocessableEntityException(nameof(request.BirthDateFilters), "Value doesn't match to the pattern.");
        }

        string filterQuery = CreateBirthDateFilterQuery(birthDateFilters);
        IQueryable<PatientDto> patients = _dataContext.Patients
            .AsNoTracking()
            .Where(filterQuery, birthDateFilters.Keys.ToArray())
            .ProjectTo<PatientDto>(_mapper.ConfigurationProvider);

        return Task.FromResult(patients);
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

    private static Dictionary<string, HashSet<string>> ParseBirthDateFilters(IEnumerable<string> birthDateFilters)
    {
        var filters = new Dictionary<string, HashSet<string>>();

        foreach (string birthDateFilter in birthDateFilters)
        {
            string prefix = birthDateFilter.Substring(0, 2);
            string inputDate = birthDateFilter.Substring(2);

            bool isValid = DateTimeOffset.TryParse(inputDate, out DateTimeOffset outputDate)
                && PatientConstants.BirthDatePrefixes.ContainsKey(prefix);
            if (!isValid)
            {
                throw new InvalidOperationException();
            }

            if (filters.TryGetValue(inputDate, out HashSet<string>? prefixes))
            {
                prefixes.Add(prefix);
            }
            else
            {
                filters.Add(inputDate, new HashSet<string> { prefix });
            }
        }

        return filters;
    }

    private static string CreateBirthDateFilterQuery(Dictionary<string, HashSet<string>> birthDateFilters)
    {
        var valueIndex = 0;
        var filterQueryCases = new List<string>();

        foreach (var filter in birthDateFilters)
        {
            foreach (var prefix in filter.Value)
            {
                PatientConstants.BirthDatePrefixes.TryGetValue(prefix, out string? comparisonOperator);
                if (comparisonOperator is not null)
                {
                    string filterQueryCase = $"BirthDate {comparisonOperator} @{valueIndex}";
                    filterQueryCases.Add(filterQueryCase);
                }
            }

            valueIndex++;
        }

        return string.Join(" AND ", filterQueryCases);
    }
}
