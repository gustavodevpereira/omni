using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Common;

/// <summary>
/// Base validator for pagination requests that enforces common validation rules.
/// </summary>
/// <typeparam name="T">The type of pagination request to validate</typeparam>
/// <remarks>
/// This validator provides standard validation rules for pagination parameters,
/// ensuring consistency across all paginated requests in the API.
/// </remarks>
public class PaginationRequestValidator<T> : AbstractValidator<T> where T : PaginationRequest
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PaginationRequestValidator{T}"/> class
    /// with common validation rules for pagination.
    /// </summary>
    protected PaginationRequestValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThan(0).WithMessage("Page number must be greater than 0");

        RuleFor(x => x.PageSize)
            .GreaterThan(0).WithMessage("Page size must be greater than 0")
            .LessThanOrEqualTo(100).WithMessage("Page size must not exceed 100");
    }
} 