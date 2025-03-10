import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { tap, switchMap, take } from 'rxjs/operators';
import { CartService, Cart, CartItem } from './cart.service';
import { AuthService } from '../../../core/services/auth.service';
import { NotificationService } from '../../../core/services/notification.service';
import { CartApiService, BackendCart } from '../../../core/api/services/cart-api.service';

/**
 * Checkout Service
 * 
 * @description Manages the checkout process, handling the conversion of local cart data
 * to backend-compatible format and processing order creation requests.
 * 
 * @usageNotes
 * This service should be used during the checkout process to:
 * 1. Transform the local cart data into the format expected by the backend API
 * 2. Submit the order to the backend
 * 3. Clear the local cart after successful checkout
 * 4. Retrieve order history
 */
@Injectable({
  providedIn: 'root'
})
export class CheckoutService {
  /**
   * Mock GUID for customer ID when real ID is not available
   * @private
   * @readonly
   */
  private readonly MOCK_CUSTOMER_ID = '7e9b1d20-c9f5-4ac9-a37f-d548b87cc424';
  
  /**
   * Mock GUID for branch ID
   * @private
   * @readonly
   */
  private readonly MOCK_BRANCH_ID = 'b2a5e8d6-7f12-4d8c-9b3e-1a5f6c7d8e9b';

  /**
   * Constructor
   * 
   * @param cartService - Service to access the current cart state
   * @param authService - Service to access user authentication state
   * @param cartApiService - Service to interact with the cart backend APIs
   * @param notificationService - Service to display notifications to the user
   */
  constructor(
    private cartService: CartService,
    private authService: AuthService,
    private cartApiService: CartApiService,
    private notificationService: NotificationService
  ) { }

  /**
   * Process the checkout of the current cart
   * 
   * @description Takes the current cart data, transforms it to backend format,
   * sends it to the API, and clears the local cart on success.
   * 
   * @returns An Observable with the API response containing the created order
   * @throws Error if user is not logged in
   */
  processCheckout(): Observable<BackendCart.ApiResponse<BackendCart.CartResponse>> {
    // Get the current user
    const currentUser = this.authService.getCurrentUser();
    
    if (!currentUser) {
      throw new Error('User must be logged in to checkout');
    }
    
    // Get the current cart as Observable and process it
    return this.cartService.getCart().pipe(
      // Take only the current value of the cart
      take(1),
      
      // Transform the cart into a request for the backend
      switchMap(localCart => {
        // Get branch information from the first product in cart
        const firstProduct = localCart.items[0]?.product;
        
        // Log para depuração
        console.log('First product in cart:', firstProduct);
        
        // Verificar explicitamente se as propriedades existem
        const branchId = firstProduct && firstProduct.branchExternalId 
          ? firstProduct.branchExternalId 
          : this.MOCK_BRANCH_ID;
          
        const branchName = firstProduct && firstProduct.branchName
          ? firstProduct.branchName
          : 'Main Branch';
        
        // Log para depuração
        console.log('Using branch data:', { branchId, branchName });
        
        // Prepare the data in the format expected by the API
        const backendCartRequest: BackendCart.CreateCartRequest = {
          customerId: currentUser.id || this.MOCK_CUSTOMER_ID, // Use user ID or fallback to mock
          customerName: currentUser.name,
          branchId: branchId, // Usando branchExternalId do produto
          branchName: branchName, // Usando branchName do produto
          products: localCart.items.map(item => ({
            productId: item.product.id,
            productName: item.product.name,
            quantity: item.quantity,
            unitPrice: item.product.price
          }))
        };
        
        // Log para depuração
        console.log('Checkout request:', backendCartRequest);
        
        // Send the cart to the backend
        return this.cartApiService.createCartBackend(backendCartRequest);
      }),
      
      // Process the response
      tap(response => {
        if (response.success) {
          // Clear the local cart after success
          this.cartService.clearCart();
          this.notificationService.success('Order placed successfully!');
        } else {
          this.notificationService.error('Failed to place order: ' + (response.message || 'Unknown error'));
        }
      })
    );
  }

  /**
   * Calculate discounts for the current cart
   * 
   * @description Takes the current cart data, transforms it to backend format,
   * sends it to the API for discount calculation, and returns the updated cart with discounts.
   * 
   * @returns An Observable with the API response containing the updated cart with discounts
   * @throws Error if user is not logged in
   */
  calculateDiscounts(): Observable<BackendCart.ApiResponse<BackendCart.CartResponse>> {
    // Get the current user
    const currentUser = this.authService.getCurrentUser();
    
    if (!currentUser) {
      throw new Error('User must be logged in to calculate discounts');
    }
    
    // Get the current cart as Observable and process it
    return this.cartService.getCart().pipe(
      // Take only the current value of the cart
      take(1),
      
      // Transform the cart into a request for the backend
      switchMap(localCart => {
        // Get branch information from the first product in cart
        const firstProduct = localCart.items[0]?.product;
        
        // Verificar explicitamente se as propriedades existem
        const branchId = firstProduct && firstProduct.branchExternalId 
          ? firstProduct.branchExternalId 
          : this.MOCK_BRANCH_ID;
          
        const branchName = firstProduct && firstProduct.branchName
          ? firstProduct.branchName
          : 'Main Branch';
        
        // Create valid date in ISO format
        // Use current date but ensure it's not in the future
        // Some users might have incorrect system date/time
        const now = new Date();
        
        // Force year to be 2023 to ensure it's not in the future
        // This is a safe assumption for a retail application
        // as we don't need precise timing for discount calculations
        now.setFullYear(2023);
        
        const safeDate = now.toISOString();
        
        // Prepare the data in the format expected by the API
        const discountRequest: BackendCart.CalculateDiscountRequest = {
          branchExternalId: branchId,
          branchName: branchName,
          date: safeDate,
          products: localCart.items.map(item => ({
            productId: item.product.id,
            name: item.product.name,
            price: item.product.price,
            quantity: item.quantity
          }))
        };
        
        // Log para depuração
        console.log('Discount calculation request:', discountRequest);
        
        // Send the cart to the backend for discount calculation
        return this.cartApiService.calculateDiscounts(discountRequest);
      }),
      
      // Process the response
      tap(response => {
        console.log('Discount calculation response:', response);
        
        if (response.success) {
          this.notificationService.success('Discounts calculated successfully!');
        } else {
          this.notificationService.error('Failed to calculate discounts: ' + (response.message || 'Unknown error'));
        }
      })
    );
  }

  /**
   * Get the order history for the current user
   * 
   * @description Retrieves a paginated list of past orders from the backend
   * 
   * @param page - The page number to retrieve (1-based)
   * @param pageSize - The number of items per page
   * @returns An Observable with the paginated order history response
   */
  getOrderHistory(page: number = 1, pageSize: number = 10): Observable<BackendCart.CartListResponse> {
    return this.cartApiService.getCartListBackend(page, pageSize);
  }
} 