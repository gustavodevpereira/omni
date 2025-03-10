import { Injectable } from '@angular/core';
import { MatSnackBar, MatSnackBarConfig, MatSnackBarRef, TextOnlySnackBar } from '@angular/material/snack-bar';

/**
 * Notification types
 */
export enum NotificationType {
  SUCCESS = 'success',
  ERROR = 'error',
  INFO = 'info',
  WARNING = 'warning'
}

/**
 * Default configuration for notifications
 */
const DEFAULT_CONFIG: MatSnackBarConfig = {
  duration: 5000, // 5 seconds
  horizontalPosition: 'end',
  verticalPosition: 'top',
};

/**
 * Notification Service
 * 
 * Provides methods to display various types of notifications using Material's SnackBar.
 * Centralizes notification display logic and styling.
 */
@Injectable({
  providedIn: 'root'
})
export class NotificationService {
  // Reference to the current notification
  private activeSnackBarRef: MatSnackBarRef<any> | null = null;

  constructor(private snackBar: MatSnackBar) { }

  /**
   * Shows a success notification
   * 
   * @param message - The message to display
   * @param action - Optional action text
   * @param config - Optional custom configuration
   */
  success(message: string, action: string = 'Close', config?: MatSnackBarConfig): void {
    this.show(message, action, { ...DEFAULT_CONFIG, ...config, panelClass: ['notification-success'] });
  }

  /**
   * Shows an error notification
   * 
   * @param message - The message to display
   * @param action - Optional action text
   * @param config - Optional custom configuration
   */
  error(message: string, action: string = 'Close', config?: MatSnackBarConfig): void {
    this.show(message, action, { ...DEFAULT_CONFIG, ...config, panelClass: ['notification-error'] });
  }

  /**
   * Shows an info notification
   * 
   * @param message - The message to display
   * @param action - Optional action text
   * @param config - Optional custom configuration
   */
  info(message: string, action: string = 'Close', config?: MatSnackBarConfig): void {
    this.show(message, action, { ...DEFAULT_CONFIG, ...config, panelClass: ['notification-info'] });
  }

  /**
   * Shows a warning notification
   * 
   * @param message - The message to display
   * @param action - Optional action text
   * @param config - Optional custom configuration
   */
  warning(message: string, action: string = 'Close', config?: MatSnackBarConfig): void {
    this.show(message, action, { ...DEFAULT_CONFIG, ...config, panelClass: ['notification-warning'] });
  }

  /**
   * Shows a notification with the given type
   * 
   * @param message - The message to display
   * @param type - The type of notification
   * @param action - Optional action text
   * @param config - Optional custom configuration
   */
  notify(message: string, type: NotificationType, action: string = 'Close', config?: MatSnackBarConfig): void {
    switch (type) {
      case NotificationType.SUCCESS:
        this.success(message, action, config);
        break;
      case NotificationType.ERROR:
        this.error(message, action, config);
        break;
      case NotificationType.INFO:
        this.info(message, action, config);
        break;
      case NotificationType.WARNING:
        this.warning(message, action, config);
        break;
      default:
        this.info(message, action, config);
    }
  }

  /**
   * Dismisses the current notification if it exists
   */
  dismiss(): void {
    if (this.activeSnackBarRef) {
      this.activeSnackBarRef.dismiss();
      this.activeSnackBarRef = null;
    }
  }

  /**
   * Shows a notification using the SnackBar
   * 
   * @param message - The message to display
   * @param action - The action text
   * @param config - The SnackBar configuration
   */
  private show(message: string, action: string, config: MatSnackBarConfig): void {
    // Dismiss any active notification before showing a new one
    this.dismiss();
    
    // Open new notification and store the reference
    this.activeSnackBarRef = this.snackBar.open(message, action, config);
  }
} 