namespace Ambev.DeveloperEvaluation.Domain.Common
{
    /// <summary>
    /// Defines the contract for accessing the current user context in the application.
    /// </summary>
    public interface IUserContext
    {
        /// <summary>
        /// Gets the unique identifier of the current user.
        /// </summary>
        /// <returns>String representation of the user ID.</returns>
        string GetUserId();

        /// <summary>
        /// Gets the display name of the current user.
        /// </summary>
        /// <returns>The name of the user.</returns>
        string GetUserName();

        /// <summary>
        /// Gets the email address of the current user.
        /// </summary>
        /// <returns>The email address of the user.</returns>
        string GetUserEmail();
    }
}