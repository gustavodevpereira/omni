using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Entities.Products;
using Ambev.DeveloperEvaluation.Application.Products.Common.Results;
using Ambev.DeveloperEvaluation.Application.Products.UseCases.GetProducts;

namespace Ambev.DeveloperEvaluation.Application.Products.Common.Mappings;

/// <summary>
/// AutoMapper profile for product-related mappings.
/// Defines mapping configurations between domain entities and DTOs for products.
/// </summary>
public class ProductMappingProfile : Profile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ProductMappingProfile"/> class.
    /// Sets up all mappings between product domain entities and DTOs.
    /// </summary>
    public ProductMappingProfile()
    {
        // Domain to DTO mappings
        CreateMap<Product, ProductResult>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.Sku, opt => opt.MapFrom(src => src.Sku))
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
            .ForMember(dest => dest.StockQuantity, opt => opt.MapFrom(src => src.StockQuantity))
            .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));
    }
} 