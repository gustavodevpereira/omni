using AutoMapper;
using MediatR;
using FluentValidation;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Entities.Sales;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale
{
    /// <summary>
    /// Handler for processing CreateSaleCommand requests.
    /// </summary>
    public class CreateSaleHandler : IRequestHandler<CreateSaleCommand, CreateSaleResult>
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _uow;

        public CreateSaleHandler(ISaleRepository saleRepository, IMapper mapper, IUnitOfWork uow)
        {
            _saleRepository = saleRepository;
            _mapper = mapper;
            _uow = uow;
        }

        public async Task<CreateSaleResult> Handle(CreateSaleCommand command, CancellationToken cancellationToken)
        {
            // Validação do comando
            var validator = new CreateSaleValidator();
            var validationResult = await validator.ValidateAsync(command, cancellationToken);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            // Criação do agregado Sale
            var sale = _mapper.Map<Sale>(command);

            // Adiciona os itens da venda
            foreach (var item in command.Items)
            {
                sale.AddItem(item.ProductExternalId, item.ProductName, item.Quantity, item.UnitPrice);
            }

            // Persistência da venda via repositório
            var createdSale = await _saleRepository.CreateAsync(sale, cancellationToken);
            await _uow.CommitAsync(cancellationToken);

            // Mapeia a entidade para o resultado
            var result = _mapper.Map<CreateSaleResult>(createdSale);
            return result;
        }
    }
}
