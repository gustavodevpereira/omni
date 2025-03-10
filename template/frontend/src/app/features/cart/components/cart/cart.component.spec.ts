import { ComponentFixture, TestBed } from '@angular/core/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { Router } from '@angular/router';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';
import { By } from '@angular/platform-browser';
import { of } from 'rxjs';

import { SharedModule } from '../../../../shared/shared.module';
import { CartComponent } from './cart.component';
import { CartService, Cart, CartItem } from '../../services/cart.service';
import { AuthService } from '../../../../core/services/auth.service';
import { NotificationService } from '../../../../core/services/notification.service';
import { Product } from '../../../../core/api/models/domain.model';

describe('CartComponent', () => {
  let component: CartComponent;
  let fixture: ComponentFixture<CartComponent>;
  let cartServiceMock: jasmine.SpyObj<CartService>;
  let authServiceMock: jasmine.SpyObj<AuthService>;
  let notificationServiceMock: jasmine.SpyObj<NotificationService>;
  let routerMock: jasmine.SpyObj<Router>;

  // Mock product data
  const mockProduct1: Product = {
    id: 'product-1',
    name: 'Product 1',
    price: 99.99,
    description: 'Test product 1',
    category: 'Electronics',
    sku: 'SKU001',
    stockQuantity: 10,
    status: 'Active',
    branchExternalId: 'branch-1',
    branchName: 'Main Branch',
    createdAt: '2023-01-01',
    updatedAt: null
  };

  const mockProduct2: Product = {
    id: 'product-2',
    name: 'Product 2',
    price: 49.99,
    description: 'Test product 2',
    category: 'Clothing',
    sku: 'SKU002',
    stockQuantity: 20,
    status: 'Active',
    branchExternalId: 'branch-1',
    branchName: 'Main Branch',
    createdAt: '2023-01-01',
    updatedAt: null
  };

  // Mock cart data
  const mockEmptyCart: Cart = {
    items: [],
    totalItems: 0,
    totalPrice: 0
  };

  const mockCartWithItems: Cart = {
    items: [
      { product: mockProduct1, quantity: 2 },
      { product: mockProduct2, quantity: 1 }
    ],
    totalItems: 3,
    totalPrice: 249.97
  };

  beforeEach(async () => {
    // Create spy objects for all dependencies
    cartServiceMock = jasmine.createSpyObj('CartService', [
      'getCart', 
      'updateQuantity', 
      'removeFromCart', 
      'clearCart'
    ]);
    authServiceMock = jasmine.createSpyObj('AuthService', ['isAuthenticated']);
    notificationServiceMock = jasmine.createSpyObj('NotificationService', [
      'info', 
      'success', 
      'warning', 
      'error'
    ]);
    routerMock = jasmine.createSpyObj('Router', ['navigate']);

    // Configure default returns
    cartServiceMock.getCart.and.returnValue(of(mockCartWithItems));
    authServiceMock.isAuthenticated.and.returnValue(true);

    await TestBed.configureTestingModule({
      imports: [
        RouterTestingModule,
        NoopAnimationsModule,
        SharedModule,
        CartComponent
      ],
      providers: [
        { provide: CartService, useValue: cartServiceMock },
        { provide: AuthService, useValue: authServiceMock },
        { provide: NotificationService, useValue: notificationServiceMock },
        { provide: Router, useValue: routerMock }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(CartComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should load cart data on init', () => {
    expect(cartServiceMock.getCart).toHaveBeenCalled();
    expect(component.cart).toEqual(mockCartWithItems);
    expect(component.cartItems.length).toBe(2);
    expect(component.totalItems).toBe(3);
  });

  it('should display cart items when cart is not empty', () => {
    const cartItems = fixture.debugElement.queryAll(By.css('.cart-item'));
    expect(cartItems.length).toBe(2);
    
    // Verify first item content
    const firstItem = cartItems[0];
    expect(firstItem.query(By.css('.cart-item-title')).nativeElement.textContent).toContain('Product 1');
    expect(firstItem.query(By.css('.price-col')).nativeElement.textContent).toContain('99.99');
    expect(firstItem.query(By.css('.quantity-display')).nativeElement.textContent).toContain('2');
    
    // Verify total price calculation
    expect(firstItem.query(By.css('.total-col')).nativeElement.textContent).toContain('199.98');
  });

  it('should show empty cart message when cart is empty', () => {
    // Update cart to be empty
    cartServiceMock.getCart.and.returnValue(of(mockEmptyCart));
    component.ngOnInit();
    fixture.detectChanges();
    
    // Verify empty cart message is displayed
    const emptyCartMessage = fixture.debugElement.query(By.css('.empty-cart-container'));
    expect(emptyCartMessage).toBeTruthy();
    expect(emptyCartMessage.query(By.css('h2')).nativeElement.textContent).toContain('Your cart is empty');
    
    // Verify continue shopping button is present
    const continueButton = emptyCartMessage.query(By.css('button'));
    expect(continueButton).toBeTruthy();
    expect(continueButton.nativeElement.textContent).toContain('Continue Shopping');
  });

  it('should update quantity when + button is clicked', () => {
    // Get the first item's increase quantity button
    const increaseButton = fixture.debugElement.queryAll(By.css('.quantity-btn'))[1];
    
    // Click the button
    increaseButton.triggerEventHandler('click', null);
    
    // Verify that updateQuantity was called with correct parameters
    expect(cartServiceMock.updateQuantity).toHaveBeenCalledWith(mockProduct1.id, 3);
    expect(notificationServiceMock.info).toHaveBeenCalledWith('Updated quantity to 3');
  });

  it('should update quantity when - button is clicked', () => {
    // Get the first item's decrease quantity button
    const decreaseButton = fixture.debugElement.queryAll(By.css('.quantity-btn'))[0];
    
    // Click the button
    decreaseButton.triggerEventHandler('click', null);
    
    // Verify that updateQuantity was called with correct parameters
    expect(cartServiceMock.updateQuantity).toHaveBeenCalledWith(mockProduct1.id, 1);
    expect(notificationServiceMock.info).toHaveBeenCalledWith('Updated quantity to 1');
  });

  it('should not update quantity below 1', () => {
    // Create a cart item with quantity 1
    const item: CartItem = { product: mockProduct1, quantity: 1 };
    
    // Try to update to 0
    component.updateQuantity(item, 0);
    
    // Verify that updateQuantity was not called
    expect(cartServiceMock.updateQuantity).not.toHaveBeenCalled();
  });

  it('should limit quantity to 99 and show notification', () => {
    // Create a cart item
    const item: CartItem = { product: mockProduct1, quantity: 90 };
    
    // Try to update to 100
    component.updateQuantity(item, 100);
    
    // Verify that updateQuantity was called with 99
    expect(cartServiceMock.updateQuantity).toHaveBeenCalledWith(mockProduct1.id, 99);
    expect(notificationServiceMock.info).toHaveBeenCalledWith('Maximum quantity reached');
  });

  it('should remove item when delete button is clicked', () => {
    // Get the first item's delete button
    const deleteButton = fixture.debugElement.query(By.css('button[color="warn"]'));
    
    // Click the button
    deleteButton.triggerEventHandler('click', null);
    
    // Verify that removeFromCart was called with correct product ID
    expect(cartServiceMock.removeFromCart).toHaveBeenCalledWith(mockProduct1.id);
  });

  it('should clear cart when clear cart button is clicked', () => {
    // Get the clear cart button
    const clearCartButton = fixture.debugElement.queryAll(By.css('button[mat-stroked-button]'))[1];
    
    // Click the button
    clearCartButton.triggerEventHandler('click', null);
    
    // Verify that clearCart was called
    expect(cartServiceMock.clearCart).toHaveBeenCalled();
  });

  it('should continue shopping when continue shopping button is clicked', () => {
    // Get the continue shopping button
    const continueShoppingButton = fixture.debugElement.queryAll(By.css('button[mat-stroked-button]'))[0];
    
    // Click the button
    continueShoppingButton.triggerEventHandler('click', null);
    
    // Verify that router.navigate was called with correct path
    expect(routerMock.navigate).toHaveBeenCalledWith(['/products']);
  });

  it('should proceed to checkout when checkout button is clicked and user is authenticated', () => {
    // Get the checkout button
    const checkoutButton = fixture.debugElement.query(By.css('.checkout-btn'));
    
    // Click the button
    checkoutButton.triggerEventHandler('click', null);
    
    // Verify that router.navigate was called with correct path
    expect(routerMock.navigate).toHaveBeenCalledWith(['/cart/checkout']);
  });

  it('should redirect to login when checkout button is clicked and user is not authenticated', () => {
    // Set auth service to return false
    authServiceMock.isAuthenticated.and.returnValue(false);
    
    // Get the checkout button
    const checkoutButton = fixture.debugElement.query(By.css('.checkout-btn'));
    
    // Click the button
    checkoutButton.triggerEventHandler('click', null);
    
    // Verify that router.navigate was called with correct path and query params
    expect(routerMock.navigate).toHaveBeenCalledWith(
      ['/auth/login'], 
      { queryParams: { returnUrl: '/cart/checkout' } }
    );
    expect(notificationServiceMock.info).toHaveBeenCalledWith('Please login to checkout');
  });

  it('should show warning when trying to checkout with empty cart', () => {
    // Update cart to be empty
    cartServiceMock.getCart.and.returnValue(of(mockEmptyCart));
    component.ngOnInit();
    fixture.detectChanges();
    
    // Call checkout method directly (button would be disabled in UI)
    component.checkout();
    
    // Verify warning was shown
    expect(notificationServiceMock.warning).toHaveBeenCalledWith('Your cart is empty');
    expect(routerMock.navigate).not.toHaveBeenCalled();
  });

  it('should calculate order summary correctly', () => {
    const summaryRows = fixture.debugElement.queryAll(By.css('.summary-row'));
    
    // Subtotal
    expect(summaryRows[0].nativeElement.textContent).toContain('Subtotal (3 items)');
    expect(summaryRows[0].nativeElement.textContent).toContain('$249.97');
    
    // Tax
    expect(summaryRows[2].nativeElement.textContent).toContain('Tax (10%)');
    expect(summaryRows[2].nativeElement.textContent).toContain('$25.00');
    
    // Total
    expect(summaryRows[3].nativeElement.textContent).toContain('Total');
    expect(summaryRows[3].nativeElement.textContent).toContain('$274.97');
  });

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