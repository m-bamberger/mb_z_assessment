using AutoMapper;

namespace ZeissAssessment.API.Mappings;

public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        CreateMap<Models.Product, Models.DTOs.ProductDto>().ReverseMap();
        CreateMap<Models.DTOs.AddProductRequestDto, Models.Product>().ReverseMap();
    }
}