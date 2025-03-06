using System.Security.Claims;
using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.WebApi.Common
{
    /// <summary>
    /// Implementation of the user context that extracts user information from the HTTP context.
    /// </summary>
    public class HttpUserContext : IUserContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpUserContext"/> class.
        /// </summary>
        /// <param name="httpContextAccessor">The HTTP context accessor to extract user claims.</param>
        public HttpUserContext(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Gets the user ID from the current HTTP context.
        /// </summary>
        /// <returns>The user ID from claims, or empty string if not available.</returns>
        public string GetUserId()
        {
            return _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
        }

        /// <summary>
        /// Gets the user name from the current HTTP context.
        /// </summary>
        /// <returns>The user name from claims, or empty string if not available.</returns>
        public string GetUserName()
        {
            return _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value ?? string.Empty;
        }

        /// <summary>
        /// Gets the user email from the current HTTP context.
        /// </summary>
        /// <returns>The user email from claims, or empty string if not available.</returns>
        public string GetUserEmail()
        {
            return _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value ?? string.Empty;
        }
    }
}