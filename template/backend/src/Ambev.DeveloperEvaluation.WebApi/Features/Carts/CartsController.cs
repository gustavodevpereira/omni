using MediatR;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Carts.GetCarts;
using Ambev.DeveloperEvaluation.WebApi.Features.Carts.AddCart;
using Ambev.DeveloperEvaluation.WebApi.Features.Carts.CalculateDiscount;
using Ambev.DeveloperEvaluation.Application.Carts.UseCases.GetCarts;
using Ambev.DeveloperEvaluation.Application.Carts.UseCases.AddCart;
using Ambev.DeveloperEvaluation.Application.Carts.UseCases.CalculateCartDiscount;
using Microsoft.AspNetCore.Authorization;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts;

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
    /// Retrieves a paginated list of carts for the current user
    /// </summary>
    /// <param name="request">The pagination parameters</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A paginated list of carts</returns>
    [HttpGet]
    [ProducesResponseType(typeof(PaginatedResponse<GetCartsResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetCarts([FromQuery] GetCartsRequest request, CancellationToken cancellationToken)
    {
        var validator = new GetCartsRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        // Get the current user ID from the token
        var currentUserId = GetCurrentUserId();
        
        // Create the command with user filtering
        var command = new GetCartsCommand(request.PageNumber, request.PageSize, currentUserId);
        var result = await _mediator.Send(command, cancellationToken);

        var mappedItems = _mapper.Map<List<GetCartsResponse>>(result.Items);
        
        var paginatedRes = new PaginatedList<GetCartsResponse>(mappedItems, result.TotalCount, result.PageNumber, result.PageSize);
        
        return OkPaginated(paginatedRes);
    }

    /// <summary>
    /// Creates a new cart for the current user
    /// </summary>
    /// <param name="request">The cart creation request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created cart</returns>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponseWithData<AddCartResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddCart([FromBody] AddCartRequest request, CancellationToken cancellationToken)
    {
        var validator = new AddCartRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        // Map the request to command
        var command = _mapper.Map<AddCartCommand>(request);
        
        // Set only the customer ID - other user information will be fetched from the repository
        command.CostumerId = GetCurrentUserId();

        var result = await _mediator.Send(command, cancellationToken);

        var response = _mapper.Map<AddCartResponse>(result);
        return CreatedAtAction(nameof(AddCart), new { id = result.Id }, response);
    }

    /// <summary>
    /// Calculates discounts for a cart without saving it
    /// </summary>
    /// <param name="request">The cart data with products</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The cart with discount calculations</returns>
    [HttpPost("CalculateDiscount")]
    [ProducesResponseType(typeof(ApiResponseWithData<CalculateCartDiscountResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CalculateDiscount(
        [FromBody] CalculateCartDiscountRequest request, 
        CancellationToken cancellationToken)
    {
        var validator = new CalculateCartDiscountRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        // Map the request to command
        var command = _mapper.Map<CalculateCartDiscountCommand>(request);
        
        // Set only the customer ID - other user information will be fetched from the repository
        command.CustomerId = GetCurrentUserId();
        
        var result = await _mediator.Send(command, cancellationToken);

        var response = _mapper.Map<CalculateCartDiscountResponse>(result);
        return Ok(response);
    }
} 