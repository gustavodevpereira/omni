using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Entities.Sales;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSaleByNumber;

/// <summary>
/// AutoMapper profile for mapping between Sale entity and GetSaleByNumberResult
/// </summary>
public class GetSaleByNumberProfile : Profile
{
    /// <summary>
    /// Initializes a new instance of GetSaleByNumberProfile
    /// </summary>
    public GetSaleByNumberProfile()
    {
        CreateMap<Sale, GetSaleByNumberResult>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
            .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));

        CreateMap<SaleItem, GetSaleByNumberResult.SaleItemDto>();
    }
} 