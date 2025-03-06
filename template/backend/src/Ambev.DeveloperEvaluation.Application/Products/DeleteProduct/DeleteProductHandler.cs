using MediatR;
using FluentValidation;
using Ambev.DeveloperEvaluation.Domain.Repositories;

namespace Ambev.DeveloperEvaluation.Application.Products.DeleteProduct;

/// <summary>
/// Handler for processing DeleteProductCommand requests.
/// </summary>
public class DeleteProductHandler : IRequestHandler<DeleteProductCommand, bool>
{
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _uow;

    /// <summary>
    /// Initializes a new instance of the DeleteProductHandler class.
    /// </summary>
    /// <param name="productRepository">The product repository</param>
    /// <param name="uow">The unit of work</param>
    public DeleteProductHandler(IProductRepository productRepository, IUnitOfWork uow)
    {
        _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
        _uow = uow ?? throw new ArgumentNullException(nameof(uow));
    }

    /// <summary>
    /// Handles the DeleteProductCommand.
    /// </summary>
    /// <param name="command">The command to delete a product</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if the product was deleted, false otherwise</returns>
    public async Task<bool> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
    {
        var validator = new DeleteProductValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var result = await _productRepository.DeleteAsync(command.Id, cancellationToken);
        
        if (result)
            await _uow.CommitAsync(cancellationToken);
            
        return result;
    }
} 