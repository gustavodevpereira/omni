using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Carts.Common.Results;
using Ambev.DeveloperEvaluation.Application.Carts.UseCases.CalculateCartDiscount;
using Ambev.DeveloperEvaluation.WebApi.Features.Carts.CalculateDiscount;
using Ambev.DeveloperEvaluation.WebApi.Features.Carts.GetCarts;
using Ambev.DeveloperEvaluation.WebApi.Features.Carts.AddCart;
using Ambev.DeveloperEvaluation.Application.Carts.UseCases.AddCart;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts;

/// <summary>
/// AutoMapper profile for cart-related mappings
/// </summary>
public class CartsMappingProfile : Profile
{
    /// <summary>
    /// Initializes a new instance of the CartsMappingProfile class
    /// </summary>
    public CartsMappingProfile()
    {
        // GetCarts mappings
        CreateMap<CartResult, GetCartsResponse>();
        CreateMap<CartProductResult, GetCartsProductResponse>();
        
        // AddCart mappings
        CreateMap<AddCartRequest, AddCartCommand>();
        CreateMap<AddCartProductRequest, AddCartProductCommand>();
        CreateMap<CartResult, AddCartResponse>();
        CreateMap<CartProductResult, AddCartProductResponse>();
        
        // CalculateCartDiscount mappings
        CreateMap<CalculateCartDiscountRequest, CalculateCartDiscountCommand>()
            .ForMember(dest => dest.CustomerId, opt => opt.Ignore())
            .ForMember(dest => dest.CustomerName, opt => opt.Ignore())
            .ForMember(dest => dest.CustomerEmail, opt => opt.Ignore());
        CreateMap<CalculateCartDiscountProductRequest, CalculateCartDiscountProductCommand>();
        CreateMap<CartResultWithDiscounts, CalculateCartDiscountResponse>();
        CreateMap<CartProductResultWithDiscount, CalculateCartDiscountProductResponse>();
    }
} 