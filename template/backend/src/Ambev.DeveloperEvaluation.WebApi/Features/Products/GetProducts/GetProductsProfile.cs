using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Products.Common.Results;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.GetProducts;

/// <summary>
/// Profile for mapping Product responses to API responses
/// </summary>
public class GetProductsProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for GetProducts feature
    /// </summary>
    public GetProductsProfile()
    {
        CreateMap<ProductResult, GetProductsResponse>();
    }
} 