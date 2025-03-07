using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Carts.Common.Results;
using Ambev.DeveloperEvaluation.Domain.Entities.Carts;

namespace Ambev.DeveloperEvaluation.Application.Carts.Common.Profiles;

/// <summary>
/// AutoMapper profile for mapping between Cart entity and CartResult
/// </summary>
public class CartResultProfile : Profile
{
    public CartResultProfile()
    {
        CreateMap<Cart, CartResult>();
    }
}