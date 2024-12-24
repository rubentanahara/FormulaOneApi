using AutoMapper;
using FormulaOne.Entities.DbSet;
using FormulaOne.Entities.Dtos.Requests;

namespace FormulaOne.Api.MappingProfiles;

public class RequestToDomain : Profile
{
    public RequestToDomain()
    {
        CreateMap<CreateDriverAchievementRequest, Achievement>()
          .ForMember(dest => dest.Status, opt => opt.MapFrom(src => 1))
          .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
          .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));

        CreateMap<UpdateDriverAchievementRequest, Achievement>()
          .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));

        CreateMap<CreateDriverRequest, Driver>()
          .ForMember(dest => dest.Status, opt => opt.MapFrom(src => 1))
          .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
          .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));

        CreateMap<UpdateDriverRequest, Driver>()
          .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));
    }
}
