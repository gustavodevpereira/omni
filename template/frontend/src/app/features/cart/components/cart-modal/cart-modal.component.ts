import { Component, OnInit, OnDestroy } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { FormsModule } from '@angular/forms';
import { MatDialogModule } from '@angular/material/dialog';
import { CartService } from '../../services/cart.service';
import { Cart, CartItem } from '../../services/cart.service';
import { A11yModule } from '@angular/cdk/a11y';
import { Router, NavigationStart } from '@angular/router';
import { Subscription } from 'rxjs';
import { filter } from 'rxjs/operators';

/**
 * Cart Modal Component
 * 
 * Displays the current items in the shopping cart, allows quantity management,
 * removing items, and proceeding to checkout.
 */
@Component({
  selector: 'app-cart-modal',
  standalone: true,
  imports: [
    CommonModule,
    MatDialogModule,
    MatButtonModule,
    MatIconModule,
    FormsModule,
    A11yModule
  ],
  templateUrl: './cart-modal.component.html',
  styleUrls: ['./cart-modal.component.scss']
})
export class CartModalComponent implements OnInit, OnDestroy {
  /** The current cart data */
  cart: Cart = { items: [], totalItems: 0, totalPrice: 0 };
  loading = false;
  private routerSubscription: Subscription | null = null;
  private cartSubscription: Subscription | null = null;

  /**
   * Constructor
   * @param cartService - Service for managing cart operations
   * @param router - Angular router for navigation
   * @param dialogRef - Reference to the dialog
   */
  constructor(
    private cartService: CartService,
    public dialogRef: MatDialogRef<CartModalComponent>,
    private router: Router
  ) {
    // Close dialog when navigation happens
    this.routerSubscription = this.router.events
      .pipe(filter(event => event instanceof NavigationStart))
      .subscribe(() => {
        this.close();
      });
      
    // Ensure clicking backdrop closes the dialog
    this.dialogRef.backdropClick().subscribe(() => {
      this.close();
    });
    
    // Allow closing with escape key
    this.dialogRef.disableClose = false;
  }

  /**
   * Initialize the component
   */
  ngOnInit(): void {
    this.loadCart();
    
    // Set initial focus and ensure mobile footer is properly focused
    setTimeout(() => {
      const closeButton = document.querySelector('.close-button') as HTMLElement;
      if (closeButton) {
        closeButton.focus();
      }
      
      // Ensure mobile footer button is focusable
      const mobileButton = document.querySelector('.mobile-footer button') as HTMLElement;
      if (mobileButton) {
        mobileButton.setAttribute('tabindex', '0');
      }
    }, 100);
  }

  /**
   * Clean up
   */
  ngOnDestroy(): void {
    if (this.routerSubscription) {
      this.routerSubscription.unsubscribe();
    }
    
    if (this.cartSubscription) {
      this.cartSubscription.unsubscribe();
    }
  }

  /**
   * Load the cart data
   */
  loadCart(): void {
    this.loading = true;
    this.cartSubscription = this.cartService.getCart().subscribe(cart => {
      this.cart = cart;
      this.loading = false;
    });
  }

  /**
   * Update the quantity of a cart item
   * @param item - The cart item to update quantity for
   * @param quantity - The new quantity for the item
   */
  updateQuantity(item: CartItem, quantity: number): void {
    this.cartService.updateQuantity(item.product.id, quantity);
  }

  /**
   * Remove an item from the cart
   * @param item - The item to remove from the cart
   */
  removeItem(item: CartItem): void {
    this.cartService.removeFromCart(item.product.id);
  }

  /**
   * Proceed to checkout
   */
  checkout(): void {
    this.dialogRef.close();
    this.router.navigate(['/cart/checkout']);
  }

  /**
   * Close the dialog and continue shopping
   */
  continueShopping(): void {
    this.close();
  }

  /**
   * Close the dialog
   */
  close(): void {
    this.dialogRef.close();
  }
} 