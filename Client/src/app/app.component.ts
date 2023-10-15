import { HttpClient,HttpHeaders  } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { AccountService } from './_services/account.service';
import { User } from './_models/user';
import { JsonPipe } from '@angular/common';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit{
  /**
   *
   */
  constructor(private accountService:AccountService) {


  }
  
  title="Communctaion App";
  baseUrl="https://localhost:44362/api/"
  ngOnInit(): void {
    this.setCurrentUser();
    
  }
  
  
  setCurrentUser(){
    const userString =localStorage.getItem('user');
    if(!userString){
      return;
    }
    const user:User=JSON.parse(userString);
    this.accountService.setCurrentUser(user);
  }
  
 

}

