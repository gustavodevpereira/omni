import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Router } from '@angular/router';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { CartService, Cart } from '../../services/cart.service';
import { CheckoutService } from '../../services/checkout.service';
import { SharedModule } from '../../../../shared/shared.module';
import { NotificationService } from '../../../../core/services/notification.service';
import { BackendCart } from '../../../../core/api/services/cart-api.service';

/**
 * Checkout Component
 * 
 * @description Displays the checkout page with order summary and provides
 * functionality to place orders. Shows cart items, pricing details, and
 * handles the order placement process.
 * 
 * @usageNotes
 * This component is used in the checkout flow after the user has reviewed
 * their cart and is ready to place an order.
 */
@Component({
  selector: 'app-checkout',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    SharedModule
  ],
  templateUrl: './checkout.component.html',
  styleUrls: ['./checkout.component.scss']
})
export class CheckoutComponent implements OnInit, OnDestroy {
  /** Current cart data */
  cart: Cart = { items: [], totalItems: 0, totalPrice: 0 };
  
  /** Flag to indicate if an order is being processed */
  isProcessing = false;
  
  /** Flag to indicate if discounts are being calculated */
  isCalculatingDiscounts = false;
  
  /** Discounted cart response from the API */
  discountedCartResponse: BackendCart.ApiResponse<BackendCart.CartResponse> | null = null;
  
  /** Stores the discounted items for easy access in the template */
  discountedItems: BackendCart.ProductResponse[] = [];
  
  /** Subject for handling component destruction and canceling subscriptions */
  private destroy$ = new Subject<void>();
  
  /**
   * Constructor
   * 
   * @param cartService - Service for cart operations
   * @param checkoutService - Service for checkout operations
   * @param notificationService - Service for displaying notifications
   * @param router - Angular router for navigation
   */
  constructor(
    private cartService: CartService,
    private checkoutService: CheckoutService,
    private notificationService: NotificationService,
    private router: Router
  ) { }
  
  /**
   * Lifecycle hook that is called after component initialization
   * Subscribes to cart updates
   */
  ngOnInit(): void {
    this.cartService.getCart()
      .pipe(takeUntil(this.destroy$))
      .subscribe(cart => {
        this.cart = cart;
        // Reset discount calculation when cart changes
        this.discountedCartResponse = null;
        this.discountedItems = [];
      });
  }
  
  /**
   * Lifecycle hook that is called before component destruction
   * Cleans up subscriptions to prevent memory leaks
   */
  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }
  
  /**
   * Calculate discounts for the current cart
   * Makes an API call to get discounted prices for all items
   */
  calculateDiscounts(): void {
    if (this.cart.items.length === 0) {
      this.notificationService.warning('Your cart is empty');
      return;
    }
    
    this.isCalculatingDiscounts = true;
    
    this.checkoutService.calculateDiscounts()
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (response) => {
          this.isCalculatingDiscounts = false;
          
          if (response.success && response.data) {
            console.log('Discount response received:', response);
            
            // Validate response data
            if (!response.data.products || response.data.products.length === 0) {
              this.notificationService.warning('No product information received in discount calculation');
              return;
            }
            
            this.discountedCartResponse = response;
            this.discountedItems = response.data.products || [];
            
            // Apply highlight animation to discounted items
            setTimeout(() => {
              const discountRows = document.querySelectorAll('.discount-row, .discounted-total');
              discountRows.forEach(row => {
                row.classList.add('highlight-animation');
              });
            }, 100);
            
            if (response.data.totalDiscount > 0) {
              this.notificationService.success(`Discounts applied! You saved ${this.formatCurrency(response.data.totalDiscount)}`);
            } else {
              this.notificationService.info('No applicable discounts found for your order.');
            }
          } else {
            console.error('Invalid discount response:', response);
            this.notificationService.error('Failed to calculate discounts: ' + (response.message || 'Unknown error'));
          }
        },
        error: (error) => {
          this.isCalculatingDiscounts = false;
          console.error('Discount calculation error:', error);
          
          // Try to extract validation errors if available
          let errorMessage = 'Failed to calculate discounts. Please try again.';
          
          if (error.error && error.error.errors && Array.isArray(error.error.errors)) {
            // Check for date-specific errors
            const dateErrors = error.error.errors.filter((err: any) => 
              err.propertyName === 'Date' && err.errorMessage?.includes('future'));
              
            if (dateErrors.length > 0) {
              // Special handling for date in the future errors
              errorMessage = 'Error: The system date appears to be incorrect. Please check your device date and time settings.';
              console.warn('Date validation error detected, current device date:', new Date().toString());
            } else {
              // Format other validation errors
              const validationErrors = error.error.errors
                .map((err: any) => `${err.propertyName}: ${err.errorMessage}`)
                .join('\n');
              
              if (validationErrors) {
                errorMessage = `Validation errors:\n${validationErrors}`;
              }
            }
          }
          
          this.notificationService.error(errorMessage);
        }
      });
  }
  
  /**
   * Format a number as currency string
   * 
   * @param value - The numeric value to format
   * @returns Formatted currency string
   */
  formatCurrency(value: number): string {
    return new Intl.NumberFormat('en-US', { style: 'currency', currency: 'USD' }).format(value);
  }
  
  /**
   * Place an order with the current cart contents
   * Validates the cart, processes the checkout, and handles the response
   */
  placeOrder(): void {
    if (this.cart.items.length === 0) {
      this.notificationService.warning('Your cart is empty');
      return;
    }
    
    // If discounts haven't been calculated yet, calculate them first
    if (!this.discountedCartResponse) {
      this.notificationService.info('Calculating discounts before completing your order...');
      this.calculateDiscounts();
      return;
    }
    
    this.isProcessing = true;
    
    this.checkoutService.processCheckout()
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (response) => {
          this.isProcessing = false;
          if (response.success) {
            // Redirect to confirmation page or order history
            this.router.navigate(['/cart/orders']);
          }
        },
        error: (error) => {
          this.isProcessing = false;
          console.error('Checkout error:', error);
          this.notificationService.error('Failed to place order. Please try again.');
        }
      });
  }
} 