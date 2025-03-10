using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Collections.Generic;
using Ambev.DeveloperEvaluation.Common.Validation;

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
}
