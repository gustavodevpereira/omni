import { ErrorHandler, Injectable, NgZone } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';
import { Router } from '@angular/router';
import { LoggingService } from '../services/logging.service';
import { NotificationService } from '../services/notification.service';
import { AuthService } from '../services/auth.service';

/**
 * @description
 * Global error handler that centrally manages all application and HTTP errors.
 * Provides specialized handling for different error types, including:
 * - HTTP errors (400, 401, 403, 404, 500, etc.)
 * - Validation errors
 * - Application errors (general errors and TypeErrors)
 * 
 * The handler integrates with notification, logging, and authentication services
 * to provide a comprehensive error management solution.
 * 
 * @usageNotes
 * This handler is registered in the application providers using:
 * ```typescript
 * { provide: ErrorHandler, useClass: GlobalErrorHandler }
 * ```
 * 
 * @implements {ErrorHandler}
 */
@Injectable()
export class GlobalErrorHandler implements ErrorHandler {
  /**
   * Creates an instance of GlobalErrorHandler.
   * 
   * @param notificationService - Service to display user-friendly error notifications
   * @param loggingService - Service to log errors for debugging and monitoring
   * @param router - Angular router for navigation in response to certain errors
   * @param authService - Authentication service to handle auth-related errors
   * @param zone - NgZone to ensure UI updates occur within Angular's change detection
   */
  constructor(
    private notificationService: NotificationService,
    private loggingService: LoggingService,
    private router: Router,
    private authService: AuthService,
    private zone: NgZone
  ) {}

  /**
   * Main error handling method that acts as the entry point for all errors.
   * Determines the error type and routes to the appropriate specialized handler.
   * 
   * @param error - The error to be handled, either a standard Error or HttpErrorResponse
   */
  handleError(error: Error | HttpErrorResponse): void {
    // Run error handling inside NgZone to ensure UI updates
    this.zone.run(() => {
      this.loggingService.logError(error.message, error);

      if (error instanceof HttpErrorResponse) {
        // Handle server or connection errors
        this.handleHttpError(error);
      } else {
        // Handle client-side errors
        this.handleApplicationError(error);
      }
    });
  }

  /**
   * Handles HTTP errors from API calls.
   * Responds to different status codes with appropriate actions and user messages.
   * 
   * @param error - The HTTP error response to handle
   * @private
   */
  private handleHttpError(error: HttpErrorResponse): void {
    switch (error.status) {
      case 0:
        // Connection error - network issue or CORS
        this.notificationService.error(
          'Network Error: Unable to connect to the server. Please check your internet connection.'
        );
        break;

      case 400:
        // Bad request - typically validation errors
        const validationMessage = this.extractValidationMessages(error.error);
        this.notificationService.error(
          validationMessage || 'The submitted data is invalid.'
        );
        break;

      case 401:
        // Unauthorized - user not authenticated
        this.notificationService.error(
          'Your session has expired. Please log in again.'
        );
        this.authService.logout();
        this.router.navigate(['/auth/login']);
        break;

      case 403:
        // Forbidden - user not authorized
        this.notificationService.error(
          'You do not have permission to perform this action.'
        );
        this.router.navigate(['/forbidden']);
        break;

      case 404:
        // Not found
        this.notificationService.error(
          'The requested resource was not found.'
        );
        break;

      case 500:
        // Server error
        this.notificationService.error(
          'An unexpected server error occurred. Our team has been notified.'
        );
        break;

      default:
        // Other errors
        this.notificationService.error(
          `An unexpected error occurred (${error.status}): ${error.message}`
        );
        break;
    }
  }

  /**
   * Handles application-level errors that occur in the client-side code.
   * Provides specialized handling for different error types.
   * 
   * @param error - The application error to handle
   * @private
   */
  private handleApplicationError(error: Error): void {
    let errorMessage = 'An unexpected error occurred';
    
    // Special handling for specific error types
    if (error instanceof TypeError) {
      errorMessage = `Type Error: ${error.message}`;
    } else if (error.message) {
      errorMessage = `Application Error: ${error.message}`;
    }
    
    this.notificationService.error(errorMessage);
  }

  /**
   * Extracts validation error messages from the API response.
   * Handles various validation error formats from the backend.
   * 
   * @param errors - The error object from the API response
   * @returns A formatted string containing all validation error messages
   * @private
   */
  private extractValidationMessages(errors: any): string {
    if (!errors) return '';

    // Handle array of error messages
    if (Array.isArray(errors)) {
      return errors.join('<br>');
    }

    // Handle object with error properties
    if (typeof errors === 'object') {
      const messages: string[] = [];
      
      // Extract messages from various possible formats
      if (errors.errors && Array.isArray(errors.errors)) {
        messages.push(...errors.errors);
      } else if (errors.message) {
        messages.push(errors.message);
      } else {
        // Extract from object with field-specific errors
        Object.keys(errors).forEach(key => {
          const fieldErrors = errors[key];
          if (Array.isArray(fieldErrors)) {
            messages.push(`${key}: ${fieldErrors.join(', ')}`);
          } else {
            messages.push(`${key}: ${fieldErrors}`);
          }
        });
      }
      
      return messages.join('<br>');
    }

    // Handle simple string error
    return errors.toString();
  }
} 