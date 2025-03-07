using Ambev.DeveloperEvaluation.Application.Carts.UseCases.UpdateCart;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Cart.Actions.UpdateCart
{
    /// <summary>
    /// AutoMapper profile for AddCart feature
    /// </summary>
    public class UpdateCartProfile : Profile
    {
        /// <summary>
        /// Initializes mapping configurations
        /// </summary>
        public UpdateCartProfile()
        {
            CreateMap<UpdateCartRequest, UpdateCartCommand>();
            CreateMap<UpdateCartProductRequest, UpdateCartProductCommand>();
        }

    }
}
