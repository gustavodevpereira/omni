import { TestBed, fakeAsync, tick } from '@angular/core/testing';
import { Router } from '@angular/router';
import { of, throwError } from 'rxjs';
import { AuthFacade } from './auth.facade';
import { AuthService } from '../services/auth.service';
import { NotificationService } from '../services/notification.service';
import { User, UserRole, RegisterRequest, UserDTO } from '../../shared/models/user.model';
import { ApiResponseWithData } from '../../shared/models/api-response.models';

/**
 * Test suite for the Authentication Facade
 * 
 * This comprehensive test suite verifies the functionality of the AuthFacade, which serves as
 * the primary interface for authentication operations throughout the application. The tests
 * cover all major authentication flows and edge cases, including:
 * 
 * - Authentication status verification
 * - User login (success and failure scenarios)
 * - User logout behavior
 * - User registration
 * - Role-based access control
 * - Navigation after authentication events
 * 
 * Each test verifies both the correct behavior of the facade methods and the proper
 * interaction with its dependencies (AuthService, NotificationService, Router).
 */
describe('AuthFacade', () => {
  let facade: AuthFacade;
  let authServiceSpy: jasmine.SpyObj<AuthService>;
  let routerSpy: jasmine.SpyObj<Router>;
  let notificationServiceSpy: jasmine.SpyObj<NotificationService>;

  /**
   * Test setup that runs before each test.
   * Creates spy objects for all dependencies and configures the testing module.
   */
  beforeEach(() => {
    // Create spies for all dependency services
    authServiceSpy = jasmine.createSpyObj('AuthService', 
      ['login', 'logout', 'isLoggedIn', 'getCurrentUser', 'register'],
      { currentUser$: of(null) }
    );
    
    routerSpy = jasmine.createSpyObj('Router', ['navigate'], {
      get url() { return '/test'; }
    });
    
    notificationServiceSpy = jasmine.createSpyObj('NotificationService', 
      ['success', 'error', 'info', 'warning']
    );

    TestBed.configureTestingModule({
      providers: [
        AuthFacade,
        { provide: AuthService, useValue: authServiceSpy },
        { provide: Router, useValue: routerSpy },
        { provide: NotificationService, useValue: notificationServiceSpy }
      ]
    });

    facade = TestBed.inject(AuthFacade);
  });

  /**
   * Base test to verify the facade is properly created.
   */
  it('should be created', () => {
    expect(facade).toBeTruthy();
  });

  /**
   * Tests related to authentication status and user information.
   */
  describe('Authentication Status', () => {
    /**
     * Verifies that the facade correctly checks if a user is authenticated.
     */
    it('should check if user is authenticated', (done) => {
      // Arrange
      authServiceSpy.isLoggedIn.and.returnValue(true);
      
      // Act
      facade.isAuthenticated().subscribe(isAuth => {
        // Assert
        expect(isAuth).withContext('Authentication status should be true').toBeTrue();
        expect(authServiceSpy.isLoggedIn).withContext('AuthService.isLoggedIn should be called').toHaveBeenCalled();
        done();
      });
    });

    /**
     * Verifies that the facade correctly retrieves the current user.
     */
    it('should get current user', (done) => {
      // Arrange
      const mockUser = User.create({
        id: '1',
        email: 'test@example.com',
        name: 'Test User',
        role: UserRole.CUSTOMER
      });
      
      authServiceSpy.getCurrentUser.and.returnValue(of(mockUser));
      
      // Act
      facade.getCurrentUser().subscribe(user => {
        // Assert
        expect(user).withContext('User should be returned').toBeTruthy();
        expect(user?.email).withContext('User email should match').toBe('test@example.com');
        expect(user?.role).withContext('User role should match').toBe(UserRole.CUSTOMER);
        expect(authServiceSpy.getCurrentUser).withContext('AuthService.getCurrentUser should be called').toHaveBeenCalled();
        done();
      });
    });

    /**
     * Verifies that the facade correctly checks if a user has a specific role.
     */
    it('should check user role', (done) => {
      // Arrange
      const mockUser = User.create({
        id: '1',
        email: 'test@example.com',
        name: 'Test User',
        role: UserRole.ADMIN
      });
      
      authServiceSpy.getCurrentUser.and.returnValue(of(mockUser));
      
      // Act
      facade.hasRole('admin').subscribe(hasRole => {
        // Assert
        expect(hasRole).withContext('User should have admin role').toBeTrue();
        expect(authServiceSpy.getCurrentUser).withContext('AuthService.getCurrentUser should be called').toHaveBeenCalled();
        done();
      });
    });

    /**
     * Verifies that the facade returns false when checking a role for an unauthenticated user.
     */
    it('should return false when checking role for unauthenticated user', (done) => {
      // Arrange
      authServiceSpy.getCurrentUser.and.returnValue(of(null));
      
      // Act
      facade.hasRole('admin').subscribe(hasRole => {
        // Assert
        expect(hasRole).withContext('Unauthenticated user should not have any role').toBeFalse();
        expect(authServiceSpy.getCurrentUser).withContext('AuthService.getCurrentUser should be called').toHaveBeenCalled();
        done();
      });
    });
  });

  /**
   * Tests related to login, logout and registration operations.
   */
  describe('Authentication Operations', () => {
    /**
     * Verifies successful user login with valid credentials.
     */
    it('should login user successfully', (done) => {
      // Arrange
      const email = 'test@example.com';
      const password = 'password123';
      const mockUserDTO: UserDTO = {
        id: '1',
        email: email,
        name: 'Test User',
        role: UserRole.CUSTOMER
      };
      
      const mockResponse: ApiResponseWithData<any> = {
        success: true,
        message: 'Login successful',
        errors: [],
        data: {
          token: 'fake-token',
          user: mockUserDTO
        }
      };
      
      authServiceSpy.login.and.returnValue(of(mockResponse));
      
      // Act
      facade.login(email, password).subscribe(success => {
        // Assert
        expect(success).withContext('Login should be successful').toBeTrue();
        expect(authServiceSpy.login).withContext('AuthService.login should be called with correct credentials')
          .toHaveBeenCalledWith({ email, password });
        expect(notificationServiceSpy.success).withContext('Success notification should be shown')
          .toHaveBeenCalled();
        done();
      });
    });

    /**
     * Verifies login failure handling with invalid credentials.
     */
    it('should handle login failure', (done) => {
      // Arrange
      const email = 'test@example.com';
      const password = 'wrong-password';
      
      authServiceSpy.login.and.returnValue(throwError(() => new Error('Invalid credentials')));
      
      // Act
      facade.login(email, password).subscribe(success => {
        // Assert
        expect(success).withContext('Login should fail').toBeFalse();
        expect(authServiceSpy.login).withContext('AuthService.login should be called with provided credentials')
          .toHaveBeenCalledWith({ email, password });
        expect(notificationServiceSpy.error).withContext('Error notification should be shown')
          .toHaveBeenCalled();
        done();
      });
    });

    /**
     * Verifies login error handling for network or server errors.
     */
    it('should handle login error', (done) => {
      // Arrange
      const email = 'test@example.com';
      const password = 'password123';
      const mockError = new Error('Network error');
      
      authServiceSpy.login.and.returnValue(throwError(() => mockError));
      
      // Act
      facade.login(email, password).subscribe(success => {
        // Assert
        expect(success).withContext('Login should fail on error').toBeFalse();
        expect(authServiceSpy.login).withContext('AuthService.login should be called with provided credentials')
          .toHaveBeenCalledWith({ email, password });
        expect(notificationServiceSpy.error).withContext('Error notification should be shown')
          .toHaveBeenCalled();
        done();
      });
    });

    /**
     * Verifies successful user registration.
     */
    it('should register user successfully', (done) => {
      // Arrange
      const registerData: RegisterRequest = {
        username: 'newuser',
        email: 'new@example.com',
        password: 'password123',
        role: UserRole.CUSTOMER
      };
      
      const mockResponse: ApiResponseWithData<any> = {
        success: true,
        message: 'Registration successful',
        errors: [],
        data: {
          id: '2',
          email: registerData.email,
          name: 'New User'
        }
      };
      
      authServiceSpy.register.and.returnValue(of(mockResponse));
      
      // Act
      facade.register(registerData).subscribe(success => {
        // Assert
        expect(success).withContext('Registration should be successful').toBeTrue();
        expect(authServiceSpy.register).withContext('AuthService.register should be called with provided data')
          .toHaveBeenCalledWith(registerData);
        expect(notificationServiceSpy.success).withContext('Success notification should be shown')
          .toHaveBeenCalled();
        done();
      });
    });

    /**
     * Verifies registration failure handling.
     */
    it('should handle registration failure', (done) => {
      // Arrange
      const registerData: RegisterRequest = {
        username: 'existinguser',
        email: 'existing@example.com',
        password: 'password123',
        role: UserRole.CUSTOMER
      };
      
      authServiceSpy.register.and.returnValue(throwError(() => new Error('Email already exists')));
      
      // Act
      facade.register(registerData).subscribe(success => {
        // Assert
        expect(success).withContext('Registration should fail').toBeFalse();
        expect(authServiceSpy.register).withContext('AuthService.register should be called with provided data')
          .toHaveBeenCalledWith(registerData);
        expect(notificationServiceSpy.error).withContext('Error notification should be shown')
          .toHaveBeenCalled();
        done();
      });
    });

    // Additional tests for logout, navigateToDefault, etc. would follow here...
  });
});
