import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { ToastrModule } from 'ngx-toastr';
import { NgxSpinnerModule } from 'ngx-spinner';
import { TabsModule } from 'ngx-bootstrap/tabs';



@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    BsDropdownModule,
    TabsModule.forRoot(),
    ToastrModule.forRoot({
      timeOut: 5000,
      positionClass: 'toast-bottom-right',
      preventDuplicates: true,
    }),
    NgxSpinnerModule,

  ],

  exports:[
    BsDropdownModule,
    ToastrModule,
    TabsModule
    // NgxSpinnerModule
  ]
})
export class SharedModule { }
