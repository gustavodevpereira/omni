using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Products.CreateProduct;
using Ambev.DeveloperEvaluation.Application.Products.GetProduct;
using Ambev.DeveloperEvaluation.Application.Products.GetAllProducts;
using Ambev.DeveloperEvaluation.Application.Products.UpdateProduct;
using Ambev.DeveloperEvaluation.Application.Products.DeleteProduct;
using Ambev.DeveloperEvaluation.WebApi.Features.Product.CreateProduct;
using Ambev.DeveloperEvaluation.WebApi.Features.Product.GetProduct;
using Ambev.DeveloperEvaluation.WebApi.Features.Product.GetAllProducts;
using Ambev.DeveloperEvaluation.WebApi.Features.Product.UpdateProduct;
using Ambev.DeveloperEvaluation.WebApi.Features.Product.DeleteProduct;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Product.Mappings;

/// <summary>
/// Profile for mapping between product DTOs and domain entities
/// </summary>
public class ProductMappingProfile : Profile
{
    /// <summary>
    /// Initializes mapping configurations for products
    /// </summary>
    public ProductMappingProfile()
    {
        // Create Product mappings
        CreateMap<CreateProductRequest, CreateProductCommand>();
        CreateMap<CreateProductResult, CreateProductResponse>();

        // Get Product mappings
        CreateMap<Guid, GetProductQuery>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src));
        CreateMap<GetProductResult, GetProductResponse>();

        // Get All Products mappings
        CreateMap<GetAllProductsResult, GetAllProductsItemResponse>();

        // Update Product mappings
        CreateMap<UpdateProductRequest, UpdateProductCommand>();
        CreateMap<UpdateProductResult, UpdateProductResponse>();

        // Delete Product mappings
        CreateMap<Guid, DeleteProductCommand>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src));
        CreateMap<bool, DeleteProductResponse>()
            .ForMember(dest => dest.Success, opt => opt.MapFrom(src => src));
    }
}
