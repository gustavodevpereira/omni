using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;

/// <summary>
/// AutoMapper profile for Sales related mappings
/// </summary>
public class SalesProfile : Profile
{
    public SalesProfile()
    {
        CreateMap<CreateSaleRequest, CreateSaleCommand>();
        CreateMap<CreateSaleItemRequest, CreateSaleItemCommand>();
        
        CreateMap<CreateSaleResult, CreateSaleResponse>();
        CreateMap<CreateSaleResult.SaleItemDto, SaleItemResponse>();
    }
}