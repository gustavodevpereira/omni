import { TestBed } from '@angular/core/testing';
import { NotificationService } from './notification.service';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';

/**
 * Test suite for NotificationService
 * 
 * @description
 * Tests the notification service that provides user feedback through snackbars.
 * This suite verifies that:
 * - Different notification types (success, error, info, warning) display correctly
 * - Notification durations are configurable
 * - Default configurations are applied correctly
 */
describe('NotificationService', () => {
  let service: NotificationService;
  let snackBarSpy: jasmine.SpyObj<MatSnackBar>;

  beforeEach(() => {
    // Create spy for MatSnackBar
    snackBarSpy = jasmine.createSpyObj('MatSnackBar', ['open']);
    
    TestBed.configureTestingModule({
      providers: [
        NotificationService,
        { provide: MatSnackBar, useValue: snackBarSpy }
      ]
    });
    
    service = TestBed.inject(NotificationService);
  });

  /**
   * Test service creation
   */
  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  /**
   * Test success notification functionality
   */
  describe('success notification', () => {
    it('should open a snackbar with success styling', () => {
      // Arrange
      const message = 'Operation successful';
      
      // Act
      service.success(message);
      
      // Assert
      expect(snackBarSpy.open).toHaveBeenCalledWith(
        message,
        'Close',
        jasmine.objectContaining({
          duration: 3000,
          horizontalPosition: 'end',
          verticalPosition: 'top',
          panelClass: ['success-snackbar']
        })
      );
    });

    it('should respect custom duration', () => {
      // Arrange
      const message = 'Operation successful';
      const customDuration = 1500;
      
      // Act
      service.success(message, customDuration);
      
      // Assert
      expect(snackBarSpy.open).toHaveBeenCalledWith(
        message,
        'Close',
        jasmine.objectContaining({
          duration: customDuration
        })
      );
    });
  });

  /**
   * Test error notification functionality
   */
  describe('error notification', () => {
    it('should open a snackbar with error styling', () => {
      // Arrange
      const message = 'Operation failed';
      
      // Act
      service.error(message);
      
      // Assert
      expect(snackBarSpy.open).toHaveBeenCalledWith(
        message,
        'Close',
        jasmine.objectContaining({
          duration: 5000, // Longer duration for errors
          panelClass: ['error-snackbar']
        })
      );
    });
  });

  /**
   * Test info notification functionality
   */
  describe('info notification', () => {
    it('should open a snackbar with info styling', () => {
      // Arrange
      const message = 'Operation in progress';
      
      // Act
      service.info(message);
      
      // Assert
      expect(snackBarSpy.open).toHaveBeenCalledWith(
        message,
        'Close',
        jasmine.objectContaining({
          duration: 3000,
          panelClass: ['info-snackbar']
        })
      );
    });
  });

  /**
   * Test warning notification functionality
   */
  describe('warning notification', () => {
    it('should open a snackbar with warning styling', () => {
      // Arrange
      const message = 'Proceed with caution';
      
      // Act
      service.warning(message);
      
      // Assert
      expect(snackBarSpy.open).toHaveBeenCalledWith(
        message,
        'Close',
        jasmine.objectContaining({
          duration: 4000,
          panelClass: ['warning-snackbar']
        })
      );
    });
  });
});
