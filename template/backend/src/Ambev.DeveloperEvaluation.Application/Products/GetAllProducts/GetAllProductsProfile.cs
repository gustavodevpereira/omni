using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Entities.Products;

namespace Ambev.DeveloperEvaluation.Application.Products.GetAllProducts;

/// <summary>
/// AutoMapper profile for mapping between Product entity and GetAllProductsResult.
/// </summary>
public class GetAllProductsProfile : Profile
{
    /// <summary>
    /// Initializes a new instance of the GetAllProductsProfile class.
    /// </summary>
    public GetAllProductsProfile()
    {
        CreateMap<Product, GetAllProductsResult>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));
    }
} 