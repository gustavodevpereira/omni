import { Directive, ElementRef, HostListener } from '@angular/core';

/**
 * Restricts input to numeric values only
 * 
 * Usage:
 * <input numberOnly>
 */
@Directive({
  selector: 'input[numberOnly]',
  standalone: true
})
export class NumberOnlyDirective {
  constructor(private el: ElementRef) {}

  @HostListener('input', ['$event'])
  onInputChange(event: Event): void {
    const initialValue = this.el.nativeElement.value;
    this.el.nativeElement.value = initialValue.replace(/[^0-9]*/g, '');
    
    if (initialValue !== this.el.nativeElement.value) {
      event.stopPropagation();
    }
  }
} 