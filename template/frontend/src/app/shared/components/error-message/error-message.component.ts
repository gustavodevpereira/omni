import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatIconModule } from '@angular/material/icon';

/**
 * Reusable error message component
 * 
 * Usage:
 * <app-error-message [message]="errorMessage"></app-error-message>
 */
@Component({
  selector: 'app-error-message',
  standalone: true,
  imports: [CommonModule, MatIconModule],
  template: `
    <div *ngIf="message" class="error-container">
      <mat-icon class="error-icon">error</mat-icon>
      <span class="error-text">{{ message }}</span>
    </div>
  `,
  styles: [`
    .error-container {
      display: flex;
      align-items: center;
      background-color: #ffebee;
      color: #d32f2f;
      padding: 12px;
      border-radius: 4px;
      margin: 10px 0;
    }
    
    .error-icon {
      margin-right: 8px;
    }
    
    .error-text {
      font-size: 14px;
    }
  `]
})
export class ErrorMessageComponent {
  @Input() message: string | null = null;
} 