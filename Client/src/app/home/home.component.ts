import { HttpClient } from '@angular/common/http';
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
  constructor(private http:HttpClient) {
 
  }
  ngOnInit(): void {
    // this.spinner.show();

    // setTimeout(() => {
    //   /** spinner ends after 5 seconds */
    //   this.spinner.hide();
    // }, 2000);
    this.gerUsers();
    
  }
  registerToggle(){
    this.registerMode=!this.registerMode
  }
  gerUsers(){
    this.http.get(this.baseUrl+'user').subscribe({
      next:response=> this.users=response,
      error:err => console.log(err),
      complete:()=>console.log("not complete")
    })
  }
  cancelRegisterMode(event:boolean){
    this.registerMode=event;
  }
  
  

}
