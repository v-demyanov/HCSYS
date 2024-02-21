using AutoMapper;
using HCSYS.Core.Models;
using HCSYS.Persistence.Entities;

namespace HCSYS.Core.Configuration;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CreatePatientRequest, Patient>()
            .ForMember(dest => dest.Name, options => options.MapFrom(source => new PatientName
            {
                Use = source.Use,
                Family = source.Family,
                Given = source.Given,
            }));

        CreateMap<UpdatePatientRequest, Patient>()
            .ForPath(dest => dest.Name.Use, options => options.MapFrom(source => source.Use))
            .ForPath(dest => dest.Name.Family, options => options.MapFrom(source => source.Family))
            .ForPath(dest => dest.Name.Given, options => options.MapFrom(source => source.Given));

        CreateMap<Patient, PatientDto>()
            .ForMember(dest => dest.Use, options => options.MapFrom(source => source.Name.Use))
            .ForMember(dest => dest.Family, options => options.MapFrom(source => source.Name.Family))
            .ForMember(dest => dest.Given, options => options.MapFrom(source => source.Name.Given));
    }
}
