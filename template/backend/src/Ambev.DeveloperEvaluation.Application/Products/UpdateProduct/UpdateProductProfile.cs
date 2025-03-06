using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Entities.Products;

namespace Ambev.DeveloperEvaluation.Application.Products.UpdateProduct;

/// <summary>
/// AutoMapper profile for mapping between UpdateProductCommand, Product entity, and UpdateProductResult.
/// </summary>
public class UpdateProductProfile : Profile
{
    /// <summary>
    /// Initializes a new instance of the UpdateProductProfile class.
    /// </summary>
    public UpdateProductProfile()
    {
        CreateMap<Product, UpdateProductResult>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));
    }
} 