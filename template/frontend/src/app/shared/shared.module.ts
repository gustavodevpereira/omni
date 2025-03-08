import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { HttpClientModule } from '@angular/common/http';
import { MaterialModule } from './material.module';

// Feature modules
import { SharedPipesModule } from './pipes/shared-pipes.module';
import { SharedDirectivesModule } from './directives/shared-directives.module';
import { SharedComponentsModule } from './components/shared-components.module';
import { SharedValidatorsModule } from './validators/shared-validators.module';

/**
 * Primary shared module for the application.
 * 
 * This module aggregates all shared resources that are used throughout the application,
 * including common Angular modules, custom components, directives, pipes, and validators.
 * 
 * By importing this single module, feature modules gain access to all shared functionality,
 * which promotes consistency and reduces boilerplate imports across the application.
 */
@NgModule({
  imports: [
    // Angular modules
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule,
    HttpClientModule,
    
    // Custom modules
    MaterialModule,
    SharedPipesModule,
    SharedDirectivesModule,
    SharedComponentsModule,
    SharedValidatorsModule
  ],
  exports: [
    // Angular modules
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule,
    HttpClientModule,
    
    // Custom modules
    MaterialModule,
    SharedPipesModule,
    SharedDirectivesModule,
    SharedComponentsModule,
    SharedValidatorsModule
  ]
})
export class SharedModule { }
