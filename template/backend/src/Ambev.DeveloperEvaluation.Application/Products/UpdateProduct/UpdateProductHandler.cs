using AutoMapper;
using MediatR;
using FluentValidation;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Application.Common.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Enums;

namespace Ambev.DeveloperEvaluation.Application.Products.UpdateProduct;

/// <summary>
/// Handler for processing UpdateProductCommand requests.
/// </summary>
public class UpdateProductHandler : IRequestHandler<UpdateProductCommand, UpdateProductResult>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _uow;

    /// <summary>
    /// Initializes a new instance of the UpdateProductHandler class.
    /// </summary>
    /// <param name="productRepository">The product repository</param>
    /// <param name="mapper">The AutoMapper instance</param>
    /// <param name="uow">The unit of work</param>
    public UpdateProductHandler(IProductRepository productRepository, IMapper mapper, IUnitOfWork uow)
    {
        _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _uow = uow ?? throw new ArgumentNullException(nameof(uow));
    }

    /// <summary>
    /// Handles the UpdateProductCommand.
    /// </summary>
    /// <param name="command">The command to update a product</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The updated product result</returns>
    public async Task<UpdateProductResult> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
    {
        var validator = new UpdateProductValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var product = await _productRepository.GetByIdAsync(command.Id, cancellationToken);
        if (product == null)
            throw new NotFoundException($"Product with ID {command.Id} not found.");

        // Update product properties
        product.Name = command.Name;
        product.Description = command.Description;
        product.Price = command.Price;
        product.StockQuantity = command.StockQuantity;
        product.Category = command.Category;
        product.UpdatedAt = DateTime.UtcNow;

        // Update status if needed
        if (command.Status == "Active" && product.Status != ProductStatus.Active)
            product.Activate();
        else if (command.Status == "Discontinued" && product.Status != ProductStatus.Discontinued)
            product.Discontinue();

        var updatedProduct = await _productRepository.UpdateAsync(product, cancellationToken);
        await _uow.CommitAsync(cancellationToken);

        return _mapper.Map<UpdateProductResult>(updatedProduct);
    }
} 