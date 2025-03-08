import { AfterContentInit, Directive, ElementRef, Input } from '@angular/core';

/**
 * Automatically focuses an element when it's rendered
 * 
 * Usage:
 * <input autoFocus>
 * <input [autoFocus]="shouldFocus">
 */
@Directive({
  selector: '[autoFocus]'
})
export class AutoFocusDirective implements AfterContentInit {
  @Input() autoFocus: boolean = true;

  constructor(private el: ElementRef) {}

  ngAfterContentInit(): void {
    if (this.autoFocus) {
      setTimeout(() => {
        this.el.nativeElement.focus();
      }, 100);
    }
  }
} 