import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { Subject, takeUntil } from 'rxjs';
import { SharedModule } from '../../../../shared/shared.module';
import { CartService, Cart, CartItem } from '../../services/cart.service';
import { NotificationService } from '../../../../core/services/notification.service';
import { AuthService } from '../../../../core/services/auth.service';

/**
 * CartComponent
 * 
 * Displays the user's shopping cart with items, quantities, and totals.
 * Allows modifying quantities, removing items, and proceeding to checkout.
 */
@Component({
  selector: 'app-cart',
  standalone: true,
  imports: [
    CommonModule,
    SharedModule
  ],
  templateUrl: './cart.component.html',
  styleUrls: ['./cart.component.scss']
})
export class CartComponent implements OnInit, OnDestroy {
  cart: Cart = { items: [], totalItems: 0, totalPrice: 0 };
  isLoading = false;
  isProcessingCheckout = false;
  
  private readonly destroy$ = new Subject<void>();

  constructor(
    private cartService: CartService,
    private authService: AuthService,
    private router: Router,
    private notificationService: NotificationService
  ) { }

  ngOnInit(): void {
    // Subscribe to cart updates
    this.cartService.getCart()
      .pipe(takeUntil(this.destroy$))
      .subscribe((cartData: Cart) => {
        this.cart = cartData;
      });
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  /**
   * Get cart items as array
   */
  get cartItems(): CartItem[] {
    return this.cart?.items || [];
  }

  /**
   * Get total number of items in cart
   */
  get totalItems(): number {
    return this.cart?.totalItems || 0;
  }

  /**
   * Update item quantity
   */
  updateQuantity(item: CartItem, newQuantity: number): void {
    if (newQuantity < 1) {
      return;
    }
    
    // Limitar a quantidade máxima para 99 (opcional)
    if (newQuantity > 99) {
      newQuantity = 99;
      this.notificationService.info('Maximum quantity reached');
    }
    
    this.cartService.updateQuantity(item.product.id, newQuantity);
    this.notificationService.info(`Updated quantity to ${newQuantity}`);
  }

  /**
   * Remove item from cart
   */
  removeItem(productId: string): void {
    this.cartService.removeFromCart(productId);
  }

  /**
   * Clear all items from cart
   */
  clearCart(): void {
    this.cartService.clearCart();
  }

  /**
   * Navigate to checkout
   */
  checkout(): void {
    if (!this.cart || this.cart.items.length === 0) {
      this.notificationService.warning('Your cart is empty');
      return;
    }
    
    if (!this.authService.isAuthenticated()) {
      this.notificationService.info('Please login to checkout');
      this.router.navigate(['/auth/login'], { queryParams: { returnUrl: '/cart/checkout' } });
      return;
    }
    
    this.router.navigate(['/cart/checkout']);
  }

  /**
   * Continue shopping (go back to products)
   */
  continueShopping(): void {
    this.router.navigate(['/products']);
  }
} 