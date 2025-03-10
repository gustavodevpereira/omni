using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Carts.Common.Results;
using Ambev.DeveloperEvaluation.Application.Carts.UseCases.AddCart;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.AddCart;

/// <summary>
/// Profile for mapping between API requests/responses and application commands/results for AddCart feature
/// </summary>
public class AddCartProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for AddCart feature
    /// </summary>
    public AddCartProfile()
    {
        // Map from API request to application command
        CreateMap<AddCartRequest, AddCartCommand>()
            .ForMember(dest => dest.CostumerId, opt => opt.Ignore()) // Will be set from JWT token
            .ForMember(dest => dest.CostumerName, opt => opt.Ignore()) // Will be set from JWT token
            .ForMember(dest => dest.BranchId, opt => opt.MapFrom(src => src.BranchId))
            .ForMember(dest => dest.BranchName, opt => opt.MapFrom(src => src.BranchName))
            .ForMember(dest => dest.CreatedOn, opt => opt.MapFrom(src => DateTime.UtcNow));

        CreateMap<AddCartProductRequest, AddCartProductCommand>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ProductId))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.ProductName));

        // Map from application result to API response
        CreateMap<CartResult, AddCartResponse>();
        CreateMap<CartProductResult, AddCartProductResponse>();
    }
} 