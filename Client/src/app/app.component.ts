import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit{
  /**
   *
   */
  constructor(private http:HttpClient) {


  }
  title="alaa";
  baseUrl="https://localhost:44362/api/"
  users:any;
  ngOnInit(): void {
    this.http.get(this.baseUrl+'User').subscribe({
      next:response=> this.users=response,
      error :(err)=> console.log ("Error", err),complete :()=>{}
    });
    
  }
  
 

}

