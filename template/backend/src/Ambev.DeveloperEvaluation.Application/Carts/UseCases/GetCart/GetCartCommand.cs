using Ambev.DeveloperEvaluation.Application.Carts.Common.Results;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Carts.UseCases.GetCart;

/// <summary>
/// Command for retrieving a cart by its ID
/// </summary>
public record GetCartCommand : IRequest<CartResult>
{
    /// <summary>
    /// The unique identifier of the cart to retrieve
    /// </summary>
    public Guid Id { get; }

    /// <summary>
    /// Initializes a new instance of GetCartCommand
    /// </summary>
    /// <param name="id">The ID of the cart to retrieve</param>
    public GetCartCommand(Guid id)
    {
        Id = id;
    }
}