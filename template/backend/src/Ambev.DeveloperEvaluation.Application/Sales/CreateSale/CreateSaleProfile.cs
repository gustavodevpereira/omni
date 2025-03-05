using Ambev.DeveloperEvaluation.Domain.Entities.Sales;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale
{
    /// <summary>
    /// AutoMapper profile for mapping between CreateSaleCommand and Sale,
    /// and between Sale and CreateSaleResult.
    /// </summary>
    public class CreateSaleProfile : Profile
    {
        public CreateSaleProfile()
        {
            // Mapeia os parâmetros do comando para o construtor da entidade Sale
            CreateMap<CreateSaleCommand, Sale>()
                .ForCtorParam("saleNumber", opt => opt.MapFrom(src => src.SaleNumber))
                .ForCtorParam("saleDate", opt => opt.MapFrom(src => src.SaleDate))
                .ForCtorParam("customerExternalId", opt => opt.MapFrom(src => src.CustomerExternalId))
                .ForCtorParam("customerName", opt => opt.MapFrom(src => src.CustomerName))
                .ForCtorParam("branchExternalId", opt => opt.MapFrom(src => src.BranchExternalId))
                .ForCtorParam("branchName", opt => opt.MapFrom(src => src.BranchName));

            CreateMap<Sale, CreateSaleResult>();
        }
    }
}
