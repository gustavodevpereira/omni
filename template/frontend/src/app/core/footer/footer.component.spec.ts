/**
 * @fileoverview Test suite for the FooterComponent
 * 
 * This test suite verifies the rendering and behavior of the application's footer component.
 * Tests ensure that all required sections are properly displayed, links are correctly
 * configured, and the component meets design and functional requirements.
 * 
 * @see FooterComponent
 */
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { FooterComponent } from './footer.component';
import { By } from '@angular/platform-browser';
import { DebugElement, NO_ERRORS_SCHEMA } from '@angular/core';
import { RouterTestingModule } from '@angular/router/testing';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';

/**
 * Test suite for the FooterComponent.
 * 
 * Verifies that the component renders correctly and contains all expected sections,
 * including navigation links, copyright information, and social media links.
 */
describe('FooterComponent', () => {
  let component: FooterComponent;
  let fixture: ComponentFixture<FooterComponent>;
  let el: DebugElement;

  /**
   * Test suite setup
   * 
   * Configures the testing module with necessary imports and creates the component fixture.
   * This runs before each test case to ensure a clean testing environment.
   */
  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [
        RouterTestingModule,
        MatIconModule,
        MatButtonModule,
        FooterComponent
      ],
      schemas: [NO_ERRORS_SCHEMA] // To ignore unrecognized elements
    }).compileComponents();

    fixture = TestBed.createComponent(FooterComponent);
    component = fixture.componentInstance;
    el = fixture.debugElement;
    fixture.detectChanges();
  });

  /**
   * Test case: Component creation
   * 
   * Verifies that the component can be created successfully without errors.
   * This is a basic sanity check for the component's initialization.
   */
  it('should create', () => {
    expect(component).toBeTruthy();
  });

  /**
   * Test case: Copyright information
   * 
   * Verifies that the copyright section is properly rendered in the footer.
   * Checks for the presence of the copyright element and validates that it
   * contains the current year and company name.
   */
  it('should display copyright information', () => {
    // Check if copyright element exists
    const copyrightElement = el.query(By.css('.copyright'));
    expect(copyrightElement).toBeTruthy('Copyright element should exist');
    
    // Check if copyright text contains current year
    const currentYear = new Date().getFullYear().toString();
    const copyrightText = copyrightElement.nativeElement.textContent;
    expect(copyrightText).toContain(currentYear, 'Copyright should contain current year');
    expect(copyrightText).toContain('Developer Store', 'Copyright should contain company name');
  });

  /**
   * Test case: Footer sections
   * 
   * Verifies that all required sections of the footer are present in the DOM.
   * This ensures the overall structure of the footer meets design requirements.
   * Sections include: brand, navigation, contact, newsletter, and legal.
   */
  it('should display all required sections', () => {
    // Check if all main sections exist
    const brandSection = el.query(By.css('.footer-brand'));
    expect(brandSection).toBeTruthy('Brand section should exist');
    
    const navSection = el.query(By.css('.footer-nav'));
    expect(navSection).toBeTruthy('Navigation section should exist');
    
    const contactSection = el.query(By.css('.footer-contact'));
    expect(contactSection).toBeTruthy('Contact section should exist');
    
    const newsletterSection = el.query(By.css('.footer-newsletter'));
    expect(newsletterSection).toBeTruthy('Newsletter section should exist');
    
    const legalSection = el.query(By.css('.footer-legal'));
    expect(legalSection).toBeTruthy('Legal links section should exist');
  });

  /**
   * Test case: Navigation links
   * 
   * Verifies that the footer contains the expected number of navigation links
   * and checks that all required destination paths are included.
   * This ensures users can navigate to key areas of the application.
   */
  it('should have correct navigation links', () => {
    // Check number of navigation links
    const navLinks = el.queryAll(By.css('.link-list a'));
    expect(navLinks.length).toBe(4, 'Should have 4 navigation links');
    
    // Get component data to verify URLs directly
    const urls = component.quickLinks.map(link => link.url);
    
    // Check specific URLs
    expect(urls).toContain('/products', 'Should link to products page');
    expect(urls).toContain('/cart', 'Should link to cart page');
    expect(urls).toContain('/about', 'Should link to about page');
    expect(urls).toContain('/contact', 'Should link to contact page');
  });

  /**
   * Test case: Social media links
   * 
   * Verifies that the footer contains the expected number of social media links
   * and checks that all required platform URLs are included.
   * This ensures the social media section is properly rendered and configured.
   */
  it('should have social media links', () => {
    // Check number of social media links
    const socialLinks = el.queryAll(By.css('.social-links a'));
    expect(socialLinks.length).toBe(3, 'Should have 3 social media links');
    
    // Get component data to verify URLs directly
    const urls = component.socialLinks.map(link => link.url);
    
    // Check specific URLs
    expect(urls).toContain('https://facebook.com', 'Should have Facebook link');
    expect(urls).toContain('https://instagram.com', 'Should have Instagram link');
    expect(urls).toContain('https://linkedin.com', 'Should have LinkedIn link');
  });

  /**
   * Test case: Newsletter form
   * 
   * Verifies that the newsletter signup form is present and contains
   * the required input field and submit button.
   * This ensures users can subscribe to newsletters through the footer.
   */
  it('should have newsletter signup form', () => {
    const newsletterForm = el.query(By.css('.newsletter-form'));
    expect(newsletterForm).toBeTruthy('Newsletter form should exist');
    
    const inputElement = newsletterForm.query(By.css('input[type="email"]'));
    expect(inputElement).toBeTruthy('Email input should exist in newsletter form');
    
    const buttonElement = newsletterForm.query(By.css('button'));
    expect(buttonElement).toBeTruthy('Subscribe button should exist in newsletter form');
  });

  /**
   * Test case: Legal links
   * 
   * Verifies that the footer contains the expected legal links (terms, privacy, cookies).
   * This ensures compliance with legal requirements for website footers.
   */
  it('should have legal links in footer bottom', () => {
    const legalLinks = el.queryAll(By.css('.footer-legal a'));
    expect(legalLinks.length).toBe(3, 'Should have 3 legal links');
    
    const legalHrefs = legalLinks.map(link => link.nativeElement.getAttribute('href'));
    expect(legalHrefs).toContain('/terms', 'Should have Terms link');
    expect(legalHrefs).toContain('/privacy', 'Should have Privacy link');
    expect(legalHrefs).toContain('/cookies', 'Should have Cookies link');
  });

  /**
   * Test case: Contact information
   * 
   * Verifies that the footer displays the required contact information.
   * Tests that address, email and phone number are all present.
   * This ensures users can find ways to contact the company.
   */
  it('should have contact information', () => {
    const contactItems = el.queryAll(By.css('.contact-item'));
    expect(contactItems.length).toBe(3, 'Should have 3 contact items');
    
    const contactText = contactItems.map(item => item.nativeElement.textContent);
    const hasAddress = contactText.some(text => text.includes('Developer Way'));
    const hasEmail = contactText.some(text => text.includes('hello@devstore.com'));
    const hasPhone = contactText.some(text => text.includes('555'));
    
    expect(hasAddress).toBeTrue();
    expect(hasEmail).toBeTrue();
    expect(hasPhone).toBeTrue();
  });
});
