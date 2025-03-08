import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

/**
 * Module that provides form validation functionality across the application.
 * 
 * Contains custom validators that can be used with Angular's Reactive Forms.
 * These validators extend the built-in validation capabilities with
 * application-specific validation rules.
 */
@NgModule({
  imports: [
    CommonModule
  ],
  exports: []
})
export class SharedValidatorsModule { } 