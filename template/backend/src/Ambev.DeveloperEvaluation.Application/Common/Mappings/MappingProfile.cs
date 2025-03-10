using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Products.Common.Results;
using Ambev.DeveloperEvaluation.Domain.Entities.Products;

namespace Ambev.DeveloperEvaluation.Application.Common.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, ProductResult>();
        }
    }
} 