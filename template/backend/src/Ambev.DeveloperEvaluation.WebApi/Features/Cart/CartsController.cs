using MediatR;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Microsoft.AspNetCore.Authorization;
using Ambev.DeveloperEvaluation.Application.Carts.UseCases.GetCart;
using Ambev.DeveloperEvaluation.Application.Carts.UseCases.GetCarts;
using Ambev.DeveloperEvaluation.Application.Carts.UseCases.AddCart;
using Ambev.DeveloperEvaluation.WebApi.Features.Cart.Actions.GetCart;
using Ambev.DeveloperEvaluation.WebApi.Features.Cart.Actions.GetCarts;
using Ambev.DeveloperEvaluation.WebApi.Features.Cart.Actions.AddCart;
using Ambev.DeveloperEvaluation.WebApi.Features.Cart.Actions.UpdateCart;
using Ambev.DeveloperEvaluation.WebApi.Features.Cart.Actions.DeleteCart;
using Ambev.DeveloperEvaluation.Application.Carts.UseCases.UpdateCart;
using Ambev.DeveloperEvaluation.Application.Carts.UseCases.DeleteCart;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Cart;

/// <summary>
/// Controller for managing cart operations
/// </summary>
[Authorize]
public class CartsController : BaseController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of CartsController
    /// </summary>
    /// <param name="mediator">The mediator instance</param>
    /// <param name="mapper">The AutoMapper instance</param>
    public CartsController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    /// <summary>
    /// Add a new cart
    /// </summary>
    /// <param name="request">Cart data</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Created cart details</returns>
    [HttpPost]
    [ProducesResponseType(typeof(GetCartResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddCart([FromBody] AddCartRequest request, CancellationToken cancellationToken)
    {
        var validator = new AddCartRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        var command = _mapper.Map<AddCartCommand>(request);
        var result = await _mediator.Send(command, cancellationToken);

        return Created(string.Empty, _mapper.Map<GetCartResponse>(result));
    }

    /// <summary>
    /// Get a specific cart by ID
    /// </summary>
    /// <param name="id">Cart ID</param>
    /// <returns>Cart details</returns>
    [HttpGet("{id:Guid}")]
    [ProducesResponseType(typeof(GetCartResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetCart([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var request = new GetCartRequest { Id = id };
        var command = _mapper.Map<GetCartCommand>(request);

        var result = await _mediator.Send(command, cancellationToken);

        return Ok(_mapper.Map<GetCartResponse>(result));
    }

    /// <summary>
    /// Get a paginated list of carts
    /// </summary>
    /// <param name="request">Pagination parameters</param>
    /// <returns>Paginated list of carts</returns>
    [HttpGet]
    [ProducesResponseType(typeof(GetCartsResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetCarts([FromQuery] GetCartsRequest request, CancellationToken cancellationToken)
    {
        var validator = new GetCartsRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        var command = _mapper.Map<GetCartsCommand>(request);
        var result = await _mediator.Send(command);

        return OkPaginated(_mapper.Map<List<GetCartResponse>>(result.Items), result.TotalCount, result.PageNumber, result.PageSize);
    }

    /// <summary>
    /// Update an existing cart
    /// </summary>
    /// <param name="id">Cart ID</param>
    /// <param name="request">Updated cart data</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Updated cart details</returns>
    [HttpPut("{id:Guid}")]
    [ProducesResponseType(typeof(GetCartResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateCart([FromRoute] Guid id, [FromBody] UpdateCartRequest request, CancellationToken cancellationToken)
    {
        var validator = new UpdateCartRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        var command = _mapper.Map<UpdateCartCommand>(request);
        command.Id = id;

        var result = await _mediator.Send(command, cancellationToken);

        return Ok(_mapper.Map<GetCartResponse>(result));
    }

    /// <summary>
    /// Delete a specific cart
    /// </summary>
    /// <param name="id">Cart ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Success message</returns>
    [HttpDelete("{id:Guid}")]
    [ProducesResponseType(typeof(DeleteCartResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteCart([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var command = new DeleteCartCommand { Id = id };
        await _mediator.Send(command, cancellationToken);

        return Ok(new DeleteCartResponse { Message = "Cart successfully deleted" });
    }
}