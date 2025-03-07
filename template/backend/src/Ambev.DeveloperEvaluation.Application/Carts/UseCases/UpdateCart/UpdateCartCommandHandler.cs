using Ambev.DeveloperEvaluation.Application.Common.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Entities.Carts;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Carts.UseCases.UpdateCart;

public class UpdateCartCommandHandler : IRequestHandler<UpdateCartCommand, UpdateCartResult>
{
    private readonly ICartRepository _cartRepository;
    private readonly IMapper _mapper;

    public UpdateCartCommandHandler(ICartRepository cartRepository, IMapper mapper)
    {
        _cartRepository = cartRepository;
        _mapper = mapper;
    }

    public async Task<UpdateCartResult> Handle(UpdateCartCommand request, CancellationToken cancellationToken)
    {
        var cart = await _cartRepository.GetByIdAsync(request.Id, cancellationToken);

        if (cart == null)
            throw new NotFoundException($"Cart with ID {request.Id} not found");

        foreach (var product in cart.Products.ToList())
        {
            cart.RemoveItem(product.Id);
        }

        var updatedCart = new Cart(
            cart.CreatedOn,
            request.CustomerExternalId,
            request.CustomerName,
            request.BranchExternalId,
            request.BranchName
        );

        updatedCart.Id = cart.Id;

        if (cart.Status == CartStatus.Cancelled)
            updatedCart.CancelCart();

        foreach (var product in request.Products)
        {
            updatedCart.AddProduct(
                product.ProductExternalId,
                product.ProductName,
                product.Quantity,
                product.UnitPrice
            );
        }

        await _cartRepository.UpdateAsync(updatedCart, cancellationToken);

        return _mapper.Map<UpdateCartResult>(updatedCart);
    }
}
