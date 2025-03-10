using MediatR;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Carts.GetCarts;
using Ambev.DeveloperEvaluation.WebApi.Features.Carts.AddCart;
using Ambev.DeveloperEvaluation.WebApi.Features.Carts.CalculateDiscount;
using Ambev.DeveloperEvaluation.Application.Carts.UseCases.GetCarts;
using Ambev.DeveloperEvaluation.Application.Carts.UseCases.AddCart;
using Ambev.DeveloperEvaluation.Application.Carts.UseCases.CalculateCartDiscount;
using Ambev.DeveloperEvaluation.Application.Carts.Common.Results;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.GetProducts;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts;

/// <summary>
/// Controller for managing shopping cart operations in the system.
/// </summary>
/// <remarks>
/// Provides endpoints for retrieving, creating, and calculating discounts for shopping carts.
/// All operations require authentication and operate in the context of the current user.
/// </remarks>
[Authorize]
public class CartsController : BaseController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="CartsController"/> class.
    /// </summary>
    /// <param name="mediator">The MediatR instance for handling commands and queries</param>
    /// <param name="mapper">The AutoMapper instance for object mapping</param>
    public CartsController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    /// <summary>
    /// Retrieves a paginated list of carts for the current user.
    /// </summary>
    /// <param name="request">The pagination parameters for the request</param>
    /// <param name="cancellationToken">Cancellation token for async operations</param>
    /// <returns>A paginated list of carts belonging to the current user</returns>
    /// <response code="200">Returns the paginated list of carts</response>
    /// <response code="400">If the request parameters are invalid</response>
    /// <response code="401">If the user is not authenticated</response>
    [HttpGet]
    [ProducesResponseType(typeof(PaginatedResponse<GetCartsResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetCarts([FromQuery] GetCartsRequest request, CancellationToken cancellationToken)
    {
        // Validate the request explicitly
        var validator = new GetCartsRequestValidator();
        var validationResult = await ValidateRequestAsync(validator, request, cancellationToken);
        if (validationResult != null)
            return BadRequestWithErrors("Validation failed", validationResult.Errors);
        
        // Get the current user ID from the token
        var currentUserId = GetCurrentUserId();

        var command = new GetCartsCommand(request.PageNumber, request.PageSize, currentUserId);
        var result = await _mediator.Send(command, cancellationToken);

        var items = _mapper.Map<IEnumerable<GetCartsResponse>>(result.Items);

        var paginatedList = new PaginatedList<GetCartsResponse>
       (
           items.ToList(),
           result.TotalCount,
           result.PageNumber,
           result.PageSize
       );

        return OkPaginated(paginatedList);
    }

    /// <summary>
    /// Creates a new cart for the current user.
    /// </summary>
    /// <param name="request">The cart creation request with product details</param>
    /// <param name="cancellationToken">Cancellation token for async operations</param>
    /// <returns>The created cart with complete details</returns>
    /// <response code="201">Returns the newly created cart</response>
    /// <response code="400">If the request is invalid or violates business rules</response>
    /// <response code="401">If the user is not authenticated</response>
    /// <response code="403">If the user is not authorized to create a cart</response>
    [HttpPost]
    [Authorize(Roles = "Admin,Customer")]
    [ProducesResponseType(typeof(ApiResponseWithData<AddCartResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> AddCart([FromBody] AddCartRequest request, CancellationToken cancellationToken)
    {
        // Validate the request
        var validationResult = await ValidateRequestAsync(new AddCartRequestValidator(), request, cancellationToken);
        if (validationResult != null)
            return BadRequestWithErrors("Validation failed", validationResult.Errors);

        // Map the request to command
        var command = _mapper.Map<AddCartCommand>(request);
        
        // Set only the customer ID - other user information will be fetched from the repository
        command.CostumerId = GetCurrentUserId();

        var result = await _mediator.Send(command, cancellationToken);

        var response = _mapper.Map<AddCartResponse>(result);
        return Created(null, new { id = result.Id }, response);
    }

    /// <summary>
    /// Calculates discounts for a cart without saving it to the database.
    /// </summary>
    /// <param name="request">The cart data with products for discount calculation</param>
    /// <param name="cancellationToken">Cancellation token for async operations</param>
    /// <returns>The cart with calculated discounts and total amounts</returns>
    /// <response code="200">Returns the cart with calculated discounts</response>
    /// <response code="400">If the request is invalid or violates business rules</response>
    /// <response code="401">If the user is not authenticated</response>
    [HttpPost("CalculateDiscount")]
    [ProducesResponseType(typeof(ApiResponseWithData<CalculateCartDiscountResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> CalculateDiscount(
        [FromBody] CalculateCartDiscountRequest request, 
        CancellationToken cancellationToken)
    {
        // Validate the request
        var validationResult = await ValidateRequestAsync(new CalculateCartDiscountRequestValidator(), request, cancellationToken);
        if (validationResult != null)
            return BadRequestWithErrors("Validation failed", validationResult.Errors);

        // Map the request to command
        var command = _mapper.Map<CalculateCartDiscountCommand>(request);
        
        // Set only the customer ID - other user information will be fetched from the repository
        command.CustomerId = GetCurrentUserId();
        
        var result = await _mediator.Send(command, cancellationToken);

        var response = _mapper.Map<CalculateCartDiscountResponse>(result);
        return Ok(response);
    }
} 