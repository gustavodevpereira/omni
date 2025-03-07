using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Carts.Common.Results;
using Ambev.DeveloperEvaluation.Domain.Entities.Carts;

namespace Ambev.DeveloperEvaluation.Application.Carts.Common.Profiles;

/// <summary>
/// AutoMapper profile for mapping between CartProduct entity and CartProductResult
/// </summary>
public class CartProductResultProfile : Profile
{
    public CartProductResultProfile()
    {
        CreateMap<CartProduct, CartProductResult>();
    }
}
