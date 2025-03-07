using MediatR;
using AutoMapper;
using FluentValidation;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using OneOf.Types;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Entities.Carts;
using Ambev.DeveloperEvaluation.Application.Carts.Common.Results;
using Ambev.DeveloperEvaluation.Application.Users.CreateUser;

namespace Ambev.DeveloperEvaluation.Application.Carts.UseCases.AddCart;

/// <summary>
/// Handler for adding a new cart
/// </summary>
public class AddCartHandler : IRequestHandler<AddCartCommand, CartResult>
{
    private readonly ICartRepository _cartRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of AddCartHandler
    /// </summary>
    /// <param name="cartRepository">Cart repository instance</param>
    /// <param name="unitOfWork">Unit of work instance</param>
    /// <param name="mapper">Mapper instance</param>
    public AddCartHandler(
        ICartRepository cartRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _cartRepository = cartRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    /// <summary>
    /// Handles the command to add a new cart
    /// </summary>
    /// <param name="command">Command containing cart data</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Result containing the added cart data</returns>
    public async Task<CartResult> Handle(AddCartCommand command, CancellationToken cancellationToken)
    {
        var validator = new AddCartValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var cart = new Cart(command.CreatedOn, command.CostumerId, command.CostumerName, command.BranchId, command.BranchName);

        foreach (var productCommand in command.Products) cart.AddProduct(productCommand.Id, productCommand.Name, productCommand.Quantity, productCommand.UnitPrice);

        // Save to repository
        await _cartRepository.CreateAsync(cart, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);

        // Map to response and return
        return _mapper.Map<CartResult>(cart);
    }
}