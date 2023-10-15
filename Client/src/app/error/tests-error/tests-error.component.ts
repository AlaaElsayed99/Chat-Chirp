import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-tests-error',
  templateUrl: './tests-error.component.html',
  styleUrls: ['./tests-error.component.css']
})
export class TestsErrorComponent implements OnInit {
  /**
   *
   */
  constructor(private http: HttpClient) {
    
    
  }
  ngOnInit(): void {
    
  }
  baseUrl="https://localhost:44362/api/";
  validationError:string[]=[];
  Get404error(){
    this.http.get(this.baseUrl+'buggy/not-found').subscribe({
      next:response=>console.log(response),
      error:err=>console.log(err),
      
      
    })
  }
  Get500error(){
    this.http.get(this.baseUrl+'buggy/server-error').subscribe({
      next:response=>console.log(response),
      error:err=>console.log(err),
      
      
    })
  }
  GetBadRequestError(){
    this.http.get(this.baseUrl+'buggy/bad-request').subscribe({
      next:response=>console.log(response),
      error:err=>console.log(err),
      
      
    })
  }
  Get401error(){
    this.http.get(this.baseUrl+'buggy/auth').subscribe({
      next:response=>console.log(response),
      error:err=>console.log(err),
      
      
    })
  }
  Get400ValidationError(){
    this.http.get(this.baseUrl+'account/register',{}).subscribe({
      next:response=>console.log(response),
      error:err=>{
        console.log(err);
        this.validationError=err; 
      },
      
      
    })
  }
}
