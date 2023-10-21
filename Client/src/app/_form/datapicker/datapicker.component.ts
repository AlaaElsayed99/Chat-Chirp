import { Component, Input, Self } from '@angular/core';
import { ControlValueAccessor, FormControl, NgControl } from '@angular/forms';
import { BsDatepickerConfig } from 'ngx-bootstrap/datepicker';
import { ControlValueAccessorModel } from 'ngx-bootstrap/timepicker/models';

@Component({
  selector: 'app-datapicker',
  templateUrl: './datapicker.component.html',
  styleUrls: ['./datapicker.component.css']
})
export class DatapickerComponent implements ControlValueAccessor {
  @Input() label='';
  @Input() maxDate:Date |undefined;
  @Input() minDate: Date| undefined;
  bsConfig: Partial<BsDatepickerConfig> | undefined;

  constructor(@Self() public ngControl:NgControl) {
    this.ngControl.valueAccessor=this;
    this.bsConfig={
      containerClass: 'theme-blue',
      dateInputFormat:'DD MMMMM YYYY',
      
    }

  }
  writeValue(obj: any): void {
  }
  registerOnChange(fn: any): void {
  }
  registerOnTouched(fn: any): void {
  }
  get control():FormControl{
    return this.ngControl.control as FormControl

  }

}
