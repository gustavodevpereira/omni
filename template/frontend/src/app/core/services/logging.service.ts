import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';

/**
 * Service responsible for application logging and error tracking.
 * 
 * @description
 * This service provides centralized logging functionality with different
 * severity levels (info, warning, error). It selectively logs based on
 * environment and can be extended to send logs to external services.
 * 
 * The service provides structured logging with optional metadata
 * and automatically formats messages for better readability.
 * 
 * In production environments, debug logs are suppressed, but errors
 * could be sent to monitoring services like Sentry or LogRocket.
 * 
 * @example
 * // Simple info log
 * this.loggingService.logInfo('User logged in');
 * 
 * // Warning with context data
 * this.loggingService.logWarning('API response slow', { endpoint: '/products', responseTime: 3500 });
 * 
 * // Error with caught exception
 * this.loggingService.logError('Failed to load user data', error);
 */
@Injectable({
  providedIn: 'root'
})
export class LoggingService {
  /**
   * Log an informational message.
   * 
   * @param message - The message to log
   * @param data - Optional data to provide context
   * 
   * @example
   * loggingService.logInfo('User viewed product details', { productId: '123' });
   */
  logInfo(message: string, data?: any): void {
    if (!environment.production) {
      console.info(`[INFO] ${message}`, data || '');
    }
    
    // In production, could send to a logging service for important info
  }

  /**
   * Log a warning message.
   * 
   * @param message - The warning message
   * @param data - Optional data to provide context
   * 
   * @example
   * loggingService.logWarning('API call took over 3 seconds', { endpoint: '/users', time: 3200 });
   */
  logWarning(message: string, data?: any): void {
    // Warnings are logged in all environments
    console.warn(`[WARNING] ${message}`, data || '');
    
    // In production, could send to a logging service
  }

  /**
   * Log an error with details.
   * 
   * @param message - The error message
   * @param error - The error object or details
   * 
   * @example
   * try {
   *   // code that might throw
   * } catch (error) {
   *   loggingService.logError('Failed to process payment', error);
   * }
   */
  logError(message: string, error: any): void {
    // Errors are logged in all environments
    console.error(`[ERROR] ${message}`, error);
    
    // In production, send to error monitoring service
    if (environment.production) {
      this.sendToErrorService(message, error);
    }
  }

  /**
   * Log application initialization
   */
  logAppInit(): void {
    this.logInfo(`Application initialized: ${environment.production ? 'PRODUCTION' : 'DEVELOPMENT'}`);
  }

  /**
   * Send error to external logging service
   * (implementation would depend on the specific service used)
   */
  private sendToErrorService(message: string, error: any): void {
    // Example implementation for external logging service
    // Uncomment and implement when adding a service like Sentry
    
    /*
    if (environment.sentryDsn) {
      Sentry.captureException(error, {
        extra: {
          message,
          timestamp: new Date().toISOString()
        }
      });
    }
    */
  }
} 