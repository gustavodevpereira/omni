using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Carts.UseCases.GetCart;
using Ambev.DeveloperEvaluation.Application.Carts.Common.Results;
using Ambev.DeveloperEvaluation.WebApi.Features.Cart.Actions.GetCart;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Cart.Common;

/// <summary>
/// AutoMapper profile for GetCart feature
/// </summary>
public class GetCartProfile : Profile
{
    /// <summary>
    /// Initializes mapping configurations
    /// </summary>
    public GetCartProfile()
    {
        CreateMap<GetCartRequest, GetCartCommand>();

        CreateMap<CartResult, GetCartResponse>()
            .ForMember(y => y.UserId, opt => opt.MapFrom(x => x.CustomerExternalId))
            .ForMember(y => y.Date, opt => opt.MapFrom(x => x.CreatedOn));

        CreateMap<CartProductResult, CartProductResponse>()
            .ForMember(y => y.ProductId, opt => opt.MapFrom(x => x.ProductExternalId));
    }
}