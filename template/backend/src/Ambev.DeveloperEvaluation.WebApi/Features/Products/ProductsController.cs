using MediatR;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.GetProducts;
using Ambev.DeveloperEvaluation.Application.Products.UseCases.GetProducts;
using Ambev.DeveloperEvaluation.Application.Products.Common.Results;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products;

/// <summary>
/// Controller for managing product-related operations in the system.
/// </summary>
/// <remarks>
/// Provides endpoints for retrieving product information with filtering and pagination.
/// All operations follow RESTful principles and return standardized responses.
/// </remarks>
[Authorize]
public class ProductsController : BaseController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="ProductsController"/> class.
    /// </summary>
    /// <param name="mediator">The MediatR instance for handling commands and queries</param>
    /// <param name="mapper">The AutoMapper instance for object mapping</param>
    public ProductsController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    /// <summary>
    /// Retrieves a paginated list of products.
    /// </summary>
    /// <param name="request">The pagination parameters for the request</param>
    /// <param name="cancellationToken">Cancellation token for async operations</param>
    /// <returns>A paginated list of products with metadata</returns>
    /// <response code="200">Returns the paginated list of products</response>
    /// <response code="400">If the request parameters are invalid</response>
    /// <response code="401">If the user is not authenticated</response>
    [HttpGet]
    [ProducesResponseType(typeof(PaginatedList<GetProductsResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetProducts([FromQuery] GetProductsRequest request, CancellationToken cancellationToken)
    {
        // Validate the request explicitly
        var validator = new GetProductsRequestValidator();
        var validationResult = await ValidateRequestAsync(validator, request, cancellationToken);
        if (validationResult != null)
            return BadRequestWithErrors("Validation failed", validationResult.Errors);

        var command = new GetProductsCommand(request.PageNumber, request.PageSize);
        var result = await _mediator.Send(command, cancellationToken);

        var items = _mapper.Map<IEnumerable<GetProductsResponse>>(result.Items);

        var paginatedList = new PaginatedList<GetProductsResponse>
        (
            items.ToList(),
            result.TotalCount,
            result.PageNumber,
            result.PageSize
        );

        return OkPaginated(paginatedList);
    }
} 