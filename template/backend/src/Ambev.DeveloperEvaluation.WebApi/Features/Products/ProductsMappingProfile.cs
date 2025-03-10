using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Products.Common.Results;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.GetProducts;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products;

/// <summary>
/// AutoMapper profile for product-related mappings.
/// Defines mapping configurations between domain entities, application DTOs and API models.
/// </summary>
public class ProductsMappingProfile : Profile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ProductsMappingProfile"/> class
    /// with all product-related mapping configurations.
    /// </summary>
    public ProductsMappingProfile()
    {
        // GetProducts mappings
        CreateMap<ProductResult, GetProductsResponse>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.Sku, opt => opt.MapFrom(src => src.Sku))
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
            .ForMember(dest => dest.StockQuantity, opt => opt.MapFrom(src => src.StockQuantity))
            .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category))
            .ForMember(dest => dest.BranchExternalId, opt => opt.MapFrom(src => src.BranchExternalId))
            .ForMember(dest => dest.BranchName, opt => opt.MapFrom(src => src.BranchName))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt));
            
        // Additional product-related mappings can be added here
    }
} 