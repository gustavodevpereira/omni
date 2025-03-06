using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Entities.Products;

namespace Ambev.DeveloperEvaluation.Application.Products.GetProduct;

/// <summary>
/// AutoMapper profile for mapping between Product entity and GetProductResult.
/// </summary>
public class GetProductProfile : Profile
{
    /// <summary>
    /// Initializes a new instance of the GetProductProfile class.
    /// </summary>
    public GetProductProfile()
    {
        CreateMap<Product, GetProductResult>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));
    }
} 