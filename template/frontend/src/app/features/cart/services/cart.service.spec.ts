import { TestBed } from '@angular/core/testing';
import { CartService, CartItem, Cart } from './cart.service';
import { NotificationService } from '../../../core/services/notification.service';
import { Product } from '../../../core/api/models/domain.model';

/**
 * CartService Unit Tests
 * 
 * @description Tests for the CartService which manages the shopping cart functionality
 * including adding, removing, and updating items in the cart, as well as persistence
 * to localStorage.
 */
describe('CartService', () => {
  let service: CartService;
  let notificationServiceMock: jasmine.SpyObj<NotificationService>;
  // Sample product for testing
  const mockProduct: Product = {
    id: '123',
    name: 'Test Product',
    price: 99.99,
    description: 'Test description',
    category: 'Test Category',
    sku: 'TEST123',
    stockQuantity: 100,
    status: 'Active',
    branchExternalId: '456',
    branchName: 'Test Branch',
    createdAt: new Date().toISOString(),
    updatedAt: null
  };
  
  // Mock for localStorage
  let localStorageMock: any = {};

  /**
   * Setup before each test
   */
  beforeEach(() => {
    // Create spy for notification service
    notificationServiceMock = jasmine.createSpyObj('NotificationService', 
      ['success', 'error', 'info', 'warning']);
    
    // Setup localStorage mock
    localStorageMock = {};
    spyOn(localStorage, 'getItem').and.callFake((key) => {
      return localStorageMock[key] || null;
    });
    spyOn(localStorage, 'setItem').and.callFake((key, value) => {
      localStorageMock[key] = value.toString();
    });
    spyOn(localStorage, 'removeItem').and.callFake((key) => {
      delete localStorageMock[key];
    });
    
    // Configure TestBed
    TestBed.configureTestingModule({
      providers: [
        CartService,
        { provide: NotificationService, useValue: notificationServiceMock }
      ]
    });
    
    // Get service instance
    service = TestBed.inject(CartService);
  });

  /**
   * Test service creation
   */
  it('should be created', () => {
    expect(service).toBeTruthy();
  });
  
  /**
   * Test adding a product to cart
   */
  it('should add a product to cart', () => {
    // Execute
    service.addToCart(mockProduct, 1);
    
    // Verify
    service.getCart().subscribe(cart => {
      expect(cart.items.length).toBe(1);
      expect(cart.items[0].product.id).toBe(mockProduct.id);
      expect(cart.items[0].quantity).toBe(1);
      expect(cart.totalItems).toBe(1);
      expect(cart.totalPrice).toBe(mockProduct.price);
    });
    
    // Verify notification was shown
    expect(notificationServiceMock.success).toHaveBeenCalledWith(
      `Added ${mockProduct.name} to cart`
    );
    
    // Verify localStorage was updated
    expect(localStorage.setItem).toHaveBeenCalled();
  });
  
  /**
   * Test adding same product multiple times
   */
  it('should increase quantity when adding same product multiple times', () => {
    // Setup
    service.addToCart(mockProduct, 1);
    
    // Execute
    service.addToCart(mockProduct, 2);
    
    // Verify
    service.getCart().subscribe(cart => {
      expect(cart.items.length).toBe(1);
      expect(cart.items[0].quantity).toBe(3);
      expect(cart.totalItems).toBe(3);
      expect(cart.totalPrice).toBe(mockProduct.price * 3);
    });
  });
  
  /**
   * Test updating item quantity
   */
  it('should update item quantity correctly', () => {
    // Setup
    service.addToCart(mockProduct, 1);
    
    // Execute
    service.updateQuantity(mockProduct.id, 5);
    
    // Verify
    service.getCart().subscribe(cart => {
      expect(cart.items[0].quantity).toBe(5);
      expect(cart.totalItems).toBe(5);
      expect(cart.totalPrice).toBe(mockProduct.price * 5);
    });
  });
  
  /**
   * Test removing an item from cart
   */
  it('should remove an item from cart', () => {
    // Setup
    service.addToCart(mockProduct, 2);
    
    // Execute
    service.removeFromCart(mockProduct.id);
    
    // Verify
    service.getCart().subscribe(cart => {
      expect(cart.items.length).toBe(0);
      expect(cart.totalItems).toBe(0);
      expect(cart.totalPrice).toBe(0);
    });
    
    // Verify notification was shown
    expect(notificationServiceMock.info).toHaveBeenCalledWith('Item removed from cart');
  });
  
  /**
   * Test clearing the cart
   */
  it('should clear the cart completely', () => {
    // Setup
    service.addToCart(mockProduct, 2);
    
    // Execute
    service.clearCart();
    
    // Verify
    service.getCart().subscribe(cart => {
      expect(cart.items.length).toBe(0);
      expect(cart.totalItems).toBe(0);
      expect(cart.totalPrice).toBe(0);
    });
    
    // Verify notification was shown
    expect(notificationServiceMock.info).toHaveBeenCalledWith('Cart cleared');
  });
  
  /**
   * Test setting quantity to zero should remove item
   */
  it('should remove item when updating quantity to zero', () => {
    // Setup
    service.addToCart(mockProduct, 2);
    
    // Execute
    service.updateQuantity(mockProduct.id, 0);
    
    // Verify
    service.getCart().subscribe(cart => {
      expect(cart.items.length).toBe(0);
    });
    
    // Verify removeFromCart was called internally
    expect(notificationServiceMock.info).toHaveBeenCalledWith('Item removed from cart');
  });
  
  /**
   * Test cart persists to localStorage
   */
  it('should persist cart to localStorage', () => {
    // Setup & Execute
    service.addToCart(mockProduct, 3);
    
    // Verify
    expect(localStorage.setItem).toHaveBeenCalled();
    
    // Get the key and verify JSON structure
    const key = 'shopping_cart';
    const storedValue = localStorageMock[key];
    
    expect(storedValue).toBeDefined();
    if (storedValue) {
      const parsedCart = JSON.parse(storedValue);
      expect(parsedCart.items.length).toBe(1);
      expect(parsedCart.totalItems).toBe(3);
      expect(parsedCart.totalPrice).toBe(mockProduct.price * 3);
    }
  });
  
  /**
   * Test loading cart from localStorage
   */
  it('should load cart from localStorage on initialization', () => {
    // Setup - prepare a cart in localStorage
    const storedCart: Cart = {
      items: [{ product: mockProduct, quantity: 4 }],
      totalItems: 4,
      totalPrice: mockProduct.price * 4
    };
    
    // Reset spies to ensure we're starting fresh
    (localStorage.getItem as jasmine.Spy).calls.reset();
    (localStorage.setItem as jasmine.Spy).calls.reset();
    
    // Set value directly in our mock
    localStorageMock['shopping_cart'] = JSON.stringify(storedCart);
    
    // Re-configure the spy to return our mock data
    (localStorage.getItem as jasmine.Spy).and.callFake((key) => {
      return localStorageMock[key] || null;
    });
    
    // Execute - create a new service which will initialize from our mocked localStorage
    TestBed.resetTestingModule();
    TestBed.configureTestingModule({
      providers: [
        CartService,
        { provide: NotificationService, useValue: notificationServiceMock }
      ]
    });
    
    const newService = TestBed.inject(CartService);
    
    // Get a value immediately since we need to test the initial value
    const cartValue = (newService as any).cartSubject.value;
    
    // Verify the initial state
    expect(cartValue.items.length).toBe(1);
    expect(cartValue.totalItems).toBe(4);
    expect(cartValue.totalPrice).toBe(mockProduct.price * 4);
  });
  
  /**
   * Test error handling when localStorage has invalid data
   */
  it('should handle corrupted localStorage data', () => {
    // Setup - prepare invalid cart data in localStorage
    // Reset spies to ensure we're starting fresh
    (localStorage.getItem as jasmine.Spy).calls.reset();
    
    // Create invalid JSON directly in our mock
    localStorageMock['shopping_cart'] = 'invalid json data';
    
    // Re-configure the spy to return our invalid data
    (localStorage.getItem as jasmine.Spy).and.callFake((key) => {
      return localStorageMock[key] || null;
    });
    
    // Spy on console.error - need to do this before initializing service
    const consoleErrorSpy = spyOn(console, 'error');
    
    // Execute - create a new service which will attempt to parse invalid data
    TestBed.resetTestingModule();
    TestBed.configureTestingModule({
      providers: [
        CartService,
        { provide: NotificationService, useValue: notificationServiceMock }
      ]
    });
    
    const newService = TestBed.inject(CartService);
    
    // Verify
    const cartValue = (newService as any).cartSubject.value;
    expect(cartValue.items.length).toBe(0); // Should have empty cart
    
    // Verify error was logged
    expect(consoleErrorSpy).toHaveBeenCalled();
  });
}); 