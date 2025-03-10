using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Entities.Carts;
using Ambev.DeveloperEvaluation.Application.Carts.Common.Results;
using Ambev.DeveloperEvaluation.Application.Carts.UseCases.GetCarts;

namespace Ambev.DeveloperEvaluation.Application.Carts.Common.Mappings;

/// <summary>
/// AutoMapper profile for cart-related mappings.
/// Defines mapping configurations between domain entities and DTOs for carts.
/// </summary>
public class CartMappingProfile : Profile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CartMappingProfile"/> class.
    /// Sets up all mappings between cart domain entities and DTOs.
    /// </summary>
    public CartMappingProfile()
    {
        // Domain to DTO mappings
        CreateMap<Cart, CartResult>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.CreatedOn, opt => opt.MapFrom(src => src.CreatedOn))
            .ForMember(dest => dest.CustomerExternalId, opt => opt.MapFrom(src => src.CustomerExternalId))
            .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.CustomerName))
            .ForMember(dest => dest.BranchExternalId, opt => opt.MapFrom(src => src.BranchExternalId))
            .ForMember(dest => dest.BranchName, opt => opt.MapFrom(src => src.BranchName))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
            .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.TotalAmount))
            .ForMember(dest => dest.Products, opt => opt.MapFrom(src => src.Products));

        CreateMap<CartProduct, CartProductResult>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.ProductExternalId, opt => opt.MapFrom(src => src.ProductExternalId))
            .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.ProductName))
            .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
            .ForMember(dest => dest.UnitPrice, opt => opt.MapFrom(src => src.UnitPrice))
            .ForMember(dest => dest.DiscountPercentage, opt => opt.MapFrom(src => src.DiscountPercentage))
            .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.TotalAmount));
    }
} 