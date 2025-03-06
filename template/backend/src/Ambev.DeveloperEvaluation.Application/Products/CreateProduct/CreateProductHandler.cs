using AutoMapper;
using MediatR;
using FluentValidation;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Entities.Products;

namespace Ambev.DeveloperEvaluation.Application.Products.CreateProduct;

/// <summary>
/// Handler for processing CreateProductCommand requests.
/// </summary>
public class CreateProductHandler : IRequestHandler<CreateProductCommand, CreateProductResult>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _uow;

    /// <summary>
    /// Initializes a new instance of the CreateProductHandler class.
    /// </summary>
    /// <param name="productRepository">The product repository</param>
    /// <param name="mapper">The AutoMapper instance</param>
    /// <param name="uow">The unit of work</param>
    public CreateProductHandler(IProductRepository productRepository, IMapper mapper, IUnitOfWork uow)
    {
        _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _uow = uow ?? throw new ArgumentNullException(nameof(uow));
    }

    /// <summary>
    /// Handles the CreateProductCommand.
    /// </summary>
    /// <param name="command">The command to create a product</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created product result</returns>
    public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
    {
        var validator = new CreateProductValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        // Check if product with the same SKU already exists
        var existingProduct = await _productRepository.GetBySkuAsync(command.Sku, cancellationToken);
        if (existingProduct != null)
            throw new ValidationException("A product with the same SKU already exists.");

        var product = _mapper.Map<Product>(command);

        var createdProduct = await _productRepository.CreateAsync(product, cancellationToken);
        await _uow.CommitAsync(cancellationToken);

        var result = _mapper.Map<CreateProductResult>(createdProduct);
        return result;
    }
} 