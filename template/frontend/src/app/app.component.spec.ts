import { TestBed } from '@angular/core/testing';
import { RouterModule } from '@angular/router';
import { AppComponent } from './app.component';
import { AuthService } from './core/services/auth.service';
import { BehaviorSubject } from 'rxjs';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { CommonModule } from '@angular/common';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';

describe('AppComponent', () => {
  let authServiceMock: jasmine.SpyObj<AuthService>;

  beforeEach(async () => {
    // Create mock for AuthService
    authServiceMock = jasmine.createSpyObj('AuthService', ['logout']);
    
    // Create BehaviorSubjects for the observables
    const isAuthenticatedSubject = new BehaviorSubject<boolean>(false);
    const currentUserSubject = new BehaviorSubject<any>(null);
    
    // Set up the mock properties
    Object.defineProperty(authServiceMock, 'isAuthenticated$', {
      get: () => isAuthenticatedSubject.asObservable()
    });
    
    Object.defineProperty(authServiceMock, 'currentUser$', {
      get: () => currentUserSubject.asObservable()
    });

    await TestBed.configureTestingModule({
      imports: [
        // Import the component since it's standalone
        AppComponent,
        // Import required modules
        RouterModule.forRoot([]),
        CommonModule,
        MatToolbarModule,
        MatButtonModule,
        MatIconModule,
        NoopAnimationsModule
      ],
      providers: [
        { provide: AuthService, useValue: authServiceMock }
      ]
    }).compileComponents();
  });

  it('should create the app', () => {
    const fixture = TestBed.createComponent(AppComponent);
    const app = fixture.componentInstance;
    expect(app).toBeTruthy();
  });

  it(`should have as title 'Angular Store'`, () => {
    const fixture = TestBed.createComponent(AppComponent);
    const app = fixture.componentInstance;
    expect(app.title).toEqual('Angular Store');
  });

  it('should render the toolbar', () => {
    const fixture = TestBed.createComponent(AppComponent);
    fixture.detectChanges();
    const compiled = fixture.nativeElement as HTMLElement;
    expect(compiled.querySelector('mat-toolbar')).toBeTruthy();
  });

  it('should call logout when logout button is clicked', () => {
    const fixture = TestBed.createComponent(AppComponent);
    const app = fixture.componentInstance;
    app.logout();
    expect(authServiceMock.logout).toHaveBeenCalled();
  });
});
