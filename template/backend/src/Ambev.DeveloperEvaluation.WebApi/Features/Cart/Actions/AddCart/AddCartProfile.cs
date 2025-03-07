using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Carts.UseCases.AddCart;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Cart.Actions.AddCart;

/// <summary>
/// AutoMapper profile for AddCart feature
/// </summary>
public class AddCartProfile : Profile
{
    /// <summary>
    /// Initializes mapping configurations
    /// </summary>
    public AddCartProfile()
    {
        CreateMap<AddCartRequest, AddCartCommand>();
        CreateMap<AddCartProductRequest, AddCartProductCommand>();
    }
}