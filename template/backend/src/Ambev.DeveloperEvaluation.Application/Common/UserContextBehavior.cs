using Ambev.DeveloperEvaluation.Domain.Common;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Common
{
    /// <summary>
    /// MediatR pipeline behavior that automatically populates user context information in commands.
    /// </summary>
    /// <typeparam name="TRequest">The type of the request.</typeparam>
    /// <typeparam name="TResponse">The type of the response.</typeparam>
    public class UserContextBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
     where TRequest : IUserCommand
    {
        private readonly IUserContext _userContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserContextBehavior{TRequest, TResponse}"/> class.
        /// </summary>
        /// <param name="userContext">The user context service.</param>
        public UserContextBehavior(IUserContext userContext)
        {
            _userContext = userContext;
        }

        /// <summary>
        /// Handles the request by populating user information when not provided.
        /// </summary>
        /// <param name="request">The request being handled.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <param name="next">The delegate to the next handler in the pipeline.</param>
        /// <returns>The response from the next handler.</returns>
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            // Preenche as informações do usuário se ainda não estiverem definidas
            if (string.IsNullOrEmpty(request.CustomerExternalId))
                request.CustomerExternalId = _userContext.GetUserId();

            if (string.IsNullOrEmpty(request.CustomerName))
                request.CustomerName = _userContext.GetUserName();

            return await next();
        }
    }
}
