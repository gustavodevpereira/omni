import { ComponentFixture, TestBed } from '@angular/core/testing';
import { HeaderComponent } from './header.component';
import { AuthService } from '../services/auth.service';
import { BehaviorSubject } from 'rxjs';
import { User, UserRole } from '../../shared/models/user.model';
import { By } from '@angular/platform-browser';
import { provideRouter } from '@angular/router';

/**
 * @description
 * Test suite for HeaderComponent
 * 
 * This comprehensive test suite verifies the behavior and rendering of the header component
 * under different authentication states. It covers:
 * 
 * - Component creation and basic existence
 * - Logo and brand display
 * - Navigation link visibility based on authentication state
 * - User information display for authenticated users
 * - Logout functionality
 * - Conditional display of login/register links for guests
 * 
 * The tests ensure the header adjusts correctly to authentication state changes
 * and properly invokes the authentication service for user operations.
 */
describe('HeaderComponent', () => {
  let component: HeaderComponent;
  let fixture: ComponentFixture<HeaderComponent>;
  let authServiceSpy: jasmine.SpyObj<AuthService>;
  let currentUserSubject: BehaviorSubject<User | null>;
  
  /**
   * Mock user for testing authenticated user scenarios
   */
  const mockUser = User.create({
    id: '1',
    email: 'test@example.com',
    name: 'Test User',
    role: UserRole.CUSTOMER
  });

  /**
   * Test setup that runs before each test.
   * Configures the testing module and prepares spies and fixtures.
   */
  beforeEach(async () => {
    // Create a subject to control currentUser$ observable
    currentUserSubject = new BehaviorSubject<User | null>(null);
    
    // Create spy for AuthService with necessary methods and properties
    authServiceSpy = jasmine.createSpyObj('AuthService', 
      ['logout', 'isLoggedIn'],
      { currentUser$: currentUserSubject.asObservable() }
    );
    
    // Default to user not logged in
    authServiceSpy.isLoggedIn.and.returnValue(false);

    // Configure testing module
    await TestBed.configureTestingModule({
      providers: [
        provideRouter([]),
        { provide: AuthService, useValue: authServiceSpy },
      ],
      imports: [HeaderComponent]
    }).compileComponents();

    // Create component and detect changes
    fixture = TestBed.createComponent(HeaderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  /**
   * Basic test for component creation.
   */
  it('should create the component', () => {
    expect(component).withContext('Component should be created').toBeTruthy();
  });

  /**
   * Test for the logout functionality.
   */
  it('should call logout method on AuthService when logout is called', () => {
    // Act - Call the component's logout method
    component.logout();
    
    // Assert - Verify the service method was called
    expect(authServiceSpy.logout).withContext('AuthService.logout should be called').toHaveBeenCalled();
  });

  /**
   * Test suite for unauthenticated user scenarios
   */
  describe('When user is not logged in', () => {
    beforeEach(() => {
      authServiceSpy.isLoggedIn.and.returnValue(false);
      fixture.detectChanges();
    });

    /**
     * Test that login and register links are displayed for guests
     */
    it('should show login and register links', () => {
      // Find links based on template structure
      const loginLink = fixture.debugElement.query(By.css('a[routerLink="/auth/login"]'));
      const registerLink = fixture.debugElement.query(By.css('a[routerLink="/auth/register"]'));
      
      // Verify login and register links are visible
      expect(loginLink).withContext('Login link should be visible').toBeTruthy();
      expect(registerLink).withContext('Register link should be visible').toBeTruthy();
      
      // Verify user profile link is not visible
      const profileLink = fixture.debugElement.query(By.css('a[routerLink="/user/profile"]'));
      expect(profileLink).withContext('User profile link should not be visible').toBeFalsy();
    });

    /**
     * Test that product and cart links are hidden for guests
     */
    it('should hide Products and Cart links', () => {
      const productsLink = fixture.debugElement.query(By.css('a[routerLink="/products"]'));
      const cartLink = fixture.debugElement.query(By.css('a[routerLink="/cart"]'));
      
      expect(productsLink).withContext('Products link should be hidden').toBeFalsy();
      expect(cartLink).withContext('Cart link should be hidden').toBeFalsy();
    });
  });

  /**
   * Test suite for authenticated user scenarios
   */
  describe('When user is logged in', () => {
    beforeEach(() => {
      // Configure service to indicate user is logged in
      authServiceSpy.isLoggedIn.and.returnValue(true);
      
      // Emit a user in the subject to simulate authenticated state
      currentUserSubject.next(mockUser);
      
      // Update the view
      fixture.detectChanges();
    });

    /**
     * Test that user information and logout are displayed for authenticated users
     */
    it('should show user information and logout button', () => {
      // Verify user profile link is displayed with name
      const profileLink = fixture.debugElement.query(By.css('a[routerLink="/user/profile"]'));
      expect(profileLink).withContext('User profile link should be visible').toBeTruthy();
      
      // In the template, user name appears inside profile link
      if (profileLink) {
        expect(profileLink.nativeElement.textContent.trim()).withContext('Profile link should show user name')
          .toContain(mockUser.name);
      }
      
      // Verify logout link is displayed
      const logoutLink = fixture.debugElement.query(By.css('a[role="button"]'));
      expect(logoutLink).withContext('Logout link should be visible').toBeTruthy();
      
      // Verify login/register links are not displayed
      const loginLink = fixture.debugElement.query(By.css('a[routerLink="/auth/login"]'));
      const registerLink = fixture.debugElement.query(By.css('a[routerLink="/auth/register"]'));
      
      expect(loginLink).withContext('Login link should not be visible').toBeFalsy();
      expect(registerLink).withContext('Register link should not be visible').toBeFalsy();
    });

    /**
     * Test that product and cart links are visible for authenticated users
     */
    it('should show Products and Cart links for logged-in users', () => {
      const productsLink = fixture.debugElement.query(By.css('a[routerLink="/products"]'));
      const cartLink = fixture.debugElement.query(By.css('a[routerLink="/cart"]'));
      
      expect(productsLink).withContext('Products link should be visible').toBeTruthy();
      expect(cartLink).withContext('Cart link should be visible').toBeTruthy();
    });

    /**
     * Test that clicking logout calls the logout method
     */
    it('should call logout when logout link is clicked', () => {
      const logoutLink = fixture.debugElement.query(By.css('a[role="button"]'));
      expect(logoutLink).withContext('Logout link should be present').toBeTruthy();
      
      // Click the link
      if (logoutLink) {
        logoutLink.nativeElement.click();
        
        // Verify method was called
        expect(authServiceSpy.logout).withContext('AuthService.logout should be called').toHaveBeenCalled();
      }
    });
  });

  /**
   * Test for the application logo
   */
  it('should have a logo that links to the home page', () => {
    const logoLink = fixture.debugElement.query(By.css('.logo a[routerLink="/"]'));
    expect(logoLink).withContext('Logo should link to home page').toBeTruthy();
    expect(logoLink.nativeElement.textContent).withContext('Logo should display correct text')
      .toContain('Developer Store');
  });
});
