using AutoMapper;
using ProductSearcher.DTO.Request;
using ProductSearcher.Models.Internal;

namespace ProductSearcherApi.Profiles;

public class BaseAutomapperProfile: Profile
{
    public BaseAutomapperProfile()
    {
        CreateMap<ProductFilterRequest, ProductFilter>();
    }
}