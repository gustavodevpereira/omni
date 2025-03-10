using Ambev.DeveloperEvaluation.Application.Common.Commands;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Common.Validators;

/// <summary>
/// Base validator for paginated requests.
/// Provides common validation rules for pagination parameters.
/// </summary>
/// <typeparam name="T">The type of paginated request to validate</typeparam>
/// <remarks>
/// This base validator ensures consistent validation rules for pagination parameters across the application.
/// All validators for paginated commands should inherit from this base class.
/// </remarks>
public abstract class PaginatedRequestValidator<T> : AbstractValidator<T> where T : PaginatedRequestBase
{
    private const int MaxPageSize = 100;
    
    /// <summary>
    /// Initializes a new instance of the <see cref="PaginatedRequestValidator{T}"/> class with common validation rules.
    /// </summary>
    protected PaginatedRequestValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThan(0)
            .WithMessage("Page number must be greater than 0");

        RuleFor(x => x.PageSize)
            .GreaterThan(0)
            .WithMessage("Page size must be greater than 0")
            .LessThanOrEqualTo(MaxPageSize)
            .WithMessage($"Page size cannot exceed {MaxPageSize}");
    }
} 