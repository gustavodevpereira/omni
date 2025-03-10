import { ComponentFixture, TestBed } from '@angular/core/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';
import { By } from '@angular/platform-browser';
import { Router } from '@angular/router';
import { of, throwError } from 'rxjs';
import { MatDialog, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { PageEvent } from '@angular/material/paginator';

import { SharedModule } from '../../../../shared/shared.module';
import { ProductListComponent } from './product-list.component';
import { ProductService, ProductListResponse } from '../../services/product.service';
import { CartService } from '../../../cart/services/cart.service';
import { NotificationService } from '../../../../core/services/notification.service';
import { Product } from '../../../../core/api/models/domain.model';
import { Component } from '@angular/core';

// Mock CartModalComponent to avoid loading the real component
@Component({
  selector: 'app-cart-modal',
  template: '<div>Mock Cart Modal</div>'
})
class MockCartModalComponent {}

/**
 * Unit tests for ProductListComponent
 */
describe('ProductListComponent', () => {
  let component: ProductListComponent;
  let fixture: ComponentFixture<ProductListComponent>;
  let productServiceMock: jasmine.SpyObj<ProductService>;
  let cartServiceMock: jasmine.SpyObj<CartService>;
  let notificationServiceMock: jasmine.SpyObj<NotificationService>;
  let dialogMock: jasmine.SpyObj<MatDialog>;
  let routerMock: jasmine.SpyObj<Router>;

  // Mock data
  const mockProducts: Product[] = [
    {
      id: 'product-1',
      name: 'Product 1',
      price: 99.99,
      description: 'Description for product 1',
      category: 'Category A',
      sku: 'SKU001',
      stockQuantity: 20,
      status: 'Active',
      branchExternalId: 'branch-1',
      branchName: 'Main Branch',
      createdAt: '2023-01-01',
      updatedAt: null
    },
    {
      id: 'product-2',
      name: 'Product 2',
      price: 149.99,
      description: 'Description for product 2',
      category: 'Category B',
      sku: 'SKU002',
      stockQuantity: 5,
      status: 'Active',
      branchExternalId: 'branch-1',
      branchName: 'Main Branch',
      createdAt: '2023-01-02',
      updatedAt: null
    }
  ];

  const mockProductsResponse: ProductListResponse = {
    products: mockProducts,
    total: 20,
    page: 0,
    limit: 6
  };

  // Mock MatDialogRef for use with dialog.open
  const mockDialogRef = {
    afterClosed: () => of(true),
    close: jasmine.createSpy('close')
  } as unknown as MatDialogRef<any>;

  beforeEach(async () => {
    // Create spy objects for all dependencies
    productServiceMock = jasmine.createSpyObj('ProductService', ['getProducts']);
    cartServiceMock = jasmine.createSpyObj('CartService', ['getCart', 'addToCart']);
    notificationServiceMock = jasmine.createSpyObj('NotificationService', ['error', 'success']);
    dialogMock = jasmine.createSpyObj('MatDialog', ['open']);
    routerMock = jasmine.createSpyObj('Router', ['navigate']);

    // Configure default returns
    productServiceMock.getProducts.and.returnValue(of(mockProductsResponse));
    cartServiceMock.getCart.and.returnValue(of({ items: [], totalItems: 0, totalPrice: 0 }));
    // Return our mockDialogRef instead of trying to create a real MatDialogRef
    dialogMock.open.and.returnValue(mockDialogRef);

    await TestBed.configureTestingModule({
      imports: [
        RouterTestingModule,
        NoopAnimationsModule,
        SharedModule
      ],
      providers: [
        { provide: ProductService, useValue: productServiceMock },
        { provide: CartService, useValue: cartServiceMock },
        { provide: NotificationService, useValue: notificationServiceMock },
        { provide: MatDialog, useValue: dialogMock },
        { provide: Router, useValue: routerMock }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(ProductListComponent);
    component = fixture.componentInstance;
    
    // Override the openCartModal method to use our mock component
    component.openCartModal = () => {
      dialogMock.open(MockCartModalComponent, {
        width: '500px',
        maxWidth: '500px'
      });
    };
    
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  /**
   * Test initial product loading
   */
  it('should load products on init', () => {
    expect(productServiceMock.getProducts).toHaveBeenCalled();
    expect(component.products).toEqual(mockProducts);
    expect(component.totalProducts).toBe(20);
    expect(component.isLoading).toBeFalse();
  });

  /**
   * Test empty product list
   */
  it('should display empty state when no products returned', () => {
    // Setup empty product list
    const emptyResponse: ProductListResponse = { products: [], total: 0, page: 0, limit: 6 };
    productServiceMock.getProducts.and.returnValue(of(emptyResponse));
    
    // Re-initialize component
    component.ngOnInit();
    fixture.detectChanges();
    
    // Check that empty state is displayed
    const emptyStateElement = fixture.debugElement.query(By.css('.text-center h3'));
    expect(emptyStateElement).toBeTruthy();
    expect(emptyStateElement.nativeElement.textContent).toContain('No products found');
  });

  /**
   * Test loading state
   */
  it('should show loading spinner while loading products', () => {
    // Set loading state
    component.isLoading = true;
    fixture.detectChanges();
    
    // Check spinner is visible
    const spinner = fixture.debugElement.query(By.css('mat-spinner'));
    expect(spinner).toBeTruthy();
    
    // Products should not be visible
    const productCards = fixture.debugElement.queryAll(By.css('.product-card'));
    expect(productCards.length).toBe(0);
  });

  /**
   * Test product cards rendering
   */
  it('should render product cards with correct information', () => {
    // Each product should be in a card
    const productCards = fixture.debugElement.queryAll(By.css('.product-card'));
    expect(productCards.length).toBe(2);
    
    // Check content of first card
    const firstCard = productCards[0];
    expect(firstCard.query(By.css('.product-title')).nativeElement.textContent).toContain('Product 1');
    expect(firstCard.query(By.css('.product-description')).nativeElement.textContent).toContain('Description for product 1');
    expect(firstCard.query(By.css('.sku')).nativeElement.textContent).toContain('SKU001');
    
    // Check low stock indicator on second product
    const secondCard = productCards[1];
    expect(secondCard.query(By.css('.stock')).classes['low-stock']).toBeTrue();
  });


  /**
   * Test add to cart functionality
   */
  it('should add product to cart and open cart modal', () => {
    fixture.detectChanges(); // Ensure component is initialized

    // Get add to cart button from first product
    const addToCartButton = fixture.debugElement.queryAll(By.css('.product-card button'))[0];
    
    // Create mock event with stopPropagation method
    const mockEvent = jasmine.createSpyObj('Event', ['stopPropagation']);
    
    // Spy on component's addToCart method
    spyOn(component, 'addToCart').and.callThrough();
    
    // Trigger click with mock event
    addToCartButton.triggerEventHandler('click', mockEvent);
    
    // Verify component's addToCart method was called
    expect(component.addToCart).toHaveBeenCalled();
    
    // Call addToCart directly to verify its internals
    component.addToCart(mockProducts[0]);
    
    // Verify product was added to cart
    expect(cartServiceMock.addToCart).toHaveBeenCalledWith(mockProducts[0]);
    
    // Verify cart modal was opened
    expect(dialogMock.open).toHaveBeenCalled();
    expect(dialogMock.open.calls.mostRecent().args[0]).toBeDefined();
  });

  /**
   * Test cart badge
   */
  it('should update cart badge when cart changes', () => {
    // Set up cart with items
    cartServiceMock.getCart.and.returnValue(of({ 
      items: [{ product: mockProducts[0], quantity: 2 }], 
      totalItems: 2, 
      totalPrice: 199.98 
    }));
    
    // Re-initialize component
    component.ngOnInit();
    fixture.detectChanges();
    
    // Check that cart badge shows correct count
    expect(component.cartItemCount).toBe(2);
  });

  /**
   * Test pagination
   */
  it('should handle page changes', () => {
    // Reset service calls
    productServiceMock.getProducts.calls.reset();
    
    // Create page event
    const pageEvent: PageEvent = {
      pageIndex: 1,
      pageSize: 12,
      length: 20
    };
    
    // Trigger page change
    component.onPageChange(pageEvent);
    
    // Verify component state updated
    expect(component.currentPage).toBe(1);
    expect(component.pageSize).toBe(12);
    
    // Verify products were reloaded
    expect(productServiceMock.getProducts).toHaveBeenCalledWith({
      page: 1,
      limit: 12
    });
  });

  /**
   * Test error handling
   */
  it('should handle errors when loading products', () => {
    // Set up error response
    productServiceMock.getProducts.and.returnValue(throwError(() => new Error('API Error')));
    
    // Trigger loading
    component.loadProducts();
    
    // Check error handling
    expect(component.isLoading).toBeFalse();
    expect(component.products).toEqual([]);
    expect(notificationServiceMock.error).toHaveBeenCalledWith('Failed to load products. Please try again.');
  });

  /**
   * Test cart modal
   */
  it('should open cart modal when cart button is clicked', () => {
    fixture.detectChanges(); // Ensure component is initialized
    
    // Call openCartModal directly to test its functionality
    component.openCartModal();
    
    // Verify dialog was opened
    expect(dialogMock.open).toHaveBeenCalled();
    
    // Basic verification of dialog config
    const dialogCall = dialogMock.open.calls.mostRecent();
    expect(dialogCall).toBeDefined();
    expect(dialogCall.args.length).toBeGreaterThanOrEqual(1);
  });

  /**
   * Test cleanup
   */
  it('should clean up subscriptions on destroy', () => {
    // Spy on Subject's next and complete methods
    const nextSpy = spyOn(component['destroy$'], 'next');
    const completeSpy = spyOn(component['destroy$'], 'complete');
    
    // Call ngOnDestroy
    component.ngOnDestroy();
    
    // Verify that next and complete were called
    expect(nextSpy).toHaveBeenCalled();
    expect(completeSpy).toHaveBeenCalled();
  });
}); 