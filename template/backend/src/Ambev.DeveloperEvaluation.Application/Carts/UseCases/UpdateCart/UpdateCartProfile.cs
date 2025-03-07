using Ambev.DeveloperEvaluation.Domain.Entities.Carts;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Carts.UseCases.UpdateCart;

public class UpdateCartProfile : Profile
{
    public UpdateCartProfile()
    {
        CreateMap<UpdateCartCommand, Cart>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Products, opt => opt.Ignore());

        CreateMap<UpdateCartProductCommand, CartProduct>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());

        // Resultado da atualização
        CreateMap<Cart, UpdateCartResult>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));

        CreateMap<CartProduct, UpdateCartProductResult>();
    }
}
