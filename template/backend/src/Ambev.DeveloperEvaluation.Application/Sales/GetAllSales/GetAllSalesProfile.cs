using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Entities.Sales;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetAllSales;

/// <summary>
/// AutoMapper profile for mapping between Sale entity and GetAllSalesResult.SaleDto
/// </summary>
public class GetAllSalesProfile : Profile
{
    /// <summary>
    /// Initializes a new instance of GetAllSalesProfile
    /// </summary>
    public GetAllSalesProfile()
    {
        CreateMap<Sale, GetAllSalesResult.SaleDto>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
            .ForMember(dest => dest.ItemCount, opt => opt.MapFrom(src => src.Items.Count));
    }
} 