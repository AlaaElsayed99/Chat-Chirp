import { Injectable } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';

@Injectable({
  providedIn: 'root'
})
export class BusyService {
busyRequstCount=0;
  constructor(private spinnerService:NgxSpinnerService) { }
  busy(){
    this.busyRequstCount++;
    this.spinnerService.show(undefined,{
      type:'square-jelly-box',
      bdColor:'rgba(255,255,255,0)',
      color:'#333333'
    })
  }
  idle(){
    this.busyRequstCount--;
    if(this.busyRequstCount<=0){
      this.busyRequstCount=0;
      this.spinnerService.hide();
    }
  }
}
