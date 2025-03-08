import { Component, Input, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';

/**
 * Reusable empty state component for when lists have no items
 * 
 * Usage:
 * <app-empty-state 
 *   icon="shopping_cart" 
 *   message="Your cart is empty" 
 *   [actionButton]="true"
 *   actionLabel="Browse Products"
 *   (actionClick)="navigateToProducts()">
 * </app-empty-state>
 */
@Component({
  selector: 'app-empty-state',
  standalone: true,
  imports: [CommonModule, MatIconModule, MatButtonModule],
  template: `
    <div class="empty-state-container">
      <mat-icon class="empty-state-icon">{{ icon }}</mat-icon>
      <h3 class="empty-state-message">{{ message }}</h3>
      <p *ngIf="subMessage" class="empty-state-submessage">{{ subMessage }}</p>
      <button 
        *ngIf="actionButton" 
        mat-raised-button 
        [color]="actionColor"
        (click)="onActionClick()">
        {{ actionLabel }}
      </button>
    </div>
  `,
  styles: [`
    .empty-state-container {
      display: flex;
      flex-direction: column;
      align-items: center;
      justify-content: center;
      padding: 40px 20px;
      text-align: center;
    }
    
    .empty-state-icon {
      font-size: 64px;
      height: 64px;
      width: 64px;
      color: #9e9e9e;
      margin-bottom: 16px;
    }
    
    .empty-state-message {
      font-size: 20px;
      font-weight: 500;
      margin: 0 0 8px 0;
      color: #424242;
    }
    
    .empty-state-submessage {
      font-size: 14px;
      color: #757575;
      margin: 0 0 24px 0;
    }
  `]
})
export class EmptyStateComponent {
  @Input() icon: string = 'inbox';
  @Input() message: string = 'No items found';
  @Input() subMessage?: string;
  @Input() actionButton: boolean = false;
  @Input() actionLabel: string = 'Action';
  @Input() actionColor: 'primary' | 'accent' | 'warn' = 'primary';
  @Output() actionClick = new EventEmitter<void>();

  onActionClick(): void {
    this.actionClick.emit();
  }
} 