namespace Ambev.DeveloperEvaluation.Application.Common
{
    /// <summary>
    /// Interface for commands that require user context information.
    /// </summary>
    /// <remarks>
    /// Commands implementing this interface will have user information automatically 
    /// populated by the UserContextBehavior if not explicitly provided.
    /// </remarks>
    public interface IUserCommand
    {
        /// <summary>
        /// Gets or sets the external identifier of the user associated with the command.
        /// </summary>
        string CustomerExternalId { get; set; }

        /// <summary>
        /// Gets or sets the display name of the user associated with the command.
        /// </summary>
        string CustomerName { get; set; }
    }
}
