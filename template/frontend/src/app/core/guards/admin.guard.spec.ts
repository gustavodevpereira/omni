import { TestBed } from '@angular/core/testing';
import { ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Router } from '@angular/router';
import { adminGuard } from './admin.guard';
import { AuthService } from '../services/auth.service';
import { NotificationService } from '../services/notification.service';
import { LoggingService } from '../services/logging.service';
import { of, throwError, firstValueFrom } from 'rxjs';
import { User, UserRole } from '../../shared/models/user.model';

/**
 * @description
 * Test suite for Admin Guard
 * 
 * This comprehensive test suite verifies the behavior of the route guard that protects
 * admin-only routes. It tests the following scenarios:
 * 
 * - Unauthenticated users are redirected to login with appropriate notifications
 * - Authenticated admin users are granted access to protected routes
 * - Authenticated non-admin users are denied access with appropriate notifications
 * - Error handling during permission checking is robust and user-friendly
 * - All guard operations are properly logged for monitoring and debugging
 */
describe('adminGuard', () => {
  let router: jasmine.SpyObj<Router>;
  let authService: jasmine.SpyObj<AuthService>;
  let notificationService: jasmine.SpyObj<NotificationService>;
  let loggingService: jasmine.SpyObj<LoggingService>;
  let routeSnapshot: ActivatedRouteSnapshot;
  let routeStateSnapshot: RouterStateSnapshot;

  /**
   * Setup before each test.
   * Creates spy objects for all dependencies and configures the testing module.
   */
  beforeEach(() => {
    // Create service spies
    router = jasmine.createSpyObj('Router', ['createUrlTree'], {
      url: '/admin/dashboard'
    });
    
    authService = jasmine.createSpyObj('AuthService', [
      'isLoggedIn',
      'getCurrentUser'
    ]);
    
    notificationService = jasmine.createSpyObj('NotificationService', [
      'warning',
      'error'
    ]);
    
    loggingService = jasmine.createSpyObj('LoggingService', [
      'logInfo',
      'logWarning',
      'logError'
    ]);
    
    // Configure route snapshots
    routeSnapshot = {} as ActivatedRouteSnapshot;
    routeStateSnapshot = { url: '/admin/dashboard' } as RouterStateSnapshot;

    TestBed.configureTestingModule({
      providers: [
        { provide: Router, useValue: router },
        { provide: AuthService, useValue: authService },
        { provide: NotificationService, useValue: notificationService },
        { provide: LoggingService, useValue: loggingService }
      ]
    });
  });

  /**
   * Test case: Unauthenticated user redirection
   * 
   * Verifies that when a user is not logged in:
   * - They are redirected to the login page
   * - A warning notification is displayed
   * - The return URL is preserved in query parameters
   * - The authentication check is logged
   */
  it('should redirect to login when user is not logged in', async () => {
    // Arrange
    authService.isLoggedIn.and.returnValue(false);
    
    const mockUrlTree = { toString: () => '/auth/login' } as UrlTree;
    router.createUrlTree.and.returnValue(mockUrlTree);
    
    // Act
    const result = TestBed.runInInjectionContext(() => 
      adminGuard(routeSnapshot, routeStateSnapshot)
    );
    
    // Assert - Using firstValueFrom to handle observables
    let actualResult;
    if (typeof result === 'object' && 'subscribe' in result) {
      actualResult = await firstValueFrom(result);
    } else {
      actualResult = result;
    }
    
    expect(actualResult).withContext('Should return URL tree for unauthenticated user').toBe(mockUrlTree);
    expect(authService.isLoggedIn).withContext('Should check if user is logged in').toHaveBeenCalled();
    expect(notificationService.warning).withContext('Should show warning notification').toHaveBeenCalledWith(
      'You must be logged in to access this page'
    );
    expect(router.createUrlTree).withContext('Should redirect to login page').toHaveBeenCalledWith(
      ['/auth/login'],
      jasmine.objectContaining({ 
        queryParams: { returnUrl: '/admin/dashboard' }
      })
    );
    expect(loggingService.logWarning).withContext('Should log authentication failure').toHaveBeenCalledWith(
      jasmine.stringContaining('not logged in, redirecting to login')
    );
  });

  /**
   * Test case: Admin user access
   * 
   * Verifies that users with admin role:
   * - Are granted access to protected routes
   * - No redirection occurs
   * - The access grant is logged
   */
  it('should allow access when user has admin role', async () => {
    // Arrange
    authService.isLoggedIn.and.returnValue(true);
    
    const adminUser = User.create({
      id: '1',
      email: 'admin@example.com',
      name: 'Admin User',
      role: UserRole.ADMIN
    });
    
    authService.getCurrentUser.and.returnValue(of(adminUser));
    
    // Act
    const result = TestBed.runInInjectionContext(() => 
      adminGuard(routeSnapshot, routeStateSnapshot)
    );
    
    // Assert - Using firstValueFrom to handle observables
    let actualResult;
    if (typeof result === 'object' && 'subscribe' in result) {
      actualResult = await firstValueFrom(result);
    } else {
      actualResult = result;
    }
    
    expect(actualResult).withContext('Should return true for admin user').toBe(true);
    expect(authService.isLoggedIn).withContext('Should check if user is logged in').toHaveBeenCalled();
    expect(authService.getCurrentUser).withContext('Should check user role').toHaveBeenCalled();
    expect(loggingService.logInfo).withContext('Should log admin access granted').toHaveBeenCalledWith(
      jasmine.stringContaining('has admin role, granting access')
    );
  });

  /**
   * Test case: Non-admin user access denial
   * 
   * Verifies that users without admin role:
   * - Are denied access to protected routes
   * - Are redirected to the products page
   * - Receive an error notification
   * - The access denial is logged
   */
  it('should deny access when user does not have admin role', async () => {
    // Arrange
    authService.isLoggedIn.and.returnValue(true);
    
    const regularUser = User.create({
      id: '2',
      email: 'user@example.com',
      name: 'Regular User',
      role: UserRole.CUSTOMER
    });
    
    authService.getCurrentUser.and.returnValue(of(regularUser));
    
    const mockUrlTree = { toString: () => '/products' } as UrlTree;
    router.createUrlTree.and.returnValue(mockUrlTree);
    
    // Act
    const result = TestBed.runInInjectionContext(() => 
      adminGuard(routeSnapshot, routeStateSnapshot)
    );
    
    // Assert - Using firstValueFrom to handle observables
    let actualResult;
    if (typeof result === 'object' && 'subscribe' in result) {
      actualResult = await firstValueFrom(result);
    } else {
      actualResult = result;
    }
    
    expect(actualResult).withContext('Should return URL tree for non-admin user').toBe(mockUrlTree);
    expect(authService.isLoggedIn).withContext('Should check if user is logged in').toHaveBeenCalled();
    expect(authService.getCurrentUser).withContext('Should check user role').toHaveBeenCalled();
    expect(notificationService.error).withContext('Should show error notification').toHaveBeenCalledWith(
      'You do not have permission to access this page'
    );
    expect(router.createUrlTree).withContext('Should redirect to products page').toHaveBeenCalledWith(['/products']);
    expect(loggingService.logWarning).withContext('Should log access denial').toHaveBeenCalledWith(
      jasmine.stringContaining('lacks admin role, access denied')
    );
  });

  /**
   * Test case: Error handling during permission check
   * 
   * Verifies that when an error occurs during user role checking:
   * - Access is denied (returns false)
   * - An error notification is displayed
   * - The error is properly logged
   */
  it('should handle error when checking user role', async () => {
    // Arrange
    authService.isLoggedIn.and.returnValue(true);
    const testError = new Error('Test error');
    authService.getCurrentUser.and.returnValue(throwError(() => testError));
    
    // Act
    const result = TestBed.runInInjectionContext(() => 
      adminGuard(routeSnapshot, routeStateSnapshot)
    );
    
    // Assert - Using firstValueFrom to handle observables
    let actualResult;
    if (typeof result === 'object' && 'subscribe' in result) {
      actualResult = await firstValueFrom(result);
    } else {
      actualResult = result;
    }
    
    expect(actualResult).withContext('Should return false on error').toBe(false);
    expect(authService.isLoggedIn).withContext('Should check if user is logged in').toHaveBeenCalled();
    expect(authService.getCurrentUser).withContext('Should check user role').toHaveBeenCalled();
    expect(notificationService.error).withContext('Should show error notification').toHaveBeenCalledWith(
      'Error checking permissions'
    );
    expect(loggingService.logError).withContext('Should log the error').toHaveBeenCalledWith(
      jasmine.stringContaining('Error checking admin permissions'),
      testError
    );
  });
});
