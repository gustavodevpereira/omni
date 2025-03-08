import { fakeAsync, tick } from '@angular/core/testing';
import { HttpErrorResponse } from '@angular/common/http';
import { Router } from '@angular/router';
import { NgZone } from '@angular/core';
import { GlobalErrorHandler } from './global-error-handler';
import { NotificationService } from '../services/notification.service';
import { LoggingService } from '../services/logging.service';
import { AuthService } from '../services/auth.service';

/**
 * Test suite for the GlobalErrorHandler class.
 * 
 * Tests the central error handling logic for both HTTP and application errors,
 * verifying that errors are correctly processed, logged, and displayed to users.
 */
describe('GlobalErrorHandler', () => {
  let errorHandler: GlobalErrorHandler;
  let notificationServiceSpy: jasmine.SpyObj<NotificationService>;
  let loggingServiceSpy: jasmine.SpyObj<LoggingService>;
  let routerSpy: jasmine.SpyObj<Router>;
  let authServiceSpy: jasmine.SpyObj<AuthService>;
  let ngZoneSpy: jasmine.SpyObj<NgZone>;

  /**
   * Setup before each test.
   * Creates spies for all dependencies and manually instantiates the handler.
   */
  beforeEach(() => {
    // Create spy objects for all dependencies
    notificationServiceSpy = jasmine.createSpyObj('NotificationService', ['error', 'warning', 'info']);
    loggingServiceSpy = jasmine.createSpyObj('LoggingService', ['logError']);
    authServiceSpy = jasmine.createSpyObj('AuthService', ['logout']);
    routerSpy = jasmine.createSpyObj('Router', ['navigate'], {
      url: '/test-url'
    });
    
    // Create NgZone spy that actually executes the callbacks
    ngZoneSpy = jasmine.createSpyObj('NgZone', ['run']);
    ngZoneSpy.run.and.callFake((fn: Function) => fn());

    // Manually create an instance of the error handler
    errorHandler = new GlobalErrorHandler(
      notificationServiceSpy,
      loggingServiceSpy,
      routerSpy,
      authServiceSpy,
      ngZoneSpy
    );
  });

  /**
   * Basic test to verify the error handler can be instantiated.
   */
  it('should be created', () => {
    expect(errorHandler).toBeTruthy();
  });

  /**
   * Tests for HTTP error handling with different status codes.
   */
  describe('HTTP error handling', () => {
    /**
     * Tests handling of 401 Unauthorized errors, verifying logout and redirection.
     */
    it('should handle 401 Unauthorized errors', fakeAsync(() => {
      // Arrange
      const httpError = new HttpErrorResponse({
        error: 'Unauthorized',
        status: 401,
        statusText: 'Unauthorized'
      });
      
      // Act
      errorHandler.handleError(httpError);
      tick();
      
      // Assert
      expect(authServiceSpy.logout).toHaveBeenCalled();
      expect(routerSpy.navigate).toHaveBeenCalledWith(['/auth/login']);
      expect(notificationServiceSpy.error).toHaveBeenCalled();
    }));

    /**
     * Tests handling of 403 Forbidden errors, verifying redirection.
     */
    it('should handle 403 Forbidden errors', fakeAsync(() => {
      // Arrange
      const httpError = new HttpErrorResponse({
        error: 'Forbidden',
        status: 403,
        statusText: 'Forbidden'
      });
      
      // Act
      errorHandler.handleError(httpError);
      tick();
      
      // Assert
      expect(notificationServiceSpy.error).toHaveBeenCalledWith(
        'You do not have permission to perform this action.'
      );
      expect(loggingServiceSpy.logError).toHaveBeenCalled();
    }));

    /**
     * Tests handling of 500 Server Error responses.
     */
    it('should handle 500 Server errors', fakeAsync(() => {
      // Arrange
      const httpError = new HttpErrorResponse({
        error: 'Server Error',
        status: 500,
        statusText: 'Internal Server Error'
      });
      
      // Act
      errorHandler.handleError(httpError);
      tick();
      
      // Assert
      expect(notificationServiceSpy.error).toHaveBeenCalled();
      expect(loggingServiceSpy.logError).toHaveBeenCalled();
    }));

    /**
     * Tests handling of 400 Bad Request errors, including validation errors.
     */
    it('should handle validation errors', fakeAsync(() => {
      // Arrange
      const validationErrors = {
        errors: [
          { 
            propertyName: 'email',
            errorMessage: 'Invalid email format',
            attemptedValue: 'invalid-email',
            errorCode: 'EmailValidator'
          }
        ]
      };
      
      const httpError = new HttpErrorResponse({
        error: validationErrors,
        status: 400,
        statusText: 'Bad Request'
      });
      
      // Act
      errorHandler.handleError(httpError);
      tick();
      
      // Assert - validation only checks the notification, not the log
      expect(notificationServiceSpy.error).toHaveBeenCalled();
    }));
  });

  /**
   * Tests for client-side application error handling.
   */
  describe('Application error handling', () => {
    /**
     * Tests handling of general application errors.
     */
    it('should properly handle general application errors', fakeAsync(() => {
      // Arrange
      const appError = new Error('General application error');
      
      // Act
      errorHandler.handleError(appError);
      tick();
      
      // Assert
      expect(loggingServiceSpy.logError).toHaveBeenCalled();
      expect(notificationServiceSpy.error).toHaveBeenCalled();
    }));

    /**
     * Tests specific handling of TypeError instances.
     */
    it('should handle TypeError specifically', fakeAsync(() => {
      // Arrange
      const typeError = new TypeError('Type error');
      
      // Act
      errorHandler.handleError(typeError);
      tick();
      
      // Assert
      expect(loggingServiceSpy.logError).toHaveBeenCalled();
      expect(notificationServiceSpy.error).toHaveBeenCalled();
    }));
  });
});
