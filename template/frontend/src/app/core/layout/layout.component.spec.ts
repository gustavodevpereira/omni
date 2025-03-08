import { ComponentFixture, TestBed } from '@angular/core/testing';
import { Component } from '@angular/core';
import { By } from '@angular/platform-browser';
import { provideRouter } from '@angular/router';
import { provideHttpClient } from '@angular/common/http';
import { LayoutComponent } from './layout.component';
import { AuthService } from '../services/auth.service';
import { BehaviorSubject } from 'rxjs';
import { HeaderComponent } from '../header/header.component';
import { FooterComponent } from '../footer/footer.component';

/**
 * Mock header component for testing layout component
 */
@Component({
  selector: 'app-header',
  standalone: true,
  template: '<div class="header-mock">Header Mock</div>'
})
class MockHeaderComponent {
  constructor(public authService: AuthService) {}
  
  logout(): void {
    // Mock implementation
  }
}

/**
 * Mock footer component for testing layout component
 */
@Component({
  selector: 'app-footer',
  standalone: true,
  template: '<div class="footer-mock">Footer Mock</div>'
})
class MockFooterComponent {}

/**
 * Test suite for LayoutComponent
 * 
 * @description
 * Verifies the layout component properly:
 * - Renders the correct structure with header, main content, and footer
 * - Includes the router outlet in the main content area
 * - Maintains proper DOM hierarchy for accessibility
 */
describe('LayoutComponent', () => {
  let component: LayoutComponent;
  let fixture: ComponentFixture<LayoutComponent>;
  let mockAuthService: jasmine.SpyObj<AuthService>;

  beforeEach(async () => {
    // Create a mock AuthService with required methods and properties
    mockAuthService = jasmine.createSpyObj('AuthService', ['logout', 'isLoggedIn', 'getToken'], {
      currentUser$: new BehaviorSubject(null)
    });
    
    // Configure the isLoggedIn method to return false by default
    mockAuthService.isLoggedIn.and.returnValue(false);

    await TestBed.configureTestingModule({
      providers: [
        provideRouter([]),
        provideHttpClient(),
        { provide: AuthService, useValue: mockAuthService }
      ]
    }).overrideComponent(
      LayoutComponent, 
      {
        remove: { imports: [HeaderComponent, FooterComponent] },
        add: { imports: [MockHeaderComponent, MockFooterComponent] }
      }
    ).compileComponents();

    fixture = TestBed.createComponent(LayoutComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  /**
   * Test component creation
   */
  it('should create the layout component', () => {
    expect(component).toBeTruthy();
  });

  /**
   * Test that the component renders the header
   */
  it('should render the header component', () => {
    const headerElement = fixture.debugElement.query(By.css('.header-mock'));
    expect(headerElement).toBeTruthy();
  });

  /**
   * Test that the component renders the main content area
   */
  it('should render main content area with router-outlet', () => {
    const mainContentElement = fixture.debugElement.query(By.css('.main-content'));
    expect(mainContentElement).toBeTruthy();
    
    const routerOutletElement = fixture.debugElement.query(By.css('router-outlet'));
    expect(routerOutletElement).toBeTruthy();
    expect(mainContentElement.nativeElement.contains(routerOutletElement.nativeElement))
      .withContext('Router outlet should be inside main content area')
      .toBeTrue();
  });

  /**
   * Test that the component renders the footer
   */
  it('should render the footer component', () => {
    const footerElement = fixture.debugElement.query(By.css('.footer-mock'));
    expect(footerElement).toBeTruthy();
  });

  /**
   * Test the DOM structure for proper semantic HTML
   */
  it('should have correct DOM hierarchy for accessibility', () => {
    const appContainer = fixture.debugElement.query(By.css('.app-container'));
    expect(appContainer).toBeTruthy();
    
    // Check that main content is between header and footer
    const children = appContainer.children;
    expect(children.length).toBe(3);
    expect(children[0].name).toBe('app-header');
    expect(children[1].name).toBe('main');
    expect(children[2].name).toBe('app-footer');
  });
});
