using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Carts.UseCases.GetCarts;
using Ambev.DeveloperEvaluation.WebApi.Features.Cart.Actions.GetCarts;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Cart.Common;

/// <summary>
/// AutoMapper profile for GetCarts feature
/// </summary>
public class GetCartsProfile : Profile
{
    /// <summary>
    /// Initializes mapping configurations
    /// </summary>
    public GetCartsProfile()
    {
        CreateMap<GetCartsRequest, GetCartsCommand>()
            .ForMember(dest => dest.PageNumber, opt => opt.MapFrom(src => src.PageNumber))
            .ForMember(dest => dest.PageSize, opt => opt.MapFrom(src => src.PageSize));
    }
}