import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, of, catchError, tap, map } from 'rxjs';
import { 
  AuthRequest, 
  AuthResponse, 
  RegisterRequest, 
  User,
  UserDTO,
  UserRole,
  UserProps
} from '../../shared/models/user.model';
import { ApiResponseWithData } from '../../shared/models/api-response.models';
import { Router } from '@angular/router';
import { environment } from '../../../environments/environment';
import { LoggingService } from './logging.service';
import { NotificationService } from './notification.service';

/**
 * Service responsible for authentication and user management.
 * 
 * @description
 * This service manages all aspects of user authentication and profile management including:
 * - User login and JWT token handling
 * - Registration of new users
 * - Profile management (retrieve, update, delete)
 * - Authentication state tracking via Observable
 * - Secure token storage and retrieval
 * 
 * The service provides a reactive currentUser$ Observable that components
 * can subscribe to for real-time updates on authentication status.
 * 
 * @example
 * // Subscribe to auth state
 * this.authService.currentUser$.subscribe(user => {
 *   this.isLoggedIn = !!user;
 *   this.userName = user?.name;
 * });
 * 
 * // Log in a user
 * this.authService.login({email: 'user@example.com', password: 'secret'})
 *   .subscribe({
 *     next: response => this.handleSuccess(response),
 *     error: error => this.handleError(error)
 *   });
 */
@Injectable({
  providedIn: 'root'
})
export class AuthService {
  /** Subject to track and broadcast user authentication state */
  private currentUserSubject = new BehaviorSubject<User | null>(null);
  
  /** Observable of the current authenticated user */
  public currentUser$ = this.currentUserSubject.asObservable();
  
  /** Storage key for authentication token */
  private tokenKey = 'auth_token';
  
  /** Storage key for user data */
  private userKey = 'user_data';
  
  /** Base API URL from environment */
  private apiUrl = environment.apiUrl;

  /**
   * Creates an instance of AuthService.
   * 
   * @param http - HttpClient for API communication
   * @param router - Angular Router for navigation
   * @param loggingService - Service for application logging
   * @param notificationService - Service for user notifications
   */
  constructor(
    private http: HttpClient,
    private router: Router,
    private loggingService: LoggingService,
    private notificationService: NotificationService
  ) {
    this.loadUserFromStorage();
    this.loggingService.logInfo('Auth Service initialized');
  }

  /**
   * Authenticates a user with the provided credentials.
   * 
   * @param credentials - Login credentials (email and password)
   * @returns Observable of authentication response
   * 
   * @example
   * this.authService.login({
   *   email: 'user@example.com',
   *   password: 'password123'
   * }).subscribe({
   *   next: response => console.log('Login successful', response),
   *   error: error => console.error('Login failed', error)
   * });
   */
  login(credentials: AuthRequest): Observable<ApiResponseWithData<AuthResponse>> {
    this.loggingService.logInfo('Attempting login', { email: credentials.email });
    
    return this.http.post<ApiResponseWithData<AuthResponse>>(`${this.apiUrl}/auth`, credentials)
      .pipe(
        tap(response => {
          if (response.success && response.data) {
            const authData = response.data;
            
            // Store token
            localStorage.setItem(this.tokenKey, authData.token);
            
            // Create user from auth response (AuthResponse has properties directly, not nested in a user property)
            const userProps: UserProps = {
              email: authData.email,
              name: authData.name,
              role: authData.role as UserRole
            };
            
            const user = User.create(userProps);
            localStorage.setItem(this.userKey, JSON.stringify(user));
            
            // Update current user
            this.currentUserSubject.next(user);
            
            this.loggingService.logInfo('User logged in successfully', { email: authData.email });
            this.notificationService.success('Login successful');
          }
        }),
        catchError(error => {
          this.loggingService.logError('Login failed', error);
          this.notificationService.error('Login failed. Please check your credentials.');
          throw error;
        })
      );
  }

  /**
   * Registers a new user account.
   * 
   * @param registerData - Registration data for new user
   * @returns Observable of registration response
   * 
   * @example
   * this.authService.register({
   *   name: 'John Doe',
   *   email: 'john@example.com',
   *   password: 'securePassword123'
   * }).subscribe({
   *   next: response => console.log('Registration successful', response),
   *   error: error => console.error('Registration failed', error)
   * });
   */
  register(registerData: RegisterRequest): Observable<ApiResponseWithData<UserDTO>> {
    this.loggingService.logInfo('Attempting registration', { email: registerData.email });
    
    return this.http.post<ApiResponseWithData<UserDTO>>(`${this.apiUrl}/users`, registerData)
      .pipe(
        tap(response => {
          if (response.success) {
            this.loggingService.logInfo('User registered successfully');
            this.notificationService.success('Registration successful. Please log in.');
          }
        }),
        catchError(error => {
          this.loggingService.logError('Registration failed', error);
          this.notificationService.error('Registration failed. Please try again.');
          throw error;
        })
      );
  }

  /**
   * Gets the current authenticated user.
   * 
   * @returns Observable of the current user or null if not authenticated
   * 
   * @example
   * this.authService.getCurrentUser().subscribe(user => {
   *   if (user) {
   *     this.userName = user.name;
   *   }
   * });
   */
  getCurrentUser(): Observable<User | null> {
    return this.currentUser$;
  }

