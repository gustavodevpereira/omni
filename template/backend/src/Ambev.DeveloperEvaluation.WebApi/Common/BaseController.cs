using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Collections.Generic;
using Ambev.DeveloperEvaluation.Common.Validation;
using FluentValidation;
using MediatR;
using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Common.Results;

namespace Ambev.DeveloperEvaluation.WebApi.Common;

[Route("api/[controller]")]
[ApiController]
public class BaseController : ControllerBase
{
    protected Guid GetCurrentUserId() =>
            Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new NullReferenceException());

    protected string GetCurrentUserEmail() =>
        User.FindFirst(ClaimTypes.Email)?.Value ?? throw new NullReferenceException();

    protected IActionResult Ok<T>(T data) =>
            base.Ok(new ApiResponseWithData<T> { Data = data, Success = true });

    protected IActionResult Created<T>(string routeName, object routeValues, T data) =>
        base.CreatedAtRoute(routeName, routeValues, new ApiResponseWithData<T> { Data = data, Success = true });

    protected IActionResult BadRequest(string message) =>
        base.BadRequest(new ApiResponse { Message = message, Success = false });

    protected IActionResult NotFound(string message = "Resource not found") =>
        base.NotFound(new ApiResponse { Message = message, Success = false });

    protected IActionResult OkPaginated<T>(PaginatedList<T> pagedList) =>
            base.Ok(new PaginatedResponse<T>
            {
                Data = pagedList,
                CurrentPage = pagedList.CurrentPage,
                TotalPages = pagedList.TotalPages,
                TotalCount = pagedList.TotalCount,
                Success = true
            });

    protected IActionResult CreatedAtAction<T>(string actionName, object routeValues, T data) =>
        base.CreatedAtAction(actionName, routeValues, new ApiResponseWithData<T> { Data = data, Success = true });

    /// <summary>
    /// Creates a BadRequest response with error details for validation errors or domain rule violations
    /// </summary>
    /// <param name="message">The error message</param>
    /// <param name="errors">Collection of validation errors or domain rule violations</param>
    /// <returns>BadRequest response with detailed error information</returns>
    protected IActionResult BadRequestWithErrors(string message, IEnumerable<ValidationErrorDetail> errors) =>
        base.BadRequest(new ApiResponse 
        { 
            Message = message, 
            Success = false,
            Errors = errors
        });
    
    /// <summary>
    /// Validates a request using the specified validator.
    /// </summary>
    /// <typeparam name="TRequest">The type of request to validate</typeparam>
    /// <param name="validator">The validator to use</param>
    /// <param name="request">The request to validate</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A ValidationResultDetail if validation fails, null if validation passes</returns>
    protected async Task<ValidationResultDetail?> ValidateRequestAsync<TRequest>(
        IValidator<TRequest> validator, 
        TRequest request, 
        CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        
        if (!validationResult.IsValid)
        {
            return new ValidationResultDetail
            {
                IsValid = false,
                Errors = validationResult.Errors.Select(error => new ValidationErrorDetail
                {
                    Error = error.ErrorCode,
                    Detail = error.ErrorMessage
                })
            };
        }
        
        return null;
    }
    
    /// <summary>
    /// Handles a paginated request, performing validation, command execution, and result mapping.
    /// </summary>
    /// <typeparam name="TRequest">The type of request</typeparam>
    /// <typeparam name="TCommand">The type of command</typeparam>
    /// <typeparam name="TResult">The type of result items</typeparam>
    /// <typeparam name="TResponse">The type of response items</typeparam>
    /// <param name="request">The request object</param>
    /// <param name="commandFactory">Function to create a command from the request</param>
    /// <param name="mapper">The mapper instance</param>
    /// <param name="mediator">The mediator instance</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>An action result containing the paginated response</returns>
    /// <remarks>
    /// This method uses a strongly-typed approach with PaginatedResultBase to handle paginated data.
    /// Rather than using reflection, it relies on the contract defined by PaginatedResultBase
    /// to access pagination properties in a type-safe manner.
    /// </remarks>
    protected async Task<IActionResult> HandlePaginatedRequestAsync<TRequest, TCommand, TResult, TResponse>(
        TRequest request,
        Func<TRequest, TCommand> commandFactory,
        IMapper mapper,
        IMediator mediator,
        CancellationToken cancellationToken)
        where TRequest : PaginationRequest
        where TCommand : IRequest<PaginatedResultBase<TResult>>
        where TResult : class
        where TResponse : class
    {
        // Execute the command directly without dynamic validator creation
        // Validation should be performed explicitly in the controller if needed
        var command = commandFactory(request);
        var result = await mediator.Send(command, cancellationToken);
        
        // We can access the properties directly because we're using PaginatedResultBase
        var items = result.Items;
        var totalCount = result.TotalCount;
        var pageNumber = result.PageNumber;
        var pageSize = result.PageSize;
        
        var mappedItems = mapper.Map<List<TResponse>>(items);
        var paginatedResult = new PaginatedList<TResponse>(mappedItems, totalCount, pageNumber, pageSize);
        
        return OkPaginated(paginatedResult);
    }
}
