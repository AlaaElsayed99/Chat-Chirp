import { Component, OnInit } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  baseUrl="https://localhost:44362/api/"
  registerMode=false;
  users:any;
  constructor() {
 
  }
  ngOnInit(): void {
    // this.spinner.show();

    // setTimeout(() => {
    //   /** spinner ends after 5 seconds */
    //   this.spinner.hide();
    // }, 2000);
    
  }
  registerToggle(){
    this.registerMode=!this.registerMode
  }
 
  cancelRegisterMode(event:boolean){
    this.registerMode=event;
  }
  
  

}
