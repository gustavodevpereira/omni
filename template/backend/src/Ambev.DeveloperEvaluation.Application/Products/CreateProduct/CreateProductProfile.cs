using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Entities.Products;

namespace Ambev.DeveloperEvaluation.Application.Products.CreateProduct;

/// <summary>
/// AutoMapper profile for mapping between CreateProductCommand, Product entity, and CreateProductResult.
/// </summary>
public class CreateProductProfile : Profile
{
    /// <summary>
    /// Initializes a new instance of the CreateProductProfile class.
    /// </summary>
    public CreateProductProfile()
    {
        CreateMap<CreateProductCommand, Product>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.Status, opt => opt.Ignore());

        CreateMap<Product, CreateProductResult>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));
    }
} 