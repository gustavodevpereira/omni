import { Injectable } from '@angular/core';
import { MatSnackBar, MatSnackBarConfig } from '@angular/material/snack-bar';

/**
 * Service responsible for displaying notification messages to users.
 * 
 * @description
 * This service provides methods to display various types of notifications
 * (success, error, info, warning) using Angular Material's SnackBar component.
 * 
 * The notifications are styled differently based on their type and have
 * sensible default durations that can be overridden when needed.
 * 
 * @example
 * // Display a success message
 * this.notificationService.success('Profile updated successfully');
 * 
 * // Display an error with longer duration
 * this.notificationService.error('Failed to update profile', 8000);
 */
@Injectable({
  providedIn: 'root'
})
export class NotificationService {
  /**
   * Default configuration for notifications
   */
  private defaultConfig: MatSnackBarConfig = {
    horizontalPosition: 'end',
    verticalPosition: 'top',
  };

  /**
   * Creates an instance of NotificationService.
   * 
   * @param snackBar - Angular Material's SnackBar service
   */
  constructor(private snackBar: MatSnackBar) {}

  /**
   * Displays a success notification message.
   * 
   * @param message - The message to display
   * @param duration - How long to display the message in milliseconds (default: 3000ms)
   * 
   * @example
   * notificationService.success('Item added to cart');
   */
  success(message: string, duration: number = 3000): void {
    this.snackBar.open(message, 'Close', {
      ...this.defaultConfig,
      duration,
      panelClass: ['success-snackbar']
    });
  }

  /**
   * Displays an error notification message.
   * 
   * @param message - The error message to display
   * @param duration - How long to display the message in milliseconds (default: 5000ms)
   * 
   * @example
   * notificationService.error('Failed to process payment');
   */
  error(message: string, duration: number = 5000): void {
    this.snackBar.open(message, 'Close', {
      ...this.defaultConfig,
      duration,
      panelClass: ['error-snackbar']
    });
  }

  /**
   * Displays an informational notification message.
   * 
   * @param message - The information message to display
   * @param duration - How long to display the message in milliseconds (default: 3000ms)
   * 
   * @example
   * notificationService.info('Your order is being processed');
   */
  info(message: string, duration: number = 3000): void {
    this.snackBar.open(message, 'Close', {
      ...this.defaultConfig,
      duration,
      panelClass: ['info-snackbar']
    });
  }

  /**
   * Displays a warning notification message.
   * 
   * @param message - The warning message to display
   * @param duration - How long to display the message in milliseconds (default: 4000ms)
   * 
   * @example
   * notificationService.warning('Your session is about to expire');
   */
  warning(message: string, duration: number = 4000): void {
    this.snackBar.open(message, 'Close', {
      ...this.defaultConfig,
      duration,
      panelClass: ['warning-snackbar']
    });
  }
} 