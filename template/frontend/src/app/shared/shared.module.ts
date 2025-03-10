import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { MaterialModule } from './material.module';

/**
 * Shared Module
 * 
 * This module contains common components, directives, and pipes
 * that are used across multiple feature modules in the application.
 * It also re-exports commonly used Angular modules and third-party libraries.
 */
@NgModule({
  declarations: [
    // Shared components, directives, and pipes will go here
  ],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule,
    MaterialModule
  ],
  exports: [
    // Re-export modules for use in feature modules
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule,
    MaterialModule,
    // Export shared components, directives, and pipes here
  ]
})
export class SharedModule { } 