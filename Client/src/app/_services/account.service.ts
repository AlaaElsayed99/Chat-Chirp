import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, map } from 'rxjs';
import { User } from '../_models/user';
import { ToastrService } from 'ngx-toastr';
import { environment } from 'src/environments/environment.development';


@Injectable({
  providedIn: 'root'
})
export class AccountService {
  baseUrl=environment.baseUrl;
  private currentUserSource= new BehaviorSubject<User |  null>(null);
  currnetUser$=this.currentUserSource.asObservable();
  constructor( private http:HttpClient, private toastr:ToastrService) { }
  login(model:any){
    return this.http.post<User>(this.baseUrl+'account/login',model).pipe(
      map((response:User)=>{
        const user=response;
        if(user){
          localStorage.setItem('user',JSON.stringify(user));
          this.currentUserSource.next(user);
          this.toastr.success("login success");
        }
        
      })
    )
  }
  register(model:any){
    return this.http.post<User>(this.baseUrl+'account/register',model).pipe(
    map(user=>{
      if(user){
        localStorage.setItem('user',JSON.stringify(user));
        this.currentUserSource.next(user);
      }
    })
    
    )

  }
  setCurrentUser(user: User){
    this.currentUserSource.next(user);
  }
  logout(){
    localStorage.removeItem("user");
    this.currentUserSource.next(null);
  }
}
