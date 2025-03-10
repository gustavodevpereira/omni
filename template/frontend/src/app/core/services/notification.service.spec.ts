import { TestBed } from '@angular/core/testing';
import { MatSnackBar, MatSnackBarRef } from '@angular/material/snack-bar';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';

import { NotificationService, NotificationType } from './notification.service';

/**
 * Unit tests for NotificationService
 */
describe('NotificationService', () => {
  let service: NotificationService;
  let snackBarMock: jasmine.SpyObj<MatSnackBar>;
  let snackBarRefMock: jasmine.SpyObj<MatSnackBarRef<any>>;

  beforeEach(() => {
    // Create spy object for the MatSnackBar
    snackBarRefMock = jasmine.createSpyObj('MatSnackBarRef', ['dismiss']);
    snackBarMock = jasmine.createSpyObj('MatSnackBar', ['open']);
    snackBarMock.open.and.returnValue(snackBarRefMock);

    TestBed.configureTestingModule({
      imports: [
        NoopAnimationsModule
      ],
      providers: [
        NotificationService,
        { provide: MatSnackBar, useValue: snackBarMock }
      ]
    });

    service = TestBed.inject(NotificationService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  /**
   * Test success notification
   */
  it('should display success notification', () => {
    const message = 'Success message';
    const action = 'OK';
    
    service.success(message, action);
    
    expect(snackBarMock.open).toHaveBeenCalledWith(
      message, 
      action, 
      jasmine.objectContaining({ 
        panelClass: ['notification-success']
      })
    );
  });

  /**
   * Test error notification
   */
  it('should display error notification', () => {
    const message = 'Error message';
    
    service.error(message);
    
    expect(snackBarMock.open).toHaveBeenCalledWith(
      message, 
      'Close', 
      jasmine.objectContaining({ 
        panelClass: ['notification-error']
      })
    );
  });

  /**
   * Test info notification
   */
  it('should display info notification', () => {
    const message = 'Info message';
    
    service.info(message);
    
    expect(snackBarMock.open).toHaveBeenCalledWith(
      message, 
      'Close', 
      jasmine.objectContaining({ 
        panelClass: ['notification-info']
      })
    );
  });

  /**
   * Test warning notification
   */
  it('should display warning notification', () => {
    const message = 'Warning message';
    
    service.warning(message);
    
    expect(snackBarMock.open).toHaveBeenCalledWith(
      message, 
      'Close', 
      jasmine.objectContaining({ 
        panelClass: ['notification-warning']
      })
    );
  });

  /**
   * Test notify method with different notification types
   */
  it('should handle different notification types with notify method', () => {
    const message = 'Test message';
    
    // Test each notification type
    service.notify(message, NotificationType.SUCCESS);
    expect(snackBarMock.open).toHaveBeenCalledWith(
      message, 
      'Close', 
      jasmine.objectContaining({ 
        panelClass: ['notification-success']
      })
    );
    
    service.notify(message, NotificationType.ERROR);
    expect(snackBarMock.open).toHaveBeenCalledWith(
      message, 
      'Close', 
      jasmine.objectContaining({ 
        panelClass: ['notification-error']
      })
    );
    
    service.notify(message, NotificationType.INFO);
    expect(snackBarMock.open).toHaveBeenCalledWith(
      message, 
      'Close', 
      jasmine.objectContaining({ 
        panelClass: ['notification-info']
      })
    );
    
    service.notify(message, NotificationType.WARNING);
    expect(snackBarMock.open).toHaveBeenCalledWith(
      message, 
      'Close', 
      jasmine.objectContaining({ 
        panelClass: ['notification-warning']
      })
    );
  });

  /**
   * Test dismiss method
   */
  it('should dismiss active notification', () => {
    // First, show a notification to create an active reference
    service.info('Test message');
    
    // Then dismiss it
    service.dismiss();
    
    // Verify dismiss was called on the reference
    expect(snackBarRefMock.dismiss).toHaveBeenCalled();
  });

  /**
   * Test dismiss with no active notification
   */
  it('should not throw error when dismissing with no active notification', () => {
    // Manually set the internal property to null
    // This is a bit of a hack, but it's the most straightforward way to test this scenario
    (service as any).activeSnackBarRef = null;
    
    // This should not throw an error
    expect(() => service.dismiss()).not.toThrow();
  });

  /**
   * Test custom configuration
   */
  it('should accept custom configuration', () => {
    const message = 'Custom config message';
    const customConfig = {
      duration: 10000,
      horizontalPosition: 'start' as const
    };
    
    service.success(message, 'Close', customConfig);
    
    expect(snackBarMock.open).toHaveBeenCalledWith(
      message, 
      'Close', 
      jasmine.objectContaining({ 
        duration: 10000,
        horizontalPosition: 'start',
        panelClass: ['notification-success']
      })
    );
  });

  /**
   * Test that new notification dismisses previous one
   */
  it('should dismiss previous notification when showing a new one', () => {
    // Show first notification
    service.info('First message');
    
    // Reset call count for clearer assertions
    snackBarRefMock.dismiss.calls.reset();
    
    // Show second notification
    service.info('Second message');
    
    // Verify dismiss was called before showing the second notification
    expect(snackBarRefMock.dismiss).toHaveBeenCalled();
    expect(snackBarMock.open).toHaveBeenCalledTimes(2);
  });
}); 