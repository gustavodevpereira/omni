import { TestBed } from '@angular/core/testing';
import { Router } from '@angular/router';
import { of, throwError } from 'rxjs';

import { AuthService } from './auth.service';
import { AuthApiService } from '../api/services/auth-api.service';
import { AuthRequests } from '../api/models/requests.model';
import { AuthResponses } from '../api/models/responses.model';
import { UserRole, UserStatus } from '../api/models/domain.model';

/**
 * Unit tests for AuthService
 */
describe('AuthService', () => {
  let service: AuthService;
  let authApiServiceMock: jasmine.SpyObj<AuthApiService>;
  let routerMock: jasmine.SpyObj<Router>;
  
  // Mock data
  const mockAuthResult: AuthResponses.AuthResult = {
    token: 'test-jwt-token',
    email: 'test@example.com',
    name: 'Test User',
    role: 'Customer'
  };
  
  const mockLoginData: AuthRequests.Login = {
    email: 'test@example.com',
    password: 'password123',
    rememberMe: true
  };
  
  const mockRegisterData: AuthRequests.Register = {
    email: 'test@example.com',
    password: 'password123',
    name: 'Test User',
    confirmPassword: 'password123',
    phone: '1234567890'
  };

  // Mock localStorage
  let localStorageMock: { [key: string]: string } = {};
  
  beforeEach(() => {
    // Reset storage mock
    localStorageMock = {};
    
    // Create localStorage mock
    spyOn(localStorage, 'getItem').and.callFake((key) => localStorageMock[key] || null);
    spyOn(localStorage, 'setItem').and.callFake((key, value) => localStorageMock[key] = value);
    spyOn(localStorage, 'removeItem').and.callFake((key) => delete localStorageMock[key]);
    
    // Create service mocks
    authApiServiceMock = jasmine.createSpyObj('AuthApiService', [
      'login', 
      'register', 
      'forgotPassword', 
      'resetPassword'
    ]);
    
    routerMock = jasmine.createSpyObj('Router', ['navigate']);
    
    // Set up default mock returns
    authApiServiceMock.login.and.returnValue(of(mockAuthResult));
    authApiServiceMock.register.and.returnValue(of(mockAuthResult));
    authApiServiceMock.forgotPassword.and.returnValue(of('Reset link sent'));
    authApiServiceMock.resetPassword.and.returnValue(of('Password reset successful'));

    TestBed.configureTestingModule({
      providers: [
        AuthService,
        { provide: AuthApiService, useValue: authApiServiceMock },
        { provide: Router, useValue: routerMock }
      ]
    });

    service = TestBed.inject(AuthService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  /**
   * Test login functionality
   */
  it('should login successfully', (done) => {
    service.login(mockLoginData.email, mockLoginData.password, mockLoginData.rememberMe).subscribe(user => {
      // Verify user data from API is returned
      expect(user).toBeTruthy();
      expect(user.email).toEqual(mockAuthResult.email);
      expect(user.name).toEqual(mockAuthResult.name);
      expect(user.role).toEqual(mockAuthResult.role as UserRole);
      
      // Verify authentication state is updated
      service.isAuthenticated$.subscribe(isAuth => {
        expect(isAuth).toBeTrue();
        
        // Verify token is stored in localStorage
        expect(localStorage.setItem).toHaveBeenCalledWith('auth_token', mockAuthResult.token);
        expect(localStorage.setItem).toHaveBeenCalledWith('current_user', jasmine.any(String));
        
        done();
      });
    });
    
    // Verify API was called with correct data
    expect(authApiServiceMock.login).toHaveBeenCalledWith(mockLoginData);
  });

  /**
   * Test login error handling
   */
  it('should handle login errors', (done) => {
    // Set up API to return error
    const errorMsg = 'Invalid credentials';
    authApiServiceMock.login.and.returnValue(throwError(() => new Error(errorMsg)));
    
    service.login(mockLoginData.email, mockLoginData.password).subscribe(
      () => {
        fail('Should have failed with error');
      },
      (error) => {
        expect(error).toBeTruthy();
        expect(error.message).toEqual(errorMsg);
        
        // Verify authentication state is not updated
        service.isAuthenticated$.subscribe(isAuth => {
          expect(isAuth).toBeFalse();
          expect(localStorage.setItem).not.toHaveBeenCalledWith('auth_token', jasmine.any(String));
          done();
        });
      }
    );
  });

  /**
   * Test register functionality
   */
  it('should register successfully', (done) => {
    service.register(mockRegisterData).subscribe(user => {
      // Verify user data from API is returned
      expect(user).toBeTruthy();
      expect(user.email).toEqual(mockAuthResult.email);
      expect(user.name).toEqual(mockAuthResult.name);
      
      // Verify authentication state is updated
      service.isAuthenticated$.subscribe(isAuth => {
        expect(isAuth).toBeTrue();
        
        // Verify token is stored
        expect(localStorage.setItem).toHaveBeenCalledWith('auth_token', mockAuthResult.token);
        expect(localStorage.setItem).toHaveBeenCalledWith('current_user', jasmine.any(String));
        
        done();
      });
    });
    
    // Verify API was called with correct data
    expect(authApiServiceMock.register).toHaveBeenCalledWith(mockRegisterData);
  });

  /**
   * Test register error handling
   */
  it('should handle register errors', (done) => {
    // Set up API to return error
    const errorMsg = 'Email already in use';
    authApiServiceMock.register.and.returnValue(throwError(() => new Error(errorMsg)));
    
    service.register(mockRegisterData).subscribe(
      () => {
        fail('Should have failed with error');
      },
      (error) => {
        expect(error).toBeTruthy();
        expect(error.message).toEqual(errorMsg);
        
        // Verify authentication state is not updated
        service.isAuthenticated$.subscribe(isAuth => {
          expect(isAuth).toBeFalse();
          expect(localStorage.setItem).not.toHaveBeenCalledWith('auth_token', jasmine.any(String));
          done();
        });
      }
    );
  });

  /**
   * Test logout functionality
   */
  it('should logout correctly', () => {
    // First set authenticated state
    localStorageMock['auth_token'] = 'test-token';
    localStorageMock['current_user'] = JSON.stringify({
      email: 'test@example.com',
      name: 'Test User',
      role: UserRole.Customer,
      status: UserStatus.Active
    });
    
    // Call the method being tested
    service.logout();
    
    // Verify localStorage items are removed
    expect(localStorage.removeItem).toHaveBeenCalledWith('auth_token');
    expect(localStorage.removeItem).toHaveBeenCalledWith('current_user');
    
    // Verify authentication state is updated
    service.isAuthenticated$.subscribe(isAuth => {
      expect(isAuth).toBeFalse();
    });
    
    // Verify redirection to login page
    expect(routerMock.navigate).toHaveBeenCalledWith(['/auth/login']);
  });

  /**
   * Test getCurrentUser method
   */
  it('should return current user', () => {
    // Initialize with mock user
    (service as any).currentUserSubject.next({
      id: '123',
      email: 'test@example.com',
      name: 'Test User',
      phone: '',
      role: UserRole.Customer,
      status: UserStatus.Active
    });
    
    const user = service.getCurrentUser();
    expect(user).toBeTruthy();
    expect(user?.email).toEqual('test@example.com');
    expect(user?.name).toEqual('Test User');
  });

  /**
   * Test isAuthenticated method
   */
  it('should return authentication state', () => {
    // Initially not authenticated
    expect(service.isAuthenticated()).toBeFalse();
    
    // Set authenticated
    (service as any).isAuthenticatedSubject.next(true);
    
    // Should now return true
    expect(service.isAuthenticated()).toBeTrue();
  });

  /**
   * Test loading from localStorage on initialization
   */
  it('should load user state from localStorage on init', () => {
    // Setup existing data in localStorage
    const mockStoredUser = {
      id: 'stored-id',
      email: 'stored@example.com',
      name: 'Stored User',
      phone: '',
      role: UserRole.Customer,
      status: UserStatus.Active
    };
    
    localStorageMock['auth_token'] = 'stored-token';
    localStorageMock['current_user'] = JSON.stringify(mockStoredUser);
    
    // Create a new instance which will check localStorage in constructor
    TestBed.resetTestingModule();
    TestBed.configureTestingModule({
      providers: [
        AuthService,
        { provide: AuthApiService, useValue: authApiServiceMock },
        { provide: Router, useValue: routerMock }
      ]
    });
    
    const newService = TestBed.inject(AuthService);
    
    // Verify the service loaded the user from localStorage
    expect(newService.isAuthenticated()).toBeTrue();
    expect(newService.getCurrentUser()).toEqual(mockStoredUser);
  });

  /**
   * Test forgot password functionality
   */
  it('should request password reset', (done) => {
    const email = 'reset@example.com';
    const expectedMessage = 'Reset link sent';
    
    service.forgotPassword(email).subscribe(message => {
      expect(message).toEqual(expectedMessage);
      done();
    });
    
    expect(authApiServiceMock.forgotPassword).toHaveBeenCalledWith({ email });
  });

  /**
   * Test reset password functionality
   */
  it('should reset password', (done) => {
    const resetData = {
      token: 'reset-token',
      password: 'new-password',
      confirmPassword: 'new-password'
    };
    const expectedMessage = 'Password reset successful';
    
    service.resetPassword(resetData.token, resetData.password, resetData.confirmPassword).subscribe(message => {
      expect(message).toEqual(expectedMessage);
      done();
    });
    
    expect(authApiServiceMock.resetPassword).toHaveBeenCalledWith(resetData);
  });

  /**
   * Test handling of invalid localStorage data
   */
  it('should handle invalid localStorage data', () => {
    // Set invalid data in localStorage
    localStorageMock['auth_token'] = 'valid-token';
    localStorageMock['current_user'] = 'not-valid-json';
    
    // Create a new instance which will check localStorage in constructor
    TestBed.resetTestingModule();
    TestBed.configureTestingModule({
      providers: [
        AuthService,
        { provide: AuthApiService, useValue: authApiServiceMock },
        { provide: Router, useValue: routerMock }
      ]
    });
    
    const newService = TestBed.inject(AuthService);
    
    // Verify the service handled the error and is not authenticated
    expect(newService.isAuthenticated()).toBeFalse();
    expect(newService.getCurrentUser()).toBeNull();
    
    // localStorage should be cleared
    expect(localStorage.removeItem).toHaveBeenCalledWith('auth_token');
    expect(localStorage.removeItem).toHaveBeenCalledWith('current_user');
  });
}); 