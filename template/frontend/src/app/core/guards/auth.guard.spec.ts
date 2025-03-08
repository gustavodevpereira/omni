import { TestBed } from '@angular/core/testing';
import { ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Router } from '@angular/router';
import { authGuard } from './auth.guard';
import { AuthService } from '../services/auth.service';
import { LoggingService } from '../services/logging.service';
import { NotificationService } from '../services/notification.service';

/**
 * @description
 * Test suite for Authentication Guard
 * 
 * This suite verifies the behavior of the route guard that protects authenticated routes.
 * It tests that:
 * - Authenticated users are granted access to protected routes
 * - Unauthenticated users are redirected to the login page
 * - Authentication attempts and results are properly logged
 * - Appropriate notifications are displayed to users
 */
describe('authGuard', () => {
  let router: jasmine.SpyObj<Router>;
  let authService: jasmine.SpyObj<AuthService>;
  let loggingService: jasmine.SpyObj<LoggingService>;
  let notificationService: jasmine.SpyObj<NotificationService>;
  let routeSnapshot: ActivatedRouteSnapshot;
  let routeStateSnapshot: RouterStateSnapshot;

  /**
   * Setup before each test.
   * Creates spy objects for all dependencies and configures the testing module.
   */
  beforeEach(() => {
    // Create service spies
    router = jasmine.createSpyObj('Router', ['createUrlTree'], {
      url: '/protected'
    });
    
    authService = jasmine.createSpyObj('AuthService', ['isLoggedIn']);
    loggingService = jasmine.createSpyObj('LoggingService', ['logInfo', 'logWarning']);
    notificationService = jasmine.createSpyObj('NotificationService', ['warning']);
    
    // Configure route snapshots
    routeSnapshot = {} as ActivatedRouteSnapshot;
    routeStateSnapshot = { url: '/protected' } as RouterStateSnapshot;

    TestBed.configureTestingModule({
      providers: [
        { provide: Router, useValue: router },
        { provide: AuthService, useValue: authService },
        { provide: LoggingService, useValue: loggingService },
        { provide: NotificationService, useValue: notificationService }
      ]
    });
  });

  /**
   * Test case: Authenticated user access
   * 
   * Verifies that authenticated users are granted access to protected routes
   * and the access attempt is properly logged.
   */
  it('should allow access when user is logged in', () => {
    // Arrange
    authService.isLoggedIn.and.returnValue(true);
    
    // Act
    const result = TestBed.runInInjectionContext(() => 
      authGuard(routeSnapshot, routeStateSnapshot)
    );
    
    // Assert
    expect(result).withContext('Should return true for authenticated user').toBe(true);
    expect(authService.isLoggedIn).withContext('Should check if user is logged in').toHaveBeenCalled();
    expect(loggingService.logInfo).withContext('Should log authentication success').toHaveBeenCalled();
    expect(router.createUrlTree).withContext('Should not redirect authenticated user').not.toHaveBeenCalled();
  });

  /**
   * Test case: Unauthenticated user redirection
   * 
   * Verifies that unauthenticated users are redirected to the login page
   * with the original URL preserved as the return URL.
   */
  it('should redirect to login page when user is not logged in', () => {
    // Arrange
    authService.isLoggedIn.and.returnValue(false);
    
    const mockUrlTree = { toString: () => '/auth/login' } as UrlTree;
    router.createUrlTree.and.returnValue(mockUrlTree);
    
    // Act
    const result = TestBed.runInInjectionContext(() => 
      authGuard(routeSnapshot, routeStateSnapshot)
    );
    
    // Assert
    expect(result).withContext('Should return URL tree for unauthenticated user').toBe(mockUrlTree);
    expect(authService.isLoggedIn).withContext('Should check if user is logged in').toHaveBeenCalled();
    expect(notificationService.warning).withContext('Should show warning notification').toHaveBeenCalled();
    expect(router.createUrlTree).withContext('Should redirect to login page').toHaveBeenCalledWith(
      ['/auth/login'],
      jasmine.objectContaining({ 
        queryParams: { returnUrl: '/protected' }
      })
    );
  });

  /**
   * Test case: Authentication check logging
   * 
   * Verifies that the guard properly logs authentication check attempts
   * and their results.
   */
  it('should log authentication checks', () => {
    // Arrange
    authService.isLoggedIn.and.returnValue(true);
    
    // Act
    TestBed.runInInjectionContext(() => 
      authGuard(routeSnapshot, routeStateSnapshot)
    );
    
    // Assert
    expect(loggingService.logInfo).withContext('Should log authentication check').toHaveBeenCalledWith(
      jasmine.stringContaining('checking if user is logged in')
    );
    expect(loggingService.logInfo).withContext('Should log authentication success').toHaveBeenCalledWith(
      jasmine.stringContaining('authenticated, allowing access')
    );
  });

  /**
   * Test case: Failed authentication logging
   * 
   * Verifies that the guard properly logs failed authentication attempts
   * and redirects to the login page.
   */
  it('should log when authentication fails', () => {
    // Arrange
    authService.isLoggedIn.and.returnValue(false);
    
    const mockUrlTree = { toString: () => '/auth/login' } as UrlTree;
    router.createUrlTree.and.returnValue(mockUrlTree);
    
    // Act
    TestBed.runInInjectionContext(() => 
      authGuard(routeSnapshot, routeStateSnapshot)
    );
    
    // Assert
    expect(loggingService.logInfo).withContext('Should log authentication check').toHaveBeenCalledWith(
      jasmine.stringContaining('checking if user is logged in')
    );
    expect(loggingService.logWarning).withContext('Should log authentication failure').toHaveBeenCalledWith(
      jasmine.stringContaining('not authenticated, redirecting to login')
    );
  });
});
