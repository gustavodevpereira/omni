import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { provideRouter } from '@angular/router';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { CoreModule } from './core/core.module';
import { routes } from './app-routing.module';

/**
 * Root Module for the application
 * 
 * Bootstraps the AppComponent and provides app-wide configuration.
 * The CoreModule is imported here to ensure singleton services are available app-wide.
 */
@NgModule({
  declarations: [],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    AppRoutingModule,
    CoreModule
  ],
  providers: [
    provideRouter(routes)
  ]
})
export class AppModule { }