  /**
   * Retrieves a user by their ID.
   * 
   * @param userId - ID of user to retrieve
   * @returns Observable of user data response
   * 
   * @example
   * this.authService.getUserById('123').subscribe(response => {
   *   if (response.success) {
   *     this.userData = response.data;
   *   }
   * });
   */
  getUserById(userId: string): Observable<ApiResponseWithData<UserDTO>> {
    this.loggingService.logInfo('Fetching user data', { userId });
    
    return this.http.get<ApiResponseWithData<UserDTO>>(`${this.apiUrl}/users/${userId}`);
  }

  /**
   * Updates user information.
   * 
   * @param userId - ID of user to update
   * @param userData - Partial user data to update
   * @returns Observable of updated user data response
   * 
   * @example
   * this.authService.updateUser('123', { name: 'New Name' }).subscribe(response => {
   *   if (response.success) {
   *     this.userData = response.data;
   *   }
   * });
   */
  updateUser(userId: string, userData: Partial<UserDTO>): Observable<ApiResponseWithData<UserDTO>> {
    this.loggingService.logInfo('Updating user data', { userId });
    
    return this.http.put<ApiResponseWithData<UserDTO>>(`${this.apiUrl}/users/${userId}`, userData)
      .pipe(
        tap(response => {
          if (response.success && response.data) {
            // If updating current user, update stored data
            const currentUser = this.currentUserSubject.getValue();
            if (currentUser && currentUser.id === userId) {
              // Create a new User instance from the DTO
              const updatedUser = User.fromDTO(response.data);
              
              // Update storage and subject
              localStorage.setItem(this.userKey, JSON.stringify(updatedUser));
              this.currentUserSubject.next(updatedUser);
            }
            
            this.notificationService.success('Profile updated successfully');
          }
        })
      );
  }

  /**
   * Deletes a user account.
   * 
   * @param userId - ID of user to delete
   * @returns Observable of deletion response
   * 
   * @example
   * this.authService.deleteUser('123').subscribe(response => {
   *   if (response.success) {
   *     this.router.navigate(['/register']);
   *   }
   * });
   */
  deleteUser(userId: string): Observable<ApiResponseWithData<null>> {
    this.loggingService.logInfo('Deleting user account', { userId });
    
    return this.http.delete<ApiResponseWithData<null>>(`${this.apiUrl}/users/${userId}`)
      .pipe(
        tap(response => {
          if (response.success) {
            // If deleting current user, log them out
            const currentUser = this.currentUserSubject.getValue();
            if (currentUser && currentUser.id === userId) {
              this.logout();
            }
            
            this.notificationService.success('Account deleted successfully');
          }
        })
      );
  }

  /**
   * Logs out the current user.
   * Clears storage and navigates to login page.
   * 
   * @example
   * // User clicks logout button
   * onLogout() {
   *   this.authService.logout();
   * }
   */
  logout(): void {
    this.loggingService.logInfo('User logged out');
    
    // Clear storage
    localStorage.removeItem(this.tokenKey);
    localStorage.removeItem(this.userKey);
    
    // Update authentication state
    this.currentUserSubject.next(null);
    
    // Navigate to login page
    this.router.navigate(['/auth/login']);
    
    // Notify user
    this.notificationService.info('You have been logged out');
  }

  /**
   * Retrieves the stored JWT token.
   * 
   * @returns The authentication token or null if not available
   */
  getToken(): string | null {
    return localStorage.getItem(this.tokenKey);
  }

  /**
   * Checks if the user is currently logged in.
   * 
   * @returns Boolean indicating if user is authenticated
   * 
   * @example
   * if (this.authService.isLoggedIn()) {
   *   this.showSecureContent();
   * }
   */
  isLoggedIn(): boolean {
    return !!this.getToken();
  }

  /**
   * Loads user data from local storage.
   * Used during service initialization.
   * 
   * @private
   */
  private loadUserFromStorage(): void {
    try {
      const userJson = localStorage.getItem(this.userKey);
      const token = this.getToken();
      
      if (userJson && token) {
        const userData = JSON.parse(userJson);
        
        // Create User instance from stored data
        const userProps: UserProps = {
          id: userData.id,
          email: userData.email,
          name: userData.name,
          role: userData.role
        };
        
        const user = User.create(userProps);
        this.currentUserSubject.next(user);
        this.loggingService.logInfo('User data loaded from storage');
      }
    } catch (error) {
      this.loggingService.logError('Error loading user data from storage', error);
      localStorage.removeItem(this.userKey);
      localStorage.removeItem(this.tokenKey);
    }
  }

  /**
   * Checks if the current user has a specific role.
   * 
   * @param role - Role to check for
   * @returns Boolean indicating if user has the role
   * 
   * @example
   * if (this.authService.hasRole(UserRole.ADMIN)) {
   *   this.showAdminFeatures();
   * }
   */
  hasRole(role: UserRole): boolean {
    const user = this.currentUserSubject.getValue();
    return !!user && user.role === role;
  }
} 