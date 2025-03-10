import { Component, OnInit, OnDestroy, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { MatTableModule, MatTableDataSource } from '@angular/material/table';
import { MatSortModule } from '@angular/material/sort';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatChipsModule } from '@angular/material/chips';
import { MatDividerModule } from '@angular/material/divider';
import { MatExpansionModule } from '@angular/material/expansion';
import { MatTooltipModule } from '@angular/material/tooltip';

import { SharedModule } from '../../../../shared/shared.module';
import { CheckoutService } from '../../services/checkout.service';
import { BackendCart } from '../../../../core/api/services/cart-api.service';
import { NotificationService } from '../../../../core/services/notification.service';

/**
 * Order History Component
 * 
 * @description Displays a paginated list of user's past orders with detailed information
 * including order status, items, and pricing details. Provides functionality to view order details, 
 * reorder items, and print invoices.
 * 
 * @usageNotes
 * This component should be used in authenticated routes only as it requires user's order history.
 * It supports responsive design for mobile and desktop views.
 * 
 * @example
 * <app-order-history></app-order-history>
 */
@Component({
  selector: 'app-order-history',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    SharedModule,
    MatTableModule,
    MatSortModule, 
    MatPaginatorModule,
    MatProgressSpinnerModule,
    MatIconModule,
    MatButtonModule,
    MatCardModule,
    MatChipsModule,
    MatDividerModule,
    MatExpansionModule,
    MatTooltipModule
  ],
  templateUrl: './order-history.component.html',
  styleUrls: ['./order-history.component.scss']
})
export class OrderHistoryComponent implements OnInit, OnDestroy {
  /** List of orders retrieved from the backend */
  orders: BackendCart.CartResponse[] = [];
  
  /** Loading state flag */
  isLoading = false;
  
  /** Current page number (0-based indexing, como no ProductListComponent) */
  currentPage = 0;
  
  /** Number of items per page */
  pageSize = 4;
  
  /** Total number of items across all pages */
  totalItems = 0;
  
  /** Total number of pages available */
  totalPages = 0;
  
  /** Columns to be displayed in the products table */
  displayedColumns: string[] = ['productName', 'unitPrice', 'quantity', 'discount', 'total'];
  
  /** Subject for handling component destruction and canceling subscriptions */
  private destroy$ = new Subject<void>();
  
  /**
   * Constructor
   * 
   * @param checkoutService - Service for managing checkout and order history
   * @param notificationService - Service for displaying notifications to the user
   */
  constructor(
    private checkoutService: CheckoutService,
    private notificationService: NotificationService,
    private cdr: ChangeDetectorRef
  ) { }
  
  /**
   * Lifecycle hook that is called after component initialization
   * Loads the initial set of orders
   */
  ngOnInit(): void {
    this.loadOrders();
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
   * Loads orders from the backend with pagination
   * Updates component state with the retrieved data
   */
  loadOrders(): void {
    this.isLoading = true;
    
    const apiPage = this.currentPage + 1;
    
    // Make API request
    this.checkoutService.getOrderHistory(apiPage, this.pageSize)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (response) => {
          // Update component state with response data
          this.orders = response.data;
          console.log(response);
          this.totalItems = response.totalCount;
          this.totalPages = response.totalPages;
          
          // Reset loading state
          this.isLoading = false;
        },
        error: (error) => {
          this.notificationService.error('Failed to load order history. Please try again.');
          
          // Reset loading state and clear data
          this.isLoading = false;
          this.orders = [];
        }
      });
  }
  
  /**
   * Handles pagination events from the MatPaginator
   * Similar to ProductListComponent implementation
   * 
   * @param event - Page event containing pagination information
   */
  handlePageEvent(event: PageEvent): void {
    // Update pagination state
    this.currentPage = event.pageIndex;
    this.pageSize = event.pageSize;
    
    // Reload orders with new parameters
    this.loadOrders();
  }
  
  /**
   * Creates a MatTableDataSource for the products of a specific order
   * 
   * @param order - The order containing products
   * @returns A MatTableDataSource with the order's products
   */
  getProductsDataSource(order: BackendCart.CartResponse): MatTableDataSource<BackendCart.ProductResponse> {
    return new MatTableDataSource(order.products);
  }
  
  /**
   * Determines the material theme color based on order status
   * 
   * @param status - The status string of the order
   * @returns The material theme color name (primary, accent, warn)
   */
  getStatusColor(status: string): string {
    switch (status.toLowerCase()) {
      case 'active':
      case 'approved':
        return 'primary';
      case 'completed':
      case 'delivered':
        return 'primary';
      case 'cancelled':
      case 'rejected':
        return 'warn';
      case 'pending':
      case 'processing':
        return 'accent';
      default:
        return 'primary';
    }
  }
  
  /**
   * Calculates the total number of items in an order
   * 
   * @param order - The order to calculate total items for
   * @returns The sum of quantities of all products in the order
   */
  getTotalItems(order: BackendCart.CartResponse): number {
    return order.products.reduce((total, product) => total + product.quantity, 0);
  }
} 