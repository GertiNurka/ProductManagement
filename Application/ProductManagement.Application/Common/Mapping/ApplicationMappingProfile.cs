using AutoMapper;
using ProductManagement.Application.Common.DTOs;
using ProductManagement.Domain.Entities;

namespace ProductManagement.Application.Common.Mapping;

public class ApplicationMappingProfile : Profile
{
    public ApplicationMappingProfile()
    {
        CreateMap<Product, ProductDto>()
            .ForMember(dest => dest.QuantityStatusName, opt => opt.MapFrom(src => Enum.GetName(typeof(QuantityStatus), src.QuantityStatus)));
    }
}
